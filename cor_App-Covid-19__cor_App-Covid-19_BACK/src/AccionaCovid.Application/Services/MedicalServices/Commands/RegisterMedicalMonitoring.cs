using AccionaCovid.Application.Core;
using AccionaCovid.Application.Resources;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace AccionaCovid.Application.Services.MedicalServices
{
    /// <summary>
    /// Servicio que registra un seguimiento medico
    /// </summary>
    public class RegisterMedicalMonitoring
    {
        /// <summary>
        /// Request
        /// </summary>
        public class RegisterMedicalMonitoringRequest : IRequest<bool>
        {
            /// <summary>
            /// Identificador del empleado
            /// </summary>
            public int IdEmployee { get; set; }

            /// <summary>
            /// Identificador del tipo de paratemtros
            /// </summary>
            public int IdParameterType { get; set; }

            /// <summary>
            /// Valor IGM
            /// </summary>
            public List<MedicalParameterValue> Parametros { get; set; }

            /// <summary>
            /// Fecha de seguimiento
            /// </summary>
            public DateTimeOffset? FechaSeguimiento { get; set; }
        }

        /// <summary>
        /// Request
        /// </summary>
        public class MedicalParameterValue
        {
            /// <summary>
            /// Identificador del parametro
            /// </summary>
            public int IdParameter { get; set; }

            /// <summary>
            /// Valor del parametro
            /// </summary>
            public bool Value { get; set; }
        }

        /// <summary>
        /// Clase de validacion
        /// </summary>
        public class RegisterMedicalMonitoringRequestValidator : AbstractValidator<RegisterMedicalMonitoringRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public RegisterMedicalMonitoringRequestValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
                RuleFor(v => v.Parametros).NotNull().WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_NULL_OR_EMPTY, ValidatorFields.ListaParametros);
                RuleFor(v => v.Parametros).Must(pl => pl?.All(p => p.IdParameter > 0) == true).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.MedicalParameter);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class RegisterMedicalMonitoringCommandHandler : BaseCommandHandler<RegisterMedicalMonitoringRequest, bool>
        {
            /// <summary>
            /// Repositorio
            /// </summary>
            private readonly IRepository<SeguimientoMedico> repositorySeguimientos;

            /// <summary>
            /// Repositorio
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="repositorySeguimientos"></param>
            /// <param name="repositoryEmpleado"></param>
            /// <param name="repositoryParametroMedico"></param>
            public RegisterMedicalMonitoringCommandHandler(IRepository<SeguimientoMedico> repositorySeguimientos, IRepository<Empleado> repositoryEmpleado, IRepository<ParametroMedico> repositoryParametroMedico)
            {
                this.repositorySeguimientos = repositorySeguimientos;
                this.repositoryEmpleado = repositoryEmpleado;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(RegisterMedicalMonitoringRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                SeguimientoMedico seguimiento = new SeguimientoMedico()
                {
                    IdFichaMedica = empleado.IdFichaMedica.Value,
                    Comentarios = "Medicion",
                    Activo = true,
                    FechaSeguimiento = request.FechaSeguimiento ?? DateTime.UtcNow
                };

                foreach (var item in request.Parametros)
                {
                    seguimiento.ValoracionParametroMedico.Add(new ValoracionParametroMedico()
                    {
                        Valor = item.Value,
                        IdParametroMedico = item.IdParameter,
                    });
                }

                // actualizamos los activos a false de todos los seguimientos del tipo que nos envian
                List<SeguimientoMedico> oldSeguimentos = await repositorySeguimientos.GetAll().Where(c => c.IdFichaMedica == empleado.IdFichaMedica.Value
                && c.ValoracionParametroMedico.Select(d => d.IdParametroMedicoNavigation.IdTipoParametro).Any(e => e == request.IdParameterType)).ToListAsync().ConfigureAwait(false);

                foreach (var item in oldSeguimentos)
                {
                    item.Activo = false;
                    repositorySeguimientos.Update(item);
                }

                repositorySeguimientos.Add(seguimiento);
                await repositorySeguimientos.SaveChangesAsync().ConfigureAwait(false);

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
                        .Include(p => p.IdFichaMedicaNavigation)
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

                if (empleado.IdFichaMedica == null)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.FichaMedica)
                    });
                }

                return empleado;
            }
        }
    }
}
