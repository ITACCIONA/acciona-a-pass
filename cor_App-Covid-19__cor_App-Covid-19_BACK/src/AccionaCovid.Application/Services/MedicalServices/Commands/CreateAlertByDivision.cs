using AccionaCovid.Application.Core;
using AccionaCovid.Application.Resources;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.MedicalServices
{
    /// <summary>
    /// Servicio que registra una alerta a todos los empelados de una division
    /// </summary>
    public class CreateAlertByDivision
    {
        /// <summary>
        /// Request
        /// </summary>
        public class CreateAlertByDivisionRequest : IRequest<bool>
        {
            /// <summary>
            /// Titulo
            /// </summary>
            public int IdDivision { get; set; }

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

            /// <summary>
            /// InterAcciona
            /// </summary>
            public bool? InterAcciona { get; set; }

            /// <summary>
            /// Estado del pasaporte del empelado
            /// </summary>
            public int? IdEstado { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class GetPassportValidator : AbstractValidator<CreateAlertByDivisionRequest>
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

                RuleFor(r => r.IdDivision).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.Division);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class CreateAlertByDivisionCommandHandler : BaseCommandHandler<CreateAlertByDivisionRequest, bool>
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
            /// Instancia del logger
            /// </summary>
            ILogger<CreateAlertByDivisionCommandHandler> logger;

            /// <summary>
            /// Constructor
            /// </summary>
            public CreateAlertByDivisionCommandHandler(IRepository<AlertaServiciosMedicos> repository, IRepository<Empleado> repositoryEmpleado, ILogger<CreateAlertByDivisionCommandHandler> logger)
            {
                this.repository = repository;
                this.repositoryEmpleado = repositoryEmpleado;
                this.logger = logger;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(CreateAlertByDivisionRequest request, CancellationToken cancellationToken)
            {
                var query = repositoryEmpleado.GetAll().Where(c => c.IdFichaLaboralNavigation.IdDivision == request.IdDivision);

                if (request.InterAcciona.HasValue && request.InterAcciona.Value)
                {
                    query = query.Where(c => c.InterAcciona.Value);
                }

                if (request.IdEstado.HasValue && request.IdEstado.Value > 0)
                {
                    query = query.Where(c => c.Pasaporte.Any(d => d.Activo.Value && d.IdEstadoPasaporte == request.IdEstado.Value));
                }

                // obtenemos todos los identificadores de todos empleados.
                List<int> idEmpl = await query.Select(c => c.Id).ToListAsync().ConfigureAwait(false);

                SendTimeOperationToLogger("GetIdEmployees");

                List<AlertaServiciosMedicos> listaAlerts = new List<AlertaServiciosMedicos>();

                foreach (var id in idEmpl)
                {
                    AlertaServiciosMedicos newAlerta = new AlertaServiciosMedicos()
                    {
                        Comentario = request.Comment,
                        Titulo = request.Title,
                        FechaNotificacion = request.CurrentDeviceDateTime,
                        IdEmpleado = id,
                        LastAction = "CREATE",
                        LastActionDate = DateTime.UtcNow,
                        Leido = false
                    };

                    listaAlerts.Add(newAlerta);
                }

                await repository.BulkInsertAsync(listaAlerts).ConfigureAwait(false);

                SendTimeOperationToLogger("Bulk");

                return true;
            }
        }
    }
}
