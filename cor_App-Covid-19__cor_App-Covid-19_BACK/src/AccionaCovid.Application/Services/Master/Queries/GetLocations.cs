using AccionaCovid.Application.Core;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Master
{
    /// <summary>
    /// Servicio que obtiene la informacion maestro de localizaciones
    /// </summary>
    public class GetLocations
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetLocationsRequest : IRequest<List<GetLocationsResponse>>
        {
            /// <summary>
            /// Nombre del país
            /// </summary>
            public string CountryName { get; set; }

            /// <summary>
            /// Identificador de la region
            /// </summary>
            public int? IdRegion { get; set; }

            /// <summary>
            /// Identificador del área
            /// </summary>
            public int? IdArea { get; set; }
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetLocationsResponse
        {
            /// <summary>
            /// Identificador de la localizacion
            /// </summary>
            public int IdLocation { get; set; }

            /// <summary>
            /// Nombre de la localizacion
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Ciudad de la localizacion
            /// </summary>
            public string Ciudad { get; set; }

            /// <summary>
            /// CodPostasl de la localizacion
            /// </summary>
            public string CodPostasl { get; set; }

            /// <summary>
            /// Direccion de la localizacion
            /// </summary>
            public string Direccion { get; set; }

            /// <summary>
            /// Pais de la localizacion
            /// </summary>
            public string Pais { get; set; }

            /// <summary>
            /// Identificador del area
            /// </summary>
            public int? IdArea { get; set; }

            /// <summary>
            /// Identificador del area
            /// </summary>
            public int? IdRegion { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetLocationsCommandHandler : BaseCommandHandler<GetLocationsRequest, List<GetLocationsResponse>>
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
            public override async Task<List<GetLocationsResponse>> Handle(GetLocationsRequest request, CancellationToken cancellationToken)
            {
                Expression<Func<Localizacion, bool>> query = l => true;

                if (!string.IsNullOrWhiteSpace(request.CountryName))
                {
                    query = query.And(l => l.Pais.Trim().ToUpper() == request.CountryName);
                }
                if (request.IdRegion.HasValue)
                {
                    query = query.And(l => l.Area.IdRegion == request.IdRegion.Value);
                }
                if (request.IdArea.HasValue)
                {
                    query = query.And(l => l.IdArea == request.IdArea.Value);
                }

                List<Localizacion> dpts = await repository.GetAll()
                    .Include(l => l.Area)
                    .Where(query)
                    .OrderBy(c => c.Nombre)
                    .ToListAsync().ConfigureAwait(false);

                var result = dpts.Select(dpt => new GetLocationsResponse()
                {
                    IdLocation = dpt.Id,
                    Ciudad = dpt.Ciudad,
                    CodPostasl = dpt.CodigoPostal,
                    Direccion = dpt.Direccion1,
                    Name = dpt.Nombre,
                    Pais = dpt.Pais,
                    IdArea = dpt.IdArea,
                    IdRegion = dpt.Area?.IdRegion
                }).ToList();

                SendTimeOperationToLogger("GetLocations");

                return result;
            }
        }
    }
}
