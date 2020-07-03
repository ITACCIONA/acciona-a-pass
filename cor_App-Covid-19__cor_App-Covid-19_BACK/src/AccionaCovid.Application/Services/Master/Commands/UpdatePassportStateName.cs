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
using static AccionaCovid.Domain.Model.Partials.Idioma;

namespace AccionaCovid.Application.Services.Master
{
    /// <summary>
    /// </summary>
    public class UpdatePassportStateName
    {
        /// <summary>
        /// </summary>
        public class UpdatePassportStateNameRequest : IRequest<bool>
        {
            /// <summary>
            /// Identificaodr del estado
            /// </summary>
            public int IdState { get; set; }

            /// <summary>
            /// Nombre en español
            /// </summary>
            public string SpanishName { get; set; }

            /// <summary>
            /// Nombre en ingles
            /// </summary>
            public string EnglishName { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class UpdatePassportStateNameValidator : AbstractValidator<UpdatePassportStateNameRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public UpdatePassportStateNameValidator()
            {
                RuleFor(r => r.IdState).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
                RuleFor(r => r.SpanishName).NotEmpty().IsValidLatinString().WithFormatMessage(ValidatorsMessages.VALUE_WRONG_FORMAT, ValidatorFields.NombreES);
                RuleFor(r => r.EnglishName).NotEmpty().IsValidLatinString().WithFormatMessage(ValidatorsMessages.VALUE_WRONG_FORMAT, ValidatorFields.NombreEN);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class UpdatePassportStateNameCommandHandler : BaseCommandHandler<UpdatePassportStateNameRequest, bool>
        {
            private readonly IRepository<EstadoPasaporte> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public UpdatePassportStateNameCommandHandler(IRepository<EstadoPasaporte> repository)
            {
                this.repository = repository;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(UpdatePassportStateNameRequest request, CancellationToken cancellationToken)
            {
                EstadoPasaporte estado = await repository.GetAll()
                    .Include(c => c.EstadoPasaporteIdioma)
                    .FirstOrDefaultAsync(c => c.Id == request.IdState).ConfigureAwait(false);

                if (estado == null)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.PassportState)
                    });
                }

                foreach (var item in estado.EstadoPasaporteIdioma)
                {
                    if(item.Idioma == IdiomaTypes.es.ToString())
                    {
                        item.Nombre = request.SpanishName;
                    }
                    else if (item.Idioma == IdiomaTypes.en.ToString())
                    {
                        item.Nombre = request.EnglishName;
                    }
                }

                repository.Update(estado);

                await repository.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }
        }
    }
}
