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
    public class GetTemperatureMedition
    {
        public class GetTemperatureMeditionRequest : IRequest<GetTemperatureMeditionResponse>
        {
            public int IdEmployee { get; set; }
        }

        public class GetTemperatureMeditionValidator : AbstractValidator<GetTemperatureMeditionRequest>
        {
            public GetTemperatureMeditionValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetTemperatureMeditionResponse
        {
            public List<TemperatureMedition> Meditions { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public GetTemperatureMeditionResponse(List<ValoracionParametroMedico> valoracionesParametros)
            {
                //ordenamos por fecha
                valoracionesParametros = valoracionesParametros.OrderByDescending(c => c.IdSegumientoMedicoNavigation.FechaSeguimiento).ToList();

                this.Meditions = valoracionesParametros.Select(vp => new TemperatureMedition
                {
                    Value = vp.Valor,
                    Date = vp.IdSegumientoMedicoNavigation.FechaSeguimiento
                })
                .ToList();
            }
        }

        public class TemperatureMedition
        {
            public bool Value { get; set; }
            public DateTimeOffset Date { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetTemperatureMeditionCommandHandler : BaseCommandHandler<GetTemperatureMeditionRequest, GetTemperatureMeditionResponse>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<SeguimientoMedico> repositorySeguimientoMedico;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetTemperatureMeditionCommandHandler(IRepository<SeguimientoMedico> repositorySeguimientoMedico, IRepository<Empleado> repositoryEmpleado)
            {
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado));
                this.repositorySeguimientoMedico = repositorySeguimientoMedico ?? throw new ArgumentNullException(nameof(repositorySeguimientoMedico));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<GetTemperatureMeditionResponse> Handle(GetTemperatureMeditionRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                var listSeg = await repositorySeguimientoMedico.GetAll()
                    .Include(c => c.ValoracionParametroMedico)
                    .Where(c => c.IdFichaMedica == empleado.IdFichaMedica
                        && c.ValoracionParametroMedico.Select(d => d.IdParametroMedicoNavigation).Any(e => e.Nombre == ParametroMedico.ParameterTypes.TemperaturaAlta.ToString()))
                    .ToListAsync().ConfigureAwait(false);

                return new GetTemperatureMeditionResponse(listSeg.SelectMany(c => c.ValoracionParametroMedico).ToList());
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
