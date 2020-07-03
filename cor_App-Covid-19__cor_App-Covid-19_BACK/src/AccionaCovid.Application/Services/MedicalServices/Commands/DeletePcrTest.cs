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

namespace AccionaCovid.Application.Services.MedicalServices
{
    /// <summary>
    /// </summary>
    public class DeletePcrTest
    {
        /// <summary>
        /// </summary>
        public class DeletePcrTestRequest : IRequest<bool>
        {
            public int IdPcrTest { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class DeletePcrTestValidator : AbstractValidator<DeletePcrTestRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public DeletePcrTestValidator()
            {
                RuleFor(r => r.IdPcrTest).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdTestPCR);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class DeletePcrTestCommandHandler : BaseCommandHandler<DeletePcrTestRequest, bool>
        {
            /// <summary>
            /// Repositorio
            /// </summary>
            private readonly IRepository<ResultadoTestPcr> repositoryResultadoTestPcr;

            /// <summary>
            /// Constructor
            /// </summary>
            public DeletePcrTestCommandHandler(IRepository<ResultadoTestPcr> repositoryResultadoTestPcr)
            {
                this.repositoryResultadoTestPcr = repositoryResultadoTestPcr;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(DeletePcrTestRequest request, CancellationToken cancellationToken)
            {
                var test = await this.repositoryResultadoTestPcr.GetBy(t => t.Id == request.IdPcrTest).FirstOrDefaultAsync().ConfigureAwait(false);

                if (test == null)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.PcrTest)
                    });
                }

                this.repositoryResultadoTestPcr.Remove(test);
                await this.repositoryResultadoTestPcr.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }
        }
    }
}
