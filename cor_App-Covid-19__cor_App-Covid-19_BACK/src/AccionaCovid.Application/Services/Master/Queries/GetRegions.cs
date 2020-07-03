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
    /// Servicio que obtiene la informacion maestro de regiones
    /// </summary>
    public class GetRegions
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetRegionsRequest : IRequest<List<GetRegionsResponse>>
        {
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetRegionsResponse
        {
            /// <summary>
            /// Identificador de la region
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Nombre de la region
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Identificador del pais
            /// </summary>
            public int IdCountry { get; set; }

            /// <summary>
            /// Nombre del pais
            /// </summary>
            public string Country { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetRegionsByCountryCommandHandler : BaseCommandHandler<GetRegionsRequest, List<GetRegionsResponse>>
        {
            private readonly IRepository<Region> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetRegionsByCountryCommandHandler(IRepository<Region> repository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<List<GetRegionsResponse>> Handle(GetRegionsRequest request, CancellationToken cancellationToken)
            {
                var regions = await repository.GetAll()
                        .Include(r => r.Pais)
                    .Select(r => new { r.Id, r.Nombre, Pais = new { r.Pais.Id, r.Pais.Nombre } })
                    .Distinct()
                    .ToListAsync().ConfigureAwait(false);

                var result = regions.Select(c => new GetRegionsResponse()
                {
                    Id = c.Id,
                    Name = c.Nombre,
                    IdCountry = c.Pais.Id,
                    Country = c.Pais.Nombre

                }).ToList();

                SendTimeOperationToLogger("GetRegions");

                return result;
            }
        }
    }
}
