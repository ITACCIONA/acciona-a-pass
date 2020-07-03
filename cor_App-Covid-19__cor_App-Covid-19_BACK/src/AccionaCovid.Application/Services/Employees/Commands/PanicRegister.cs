using AccionaCovid.Application.Core;
using AccionaCovid.Application.Resources;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using AccionaCovid.Domain.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Employees
{
    /// <summary>
    /// Servicio que registra una alerta al responsable del empelado y para el estado a Observacion
    /// </summary>
    public class PanicRegister
    {
        /// <summary>
        /// Request
        /// </summary>
        public class PanicRegisterRequest : IRequest<bool>
        {
            /// <summary>
            /// Empleado
            /// </summary>
            public int IdEmployee { get; set; }

            /// <summary>
            /// Fecha de la alerta
            /// </summary>
            public DateTimeOffset CurrentDeviceDateTime { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class GetPassportValidator : AbstractValidator<PanicRegisterRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public GetPassportValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class PanicRegisterCommandHandler : BaseCommandHandler<PanicRegisterRequest, bool>
        {
            /// <summary>
            /// Repositorio
            /// </summary>
            private readonly IRepository<AlertaServiciosMedicos> repository;

            /// <summary>
            /// Repositorio
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public PanicRegisterCommandHandler(IRepository<AlertaServiciosMedicos> repository, IRepository<Empleado> repositoryEmpleado)
            {
                this.repository = repository;
                this.repositoryEmpleado = repositoryEmpleado;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(PanicRegisterRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                // CReamos la alerta al empleado responsable
                if (empleado.IdFichaLaboralNavigation != null && empleado.IdFichaLaboralNavigation.IdResponsableDirecto.HasValue)
                {
                    Empleado empleadoRespo = await repositoryEmpleado.GetAll().FirstOrDefaultAsync(c => c.Id == empleado.IdFichaLaboralNavigation.IdResponsableDirecto.Value).ConfigureAwait(false);

                    if (empleadoRespo != null)
                    {
                        // Comprobación de que en los últimos síntomas, alguno fue true----------------------
                        var resultadosSintomas = await repositoryEmpleado.GetAll()
                            .Include(e => e.IdFichaMedicaNavigation)
                                .ThenInclude(fm => fm.ResultadoEncuestaSintomas)
                            .Where(e => e.Id == request.IdEmployee)
                            .Select(e => e.IdFichaMedicaNavigation)
                            .Where(fm => !fm.Deleted)
                            .SelectMany(fm => fm.ResultadoEncuestaSintomas)
                            .Where(res => !res.Deleted)
                            .ToListAsync().ConfigureAwait(false);

                        var ultimosResul = resultadosSintomas.GroupBy(res => res.LastActionDate.ToString("yyyyMMdd_HHmmss.fff"))
                            .OrderByDescending(x => x.Key)
                            .Select(c => new { c.Key, anyTrue = c.Any(res => res.Valor) })
                            .FirstOrDefault();

                        if (ultimosResul == null || ultimosResul.anyTrue == false)
                            return true; // lo mejor sería devolver false pero no se sabe que espera la app
                        // -----------------------------------------------------------------------------------

                        // creamos la alerta al empleado responsable

                        AlertaServiciosMedicos newAlerta = new AlertaServiciosMedicos()
                        {
                            Comentario = string.Format(ValidatorsMessages.PANIC_COMMIT_ALERT, empleado.NombreCompleto),
                            Titulo = ValidatorsMessages.PANIC_TITLE_ALERT,
                            FechaNotificacion = request.CurrentDeviceDateTime,
                            IdEmpleado = empleadoRespo.Id,
                            Leido = false
                        };

                        repository.Add(newAlerta);

                        await repository.SaveChangesAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        throw new MultiMessageValidationException(new ErrorMessage()
                        {
                            Code = "PANIC_ALERT_NO_RESPONSABLE",
                            Message = ValidatorsMessages.PANIC_ALERT_NO_RESPONSABLE
                        });
                    }
                }
                else
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "PANIC_ALERT_NO_RESPONSABLE",
                        Message = ValidatorsMessages.PANIC_ALERT_NO_RESPONSABLE
                    });
                }

                return true;
            }

            /// <summary>
            /// Metodo que valida si existe el empleado y su ficha
            /// </summary>
            /// <param name="idEmpleado"></param>
            /// <returns></returns>
            private async Task<Empleado> ValidacionEmpleado(int idEmpleado)
            {
                Empleado empleado = await repositoryEmpleado
                    .GetBy(p => p.Id == idEmpleado)
                    .Include(e => e.Pasaporte)
                    .Include(c => c.IdFichaLaboralNavigation)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                if (empleado == null)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.Empleado)
                    });
                }

                return empleado;
            }
        }
    }
}
