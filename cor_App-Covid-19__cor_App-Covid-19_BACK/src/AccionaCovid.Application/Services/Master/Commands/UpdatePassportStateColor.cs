using AccionaCovid.Application.Core;
using AccionaCovid.Application.Resources;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Master
{
    /// <summary>
    /// </summary>
    public class UpdatePassportStateColor
    {
        /// <summary>
        /// </summary>
        public class UpdatePassportStateColorRequest : IRequest<bool>
        {
            /// <summary>
            /// Identificaodr del estado
            /// </summary>
            public int IdState { get; set; }

            /// <summary>
            /// Identificaodr del color
            /// </summary>
            public int IdColor { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class UpdatePassportStateColorValidator : AbstractValidator<UpdatePassportStateColorRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public UpdatePassportStateColorValidator()
            {
                RuleFor(r => r.IdState).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEstadoPasaporte);
                RuleFor(r => r.IdColor).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdColor);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class UpdatePassportStateColorCommandHandler : BaseCommandHandler<UpdatePassportStateColorRequest, bool>
        {
            private readonly IRepository<EstadoPasaporte> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public UpdatePassportStateColorCommandHandler(IRepository<EstadoPasaporte> repository)
            {
                this.repository = repository;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(UpdatePassportStateColorRequest request, CancellationToken cancellationToken)
            {
                EstadoPasaporte estado = await repository.GetAll().FirstOrDefaultAsync(c => c.Id == request.IdState).ConfigureAwait(false);

                if (estado == null)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.PassportState)
                    });
                }

                estado.IdColorEstado = request.IdColor;

                repository.Update(estado);

                await repository.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }
        }
    }
}
