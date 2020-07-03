using AccionaCovid.Application.Core;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Master
{
    /// <summary>
    /// Servicio que obtiene la informacion maestro de seguimientos
    /// </summary>
    public class GetMonitorings
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetMonitoringsRequest : IRequest<List<GetMonitoringsResponse>>
        {
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetMonitoringsResponse
        {
            /// <summary>
            /// Identificador del tipo de parametro
            /// </summary>
            public int IdParameterType { get; set; }

            /// <summary>
            /// Nombre del tipo de parametro
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Nombre del tipo de parametro
            /// </summary>
            public List<ParameterMonitoring> Parameters { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public GetMonitoringsResponse(TipoParametroMedico tipo)
            {
                IdParameterType = tipo.Id;
                Name = tipo.Nombre;
                Parameters = new List<ParameterMonitoring>();

                foreach (var param in tipo.ParametroMedico)
                {
                    ParameterMonitoring newParameterMonitoring = new ParameterMonitoring()
                    {
                        IdParameter = param.Id,
                        Name = param.Nombre
                    };

                    Parameters.Add(newParameterMonitoring);
                }
            }
        }

        /// <summary>
        /// Tipo de parametro
        /// </summary>
        public class ParameterMonitoring
        {
            /// <summary>
            /// Identificador del tipo de parametro
            /// </summary>
            public int IdParameter { get; set; }

            /// <summary>
            /// Nombre del parametro
            /// </summary>
            public string Name { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetMonitoringsCommandHandler : BaseCommandHandler<GetMonitoringsRequest, List<GetMonitoringsResponse>>
        {
            private readonly IRepository<TipoParametroMedico> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetMonitoringsCommandHandler(IRepository<TipoParametroMedico> repository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<List<GetMonitoringsResponse>> Handle(GetMonitoringsRequest request, CancellationToken cancellationToken)
            {
                List<TipoParametroMedico> estados = await repository.GetAll().Include(c => c.ParametroMedico).ToListAsync().ConfigureAwait(false);

                var result = estados.Select(dpt => new GetMonitoringsResponse(dpt)).ToList();

                return result;
            }
        }
    }
}
