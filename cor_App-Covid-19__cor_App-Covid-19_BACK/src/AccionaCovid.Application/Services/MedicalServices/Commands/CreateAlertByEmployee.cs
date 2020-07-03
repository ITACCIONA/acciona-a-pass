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
    /// Servicio que registra una alerta a todos los empelados de una division
    /// </summary>
    public class CreateAlertByEmployee
    {
        /// <summary>
        /// Request
        /// </summary>
        public class CreateAlertByEmployeeRequest : IRequest<bool>
        {
            /// <summary>
            /// Empleado
            /// </summary>
            public int IdEmployee { get; set; }

            /// <summary>
            /// Fecha de la alerta
            /// </summary>
            public DateTimeOffset CurrentDeviceDateTime { get; set; }

            /// <summary>
            /// Titulo
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// Comentario
            /// </summary>
            public string Comment { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class GetPassportValidator : AbstractValidator<CreateAlertByEmployeeRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public GetPassportValidator()
            {
                RuleFor(v => v.Title).NotNull().NotEmpty().WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_NULL_OR_EMPTY, ValidatorFields.Title);
                RuleFor(v => v.Title).IsValidLatinString().WithFormatMessage(ValidatorsMessages.VALUE_WRONG_FORMAT, ValidatorFields.Title);

                RuleFor(v => v.Comment).NotNull().NotEmpty().WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_NULL_OR_EMPTY, ValidatorFields.Comment);
                RuleFor(v => v.Comment).IsValidLatinString().WithFormatMessage(ValidatorsMessages.VALUE_WRONG_FORMAT, ValidatorFields.Comment);

                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class CreateAlertByEmployeeCommandHandler : BaseCommandHandler<CreateAlertByEmployeeRequest, bool>
        {
            /// <summary>
            /// Repositorio
            /// </summary>
            private readonly IRepository<AlertaServiciosMedicos> repository;

            /// <summary>
            /// Repositorio
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public CreateAlertByEmployeeCommandHandler(IRepository<AlertaServiciosMedicos> repository, IRepository<Empleado> repositoryEmpleado)
            {
                this.repository = repository;
                this.repositoryEmpleado = repositoryEmpleado;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(CreateAlertByEmployeeRequest request, CancellationToken cancellationToken)
            {
                await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                AlertaServiciosMedicos newAlerta = new AlertaServiciosMedicos()
                {
                    Comentario = request.Comment,
                    Titulo = request.Title,
                    FechaNotificacion = request.CurrentDeviceDateTime,
                    IdEmpleado = request.IdEmployee,
                    Leido = false
                };

                repository.Add(newAlerta);

                await repository.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }

            /// <summary>
            /// Metodo que valida si existe el empleado y su ficha
            /// </summary>
            /// <param name="idEmpleado"></param>
            /// <returns></returns>
            private async Task ValidacionEmpleado(int idEmpleado)
            {
                Empleado empleado = await repositoryEmpleado
                    .GetBy(p => p.Id == idEmpleado)
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
            }
        }
    }
}
