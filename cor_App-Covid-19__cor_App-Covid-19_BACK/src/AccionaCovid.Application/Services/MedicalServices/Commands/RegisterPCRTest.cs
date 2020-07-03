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

namespace AccionaCovid.Application.Services.MedicalServices
{
    /// <summary>
    /// Servicio que registra un test de tipo PCR
    /// </summary>
    public class RegisterPCRTest
    {
        /// <summary>
        /// Request
        /// </summary>
        public class RegisterPCRTestRequest : IRequest<bool>
        {
            /// <summary>
            /// Identificador del empleado
            /// </summary>
            public int IdEmployee { get; set; }

            /// <summary>
            /// Valor del test
            /// </summary>
            public bool Positivo { get; set; }

            /// <summary>
            /// Fecha del test
            /// </summary>
            public DateTimeOffset? FechaTest { get; set; }
        }

        /// <summary>
        /// Clase de validacion
        /// </summary>
        public class RegisterPCRTestRequestValidator : AbstractValidator<RegisterPCRTestRequest>
        {
            public RegisterPCRTestRequestValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class RegisterPCRTestCommandHandler : BaseCommandHandler<RegisterPCRTestRequest, bool>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<ResultadoTestPcr> repository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public RegisterPCRTestCommandHandler(IRepository<ResultadoTestPcr> repository, IRepository<Empleado> repositoryEmpleado)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(RegisterPCRTestRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                ResultadoTestPcr test = new ResultadoTestPcr()
                {
                    FechaTest = request.FechaTest.HasValue ? request.FechaTest.Value : DateTime.UtcNow,
                    Positivo = request.Positivo,
                    IdFichaMedica = empleado.IdFichaMedica.Value
                };

                repository.Add(test);
                await repository.SaveChangesAsync().ConfigureAwait(false);

                return true;
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
