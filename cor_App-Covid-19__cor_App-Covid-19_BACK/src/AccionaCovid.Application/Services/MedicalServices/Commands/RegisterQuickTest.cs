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
    /// Servicio que registra un test de tipo rapido
    /// </summary>
    public class RegisterQuickTest
    {
        /// <summary>
        /// Request
        /// </summary>
        public class RegisterQuickTestRequest : IRequest<bool>
        {
            /// <summary>
            /// Identificador del empleado
            /// </summary>
            public int IdEmployee { get; set; }

            /// <summary>
            /// Valor de control
            /// </summary>
            public bool Control { get; set; }

            /// <summary>
            /// Valor IGG
            /// </summary>
            public bool Igg { get; set; }

            /// <summary>
            /// Valor IGM
            /// </summary>
            public bool Igm { get; set; }

            /// <summary>
            /// Fecha del test
            /// </summary>
            public DateTimeOffset? FechaTest { get; set; }
        }

        /// <summary>
        /// Clase de validacion
        /// </summary>
        public class RegisterQuickTestRequestValidator : AbstractValidator<RegisterQuickTestRequest>
        {
            public RegisterQuickTestRequestValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class RegisterQuickTestCommandHandler : BaseCommandHandler<RegisterQuickTestRequest, bool>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<ResultadoTestMedico> repository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public RegisterQuickTestCommandHandler(IRepository<ResultadoTestMedico> repository, IRepository<Empleado> repositoryEmpleado)
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
            public override async Task<bool> Handle(RegisterQuickTestRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                ResultadoTestMedico test = new ResultadoTestMedico()
                {
                    FechaTest = request.FechaTest.HasValue ? request.FechaTest.Value : DateTime.UtcNow,
                    Control = request.Control,
                    Igg = request.Igg,
                    Igm = request.Igm,
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
