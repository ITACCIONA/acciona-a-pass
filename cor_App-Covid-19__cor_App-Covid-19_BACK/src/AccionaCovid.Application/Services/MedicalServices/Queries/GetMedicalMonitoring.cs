using AccionaCovid.Application.Core;
using AccionaCovid.Application.Resources;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace AccionaCovid.Application.Services.MedicalServices
{
    /// <summary>
    /// Servicio que obtiene la informacion de seguimientos medicos
    /// </summary>
    public class GetMedicalMonitoring
    {
        /// <summary>
        /// </summary>
        public class GetMedicalMonitoringRequest : IRequest<GetMedicalMonitoringResponse>
        {
            public GetMedicalMonitoringRequest(int idEmpleado)
            {
                IdEmpleado = idEmpleado;
            }

            /// <summary>
            /// Nombre del empleado
            /// </summary>
            public int IdEmpleado { get; set; }
        }

        /// <summary>
        /// Validador del request
        /// </summary>
        public class GetMedicalMonitoringRequestValidator : AbstractValidator<GetMedicalMonitoringRequest>
        {
            /// <summary>
            /// Validaciones a realizar
            /// </summary>
            public GetMedicalMonitoringRequestValidator()
            {
                RuleFor(v => v.IdEmpleado).Must(p => p > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class GetMedicalMonitoringResponse
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="testRapidos"></param>
            /// <param name="testPcr"></param>
            public GetMedicalMonitoringResponse(List<SeguimientoMedico> seguimientos)
            {
                MedicalMonitoring = new List<MedicalMonitoring>();

                if (seguimientos != null && seguimientos.Any())
                {
                    seguimientos = seguimientos.OrderByDescending(c => c.FechaSeguimiento).ToList();

                    var tipos = seguimientos.SelectMany(c => c.ValoracionParametroMedico.Select(d => d.IdParametroMedicoNavigation.IdTipoParametroNavigation)).Distinct();

                    foreach (var tipo in tipos)
                    {
                        MedicalMonitoring newMonotoring = new MedicalMonitoring()
                        {
                            IdParameterType = tipo.Id,
                            NameParameterType = tipo.Nombre
                        };

                        MedicalMonitoring.Add(newMonotoring);

                        var newSeguimiento = tipo.ParametroMedico.SelectMany(c => c.ValoracionParametroMedico.Select(d => d.IdSegumientoMedicoNavigation)).Distinct();

                        foreach (var seguiTipo in newSeguimiento)
                        {
                            MonitoringValue newMonitoringValue = new MonitoringValue()
                            {
                                Id = seguiTipo.Id,
                                Comment = seguiTipo.Comentarios,
                                FechaTest = seguiTipo.FechaSeguimiento
                            };

                            newMonotoring.MonitoringValue.Add(newMonitoringValue);

                            foreach (var valoracion in seguiTipo.ValoracionParametroMedico)
                            {
                                ParameterValueMonitoring newParameterValueMonitoring = new ParameterValueMonitoring()
                                {
                                    Value = valoracion.Valor,
                                    IdParameter = valoracion.IdParametroMedico,
                                    NameParameter = valoracion.IdParametroMedicoNavigation.Nombre
                                };

                                newMonitoringValue.ParameterValues.Add(newParameterValueMonitoring);
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Listado de test rapidos
            /// </summary>
            public List<MedicalMonitoring> MedicalMonitoring { get; set; }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class MedicalMonitoring
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public MedicalMonitoring()
            {
                MonitoringValue = new List<MonitoringValue>();
            }

            /// <summary>
            /// Id del tipo de parametros
            /// </summary>
            public int IdParameterType { get; set; }

            /// <summary>
            /// Nombre del tipo de parametros
            /// </summary>
            public string NameParameterType { get; set; }

            /// <summary>
            /// Listado de parametros para el tipo
            /// </summary>
            public List<MonitoringValue> MonitoringValue { get; set; }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class MonitoringValue
        {
            public int Id { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public MonitoringValue()
            {
                ParameterValues = new List<ParameterValueMonitoring>();
            }

            /// <summary>
            /// Fecha del seguimiento
            /// </summary>
            public DateTimeOffset FechaTest { get; set; }

            /// <summary>
            /// Comentario del seguimiento
            /// </summary>
            public string Comment { get; set; }

            /// <summary>
            /// Listado de parametros para el tipo
            /// </summary>
            public List<ParameterValueMonitoring> ParameterValues { get; set; }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class ParameterValueMonitoring
        {
            /// <summary>
            /// Id del parametro
            /// </summary>
            public int IdParameter { get; set; }

            /// <summary>
            /// Nombre del parametro
            /// </summary>
            public string NameParameter { get; set; }

            /// <summary>
            /// Valor
            /// </summary>
            public bool Value { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetMedicalMonitoringCommandHandler : BaseCommandHandler<GetMedicalMonitoringRequest, GetMedicalMonitoringResponse>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<SeguimientoMedico> repositorySeguimientoMedico;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetMedicalMonitoringCommandHandler(IRepository<SeguimientoMedico> repositorySeguimientoMedico, IRepository<Empleado> repositoryEmpleado)
            {
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado));
                this.repositorySeguimientoMedico = repositorySeguimientoMedico ?? throw new ArgumentNullException(nameof(repositorySeguimientoMedico));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<GetMedicalMonitoringResponse> Handle(GetMedicalMonitoringRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmpleado).ConfigureAwait(false);

                var listaTestRap = await repositorySeguimientoMedico.GetAll()
                    .Include(c => c.ValoracionParametroMedico)
                        .ThenInclude(c => c.IdParametroMedicoNavigation)
                        .ThenInclude(c => c.IdTipoParametroNavigation)
                    .Where(c => c.IdFichaMedica == empleado.IdFichaMedica
                        && c.ValoracionParametroMedico.Select(d => d.IdParametroMedicoNavigation).Any(e => e.Nombre != ParametroMedico.ParameterTypes.TemperaturaAlta.ToString()))
                    .ToListAsync().ConfigureAwait(false);

                return new GetMedicalMonitoringResponse(listaTestRap);
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
