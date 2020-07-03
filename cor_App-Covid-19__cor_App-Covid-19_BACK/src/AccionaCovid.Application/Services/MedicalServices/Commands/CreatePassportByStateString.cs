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

namespace AccionaCovid.Application.Services.MedicalServices
{
    public class CreatePassportByStateString
    {
        public class CreatePassportByStateStringRequest : IRequest<bool>
        {
            public int IdEmployee { get; set; }
            public DateTimeOffset CurrentDeviceDateTime { get; set; }
            public string StateString { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class GetPassportValidator : AbstractValidator<CreatePassportByStateStringRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public GetPassportValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
                RuleFor(v => v.StateString).NotNull().NotEmpty().WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_NULL_OR_EMPTY, ValidatorFields.PassportState);
                RuleFor(v => v.StateString).IsValidLatinString().WithFormatMessage(ValidatorsMessages.VALUE_WRONG_FORMAT, ValidatorFields.PassportState);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class CreatePassportByStateStringCommandHandler : BaseCommandHandler<CreatePassportByStateStringRequest, bool>
        {
            private readonly ICreatePassportService CreatePassportService;
            private readonly IRepository<Empleado> repository;
            private readonly IRepository<EstadoPasaporte> repositoryEstados;
            private readonly ITelemetryService telemetry;

            /// <summary>
            /// Constructor
            /// </summary>
            public CreatePassportByStateStringCommandHandler(ICreatePassportService CreatePassportService, IRepository<Empleado> repository, IRepository<EstadoPasaporte> repositoryEstados, ITelemetryService telemetry)
            {
                this.CreatePassportService = CreatePassportService;
                this.repository = repository;
                this.repositoryEstados = repositoryEstados;
                this.telemetry = telemetry ?? throw new ArgumentNullException(nameof(telemetry));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(CreatePassportByStateStringRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await repository.GetBy(e => e.Id == request.IdEmployee)
                        .Include(e => e.Pasaporte)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                if (empleado == null) // Si no hay ficha médica no está bien registrado.
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.Empleado)
                    });
                }

                var state = await repositoryEstados.GetAll().FirstOrDefaultAsync(s => s.Nombre == request.StateString).ConfigureAwait(false);

                if (state == null)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.PassportState)
                    });
                }

                if(request.StateString == "DeclaracionCaducada1")
                {
                    if (this.Roles.Contains("PRL"))
                        telemetry.TrackEvent("Reseteo_PRL");
                    if(this.Roles.Contains("RRHHCent"))
                        telemetry.TrackEvent("Reseteo_RRHH");
                    if (this.Roles.Contains("GestorContratas"))
                        telemetry.TrackEvent("Reseteo_GestorContratas");

                }

                CreatePassportService.CreateFromChoosenState(empleado, request.CurrentDeviceDateTime, state, true);

                repository.Update(empleado);
                await repository.SaveChangesAsync();

                return true;
            }
        }
    }
}
