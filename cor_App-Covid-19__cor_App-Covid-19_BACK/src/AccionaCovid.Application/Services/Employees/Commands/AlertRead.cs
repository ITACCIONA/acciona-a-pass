using AccionaCovid.Application.Core;
using AccionaCovid.Application.Resources;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Employees
{
    /// <summary>
    /// Servicio que registra una alerta como leida
    /// </summary>
    public class AlertRead
    {
        /// <summary>
        /// Request
        /// </summary>
        public class AlertReadRequest : IRequest<bool>
        {
            /// <summary>
            /// Identificador del empleado
            /// </summary>
            public int IdEmployee { get; set; }

            /// <summary>
            /// Identificador de la alerta
            /// </summary>
            public int IdAlert { get; set; }
        }

        /// <summary>
        /// Clase de validacion
        /// </summary>
        public class AlertReadRequestValidator : AbstractValidator<AlertReadRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public AlertReadRequestValidator()
            {
                RuleFor(r => r.IdAlert).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.Alerta);
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class AlertReadCommandHandler : BaseCommandHandler<AlertReadRequest, bool>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<AlertaServiciosMedicos> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public AlertReadCommandHandler(IRepository<AlertaServiciosMedicos> repository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(AlertReadRequest request, CancellationToken cancellationToken)
            {
                AlertaServiciosMedicos alerta = await repository.GetAll().FirstOrDefaultAsync(c => c.Id == request.IdAlert && c.IdEmpleado == request.IdEmployee).ConfigureAwait(false);

                if(alerta == null)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.Alerta)
                    });
                }

                alerta.Leido = true;

                repository.Update(alerta);

                await repository.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }
        }
    }
}
