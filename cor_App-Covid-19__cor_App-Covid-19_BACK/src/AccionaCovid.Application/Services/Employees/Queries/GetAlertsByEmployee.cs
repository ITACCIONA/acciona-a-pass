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
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Employees
{
    /// <summary>
    /// Servicio que obtiene las alertas de un empleado (pendientes y no pendientes)
    /// </summary>
    public class GetAlertsByEmployee
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetAlertsByEmployeeRequest : IRequest<GetAlertsByEmployeeResponse>
        {
            /// <summary>
            /// Identificador del empleado
            /// </summary>
            public int IdEmployee { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class GetAlertsByEmployeeValidator : AbstractValidator<GetAlertsByEmployeeRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public GetAlertsByEmployeeValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetAlertsByEmployeeResponse
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="alertsNoRead"></param>
            /// <param name="alertsRead"></param>
            public GetAlertsByEmployeeResponse(List<AlertaServiciosMedicos> alertsNoRead, List<AlertaServiciosMedicos> alertsRead)
            {
                AlertsNoRead = new List<AlertEmployee>();
                AlertsRead = new List<AlertEmployee>();

                alertsNoRead = alertsNoRead.OrderByDescending(c => c.FechaNotificacion).ToList();
                alertsRead = alertsRead.OrderByDescending(c => c.FechaNotificacion).ToList();

                foreach (var item in alertsNoRead)
                {
                    AlertEmployee newAlert = new AlertEmployee()
                    {
                        FechaNotificacion = item.FechaNotificacion,
                        Comment = item.Comentario,
                        Title = item.Titulo,
                        IdAlert = item.Id
                    };

                    AlertsNoRead.Add(newAlert);
                }

                foreach (var item in alertsRead)
                {
                    AlertEmployee newAlert = new AlertEmployee()
                    {
                        FechaNotificacion = item.FechaNotificacion,
                        Comment = item.Comentario,
                        Title = item.Titulo,
                        IdAlert = item.Id
                    };

                    AlertsRead.Add(newAlert);
                }
            }

            /// <summary>
            /// Lista de alertas sin leer
            /// </summary>
            public List<AlertEmployee> AlertsNoRead { get; set; }

            /// <summary>
            /// Lista de alertas leidas
            /// </summary>
            public List<AlertEmployee> AlertsRead { get; set; }
        }

        /// <summary>
        /// Entidad que representa una alerta de un empelado
        /// </summary>
        public class AlertEmployee
        {
            /// <summary>
            /// Identificador de la alerta
            /// </summary>
            public int IdAlert { get; set; }

            /// <summary>
            /// Titulo
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// Comentario
            /// </summary>
            public string Comment { get; set; }

            /// <summary>
            /// Identificador del empleado
            /// </summary>
            public DateTimeOffset FechaNotificacion { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetAlertsByEmployeeCommandHandler : BaseCommandHandler<GetAlertsByEmployeeRequest, GetAlertsByEmployeeResponse>
        {
            /// <summary>
            /// Repositorio
            /// </summary>
            private readonly IRepository<AlertaServiciosMedicos> repository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Instancia del logger
            /// </summary>
            ILogger<GetAlertsByEmployeeCommandHandler> logger;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetAlertsByEmployeeCommandHandler(IRepository<AlertaServiciosMedicos> repository, IRepository<Empleado> repositoryEmpleado, ILogger<GetAlertsByEmployeeCommandHandler> logger)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado));
                this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async override Task<GetAlertsByEmployeeResponse> Handle(GetAlertsByEmployeeRequest request, CancellationToken cancellationToken)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                int limit = 20;

                await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                List<AlertaServiciosMedicos> listaNoLeidos = await repository.GetAll().Where(c => c.IdEmpleado == request.IdEmployee && !c.Leido.Value).OrderByDescending(c => c.FechaNotificacion).Take(limit).ToListAsync().ConfigureAwait(false);
                stopWatch.SendTimeOperation("GetAlertsByEmployee -> Alerts No Read", logger);

                List<AlertaServiciosMedicos> listaLeidos = await repository.GetAll().Where(c => c.IdEmpleado == request.IdEmployee && c.Leido.Value).OrderByDescending(c => c.FechaNotificacion).Take(limit).ToListAsync().ConfigureAwait(false);
                stopWatch.SendTimeOperation("GetAlertsByEmployee -> Alerts Read", logger);

                return new GetAlertsByEmployeeResponse(listaNoLeidos, listaLeidos);
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
            }
        }
    }
}
