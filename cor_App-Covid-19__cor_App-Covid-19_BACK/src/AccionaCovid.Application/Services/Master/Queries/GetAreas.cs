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
    /// Servicio que obtiene la informacion maestro de areas
    /// </summary>
    public class GetAreas
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetAreasRequest : IRequest<List<GetAreasResponse>>
        {
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetAreasResponse
        {
            /// <summary>
            /// Identificador del area
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Nombre del area
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Identificador de la region
            /// </summary>
            public int IdRegion { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetAreasByRegionCommandHandler : BaseCommandHandler<GetAreasRequest, List<GetAreasResponse>>
        {
            private readonly IRepository<Area> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetAreasByRegionCommandHandler(IRepository<Area> repository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<List<GetAreasResponse>> Handle(GetAreasRequest request, CancellationToken cancellationToken)
            {
                var areas = await repository.GetAll()
                    .Select(a => new { a.Id, a.Nombre, a.IdRegion})
                    .Distinct()
                    .ToListAsync().ConfigureAwait(false);

                var result = areas.Select(c => new GetAreasResponse()
                {
                    Id = c.Id,
                    Name = c.Nombre,
                    IdRegion = c.IdRegion
                }).ToList();

                SendTimeOperationToLogger("GetAreas");

                return result;
            }
        }
    }
}
