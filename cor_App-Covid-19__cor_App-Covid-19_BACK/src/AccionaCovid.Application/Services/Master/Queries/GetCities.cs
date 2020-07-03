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
    /// Servicio que obtiene la informacion maestro de ciudades
    /// </summary>
    public class GetCities
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetCitiesRequest : IRequest<List<GetCitiesResponse>>
        {
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetCitiesResponse
        {
            /// <summary>
            /// Pais de la localizacion
            /// </summary>
            public string Pais { get; set; }

            /// <summary>
            /// Ciudad de la localizacion
            /// </summary>
            public string Ciudad { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetLocationsCommandHandler : BaseCommandHandler<GetCitiesRequest, List<GetCitiesResponse>>
        {
            private readonly IRepository<Localizacion> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetLocationsCommandHandler(IRepository<Localizacion> repository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<List<GetCitiesResponse>> Handle(GetCitiesRequest request, CancellationToken cancellationToken)
            {
                var ciudades = await repository.GetAll()
                    .Select(l => new { l.Pais, l.Ciudad})
                    .Distinct()
                    .ToListAsync().ConfigureAwait(false);

                var result = ciudades.Select(c => new GetCitiesResponse()
                {
                    Pais = c.Pais,
                    Ciudad = c.Ciudad,

                }).ToList();

                SendTimeOperationToLogger("GetCities");

                return result;
            }
        }
    }
}
