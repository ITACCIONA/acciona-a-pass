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
    public class DeleteMedicalMonitoring
    {
        /// <summary>
        /// </summary>
        public class DeleteMedicalMonitoringRequest : IRequest<bool>
        {
            public int IdMedicalMonitoring { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class DeleteMedicalMonitoringValidator : AbstractValidator<DeleteMedicalMonitoringRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public DeleteMedicalMonitoringValidator()
            {
                RuleFor(r => r.IdMedicalMonitoring).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdMonitorizacionMedica);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class DeleteMedicalMonitoringCommandHandler : BaseCommandHandler<DeleteMedicalMonitoringRequest, bool>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<SeguimientoMedico> repositorySeguimientoMedico;

            /// <summary>
            /// Constructor
            /// </summary>
            public DeleteMedicalMonitoringCommandHandler(IRepository<SeguimientoMedico> repositorySeguimientoMedico)
            {
                this.repositorySeguimientoMedico = repositorySeguimientoMedico;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(DeleteMedicalMonitoringRequest request, CancellationToken cancellationToken)
            {
                var test = await this.repositorySeguimientoMedico.GetBy(t => t.Id == request.IdMedicalMonitoring).FirstOrDefaultAsync().ConfigureAwait(false);

                if (test == null)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.PcrTest)
                    });
                }

                this.repositorySeguimientoMedico.Remove(test);
                await this.repositorySeguimientoMedico.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
        }
    }
}
