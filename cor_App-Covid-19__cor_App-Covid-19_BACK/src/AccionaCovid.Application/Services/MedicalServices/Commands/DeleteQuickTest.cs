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
    public class DeleteQuickTest
    {
        /// <summary>
        /// </summary>
        public class DeleteQuickTestRequest : IRequest<bool>
        {
            public int IdQuickTest { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class DeleteQuickTestValidator : AbstractValidator<DeleteQuickTestRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public DeleteQuickTestValidator()
            {
                RuleFor(r => r.IdQuickTest).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdTestRapido);
            }
        }


        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class DeleteQuickTestCommandHandler : BaseCommandHandler<DeleteQuickTestRequest, bool>
        {
            /// <summary>
            /// Repositorio
            /// </summary>
            private readonly IRepository<ResultadoTestMedico> repositoryResultadoTestMedico;

            /// <summary>
            /// Constructor
            /// </summary>
            public DeleteQuickTestCommandHandler(IRepository<ResultadoTestMedico> repositoryResultadoTestMedico)
            {
                this.repositoryResultadoTestMedico = repositoryResultadoTestMedico;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(DeleteQuickTestRequest request, CancellationToken cancellationToken)
            {
                var test = await this.repositoryResultadoTestMedico.GetBy(t => t.Id == request.IdQuickTest).FirstOrDefaultAsync().ConfigureAwait(false);

                if (test == null)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.PcrTest)
                    });
                }

                this.repositoryResultadoTestMedico.Remove(test);
                await this.repositoryResultadoTestMedico.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }
        }
    }
}
