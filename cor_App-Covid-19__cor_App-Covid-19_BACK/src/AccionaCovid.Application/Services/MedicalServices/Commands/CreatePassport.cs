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
    public class CreatePassport
    {
        public class CreatePassportRequest : IRequest<bool>
        {
            public int IdEmployee { get; set; }
            public DateTimeOffset CurrentDeviceDateTime { get; set; }
            public int IdPassportState { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class GetPassportValidator : AbstractValidator<CreatePassportRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public GetPassportValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
                RuleFor(r => r.IdPassportState).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEstadoPasaporte);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class CreatePassportCommandHandler : BaseCommandHandler<CreatePassportRequest, bool>
        {
            private readonly ICreatePassportService createPassportService;
            private readonly IRepository<Empleado> repository;
            private readonly IRepository<EstadoPasaporte> repositoryEstados;

            /// <summary>
            /// Constructor
            /// </summary>
            public CreatePassportCommandHandler(ICreatePassportService createPassportService, IRepository<Empleado> repository, IRepository<EstadoPasaporte> repositoryEstados)
            {
                this.createPassportService = createPassportService;
                this.repository = repository;
                this.repositoryEstados = repositoryEstados;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(CreatePassportRequest request, CancellationToken cancellationToken)
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

                var state = await repositoryEstados.GetAll().FirstOrDefaultAsync(s => s.Id == request.IdPassportState).ConfigureAwait(false);

                // validacion de rol
                if (this.Roles.Contains("RRHH") && !this.Roles.Contains("ServicioMedico"))
                {
                    if (state.EstadoId != (int)EstadoPasaporte.AllowedRRHHPassportStatesId.AsintomaticoPcrReconvertidoIGG &&
                        state.EstadoId != (int)EstadoPasaporte.AllowedRRHHPassportStatesId.SintomaticoPcrReconvertidoIGG)
                    {
                        throw new MultiMessageValidationException(new ErrorMessage()
                        {
                            Code = "FORBIDDEN_PASSPORT_FOR_HR",
                            Message = string.Format(ValidatorsMessages.FORBIDDEN_PASSPORT_FOR_HR)
                        });
                    }
                }


                createPassportService.CreateFromChoosenState(empleado, request.CurrentDeviceDateTime, state, true);

                repository.Update(empleado);
                await repository.SaveChangesAsync();

                return true;
            }
        }
    }
}
