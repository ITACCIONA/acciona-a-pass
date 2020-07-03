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
    public class RegisterTemperatureMedition
    {
        public class RegisterTemperatureMeditionRequest : IRequest<bool>
        {
            public int IdEmployee { get; set; }
            public bool IsTemperatureOverThreshold { get; set; }
            public DateTimeOffset? MeditionDateTime { get; set; }
        }

        public class RegisterTemperatureMeditionValidator : AbstractValidator<RegisterTemperatureMeditionRequest>
        {
            public RegisterTemperatureMeditionValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class RegisterTemperatureMeditionCommandHandler : BaseCommandHandler<RegisterTemperatureMeditionRequest, bool>
        {
            private readonly IRepository<SeguimientoMedico> repositorySeguimientos;
            private readonly IRepository<Empleado> repositoryEmpleado;
            private readonly IRepository<ParametroMedico> repositoryParametroMedico;

            /// <summary>
            /// Constructor
            /// </summary>
            public RegisterTemperatureMeditionCommandHandler(IRepository<SeguimientoMedico> repositorySeguimientos, IRepository<Empleado> repositoryEmpleado, IRepository<ParametroMedico> repositoryParametroMedico)
            {
                this.repositorySeguimientos = repositorySeguimientos;
                this.repositoryEmpleado = repositoryEmpleado;
                this.repositoryParametroMedico = repositoryParametroMedico;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async override Task<bool> Handle(RegisterTemperatureMeditionRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await repositoryEmpleado
                    .GetBy(p => p.Id == request.IdEmployee)
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

                ParametroMedico paramTempe = await repositoryParametroMedico.GetAll().FirstOrDefaultAsync(c => c.Nombre == ParametroMedico.ParameterTypes.TemperaturaAlta.ToString()).ConfigureAwait(false);

                SeguimientoMedico seguimiento = new SeguimientoMedico()
                {
                    IdFichaMedica = empleado.IdFichaMedica.Value,
                    Comentarios = "Temperature Medition",
                    Activo = true,
                    FechaSeguimiento = request.MeditionDateTime.HasValue ? request.MeditionDateTime.Value : DateTimeOffset.Now
                };

                seguimiento.ValoracionParametroMedico = new List<ValoracionParametroMedico>()
                {
                    new ValoracionParametroMedico()
                    {
                        Valor = request.IsTemperatureOverThreshold,
                        IdParametroMedico = paramTempe.Id,
                    }
                };

                List<SeguimientoMedico> oldSeguimentos = await repositorySeguimientos.GetAll().Where(c => c.IdFichaMedica == empleado.IdFichaMedica.Value
                && c.ValoracionParametroMedico.Select(d => d.IdParametroMedico).Any(e => e == paramTempe.Id)).ToListAsync().ConfigureAwait(false);

                foreach (var item in oldSeguimentos)
                {
                    item.Activo = false;
                    repositorySeguimientos.Update(item);
                }

                repositorySeguimientos.Add(seguimiento);
                await this.repositorySeguimientos.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }
        }
    }
}
