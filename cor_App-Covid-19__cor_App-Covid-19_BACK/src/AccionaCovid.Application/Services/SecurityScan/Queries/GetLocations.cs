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

namespace AccionaCovid.Application.Services.SecurityScan
{
    /// <summary>
    /// Servicio que obtiene la informacion maestro de localizaciones
    /// </summary>
    public class GetLocations
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetLocationsRequest : IRequest<List<GetLocationsSecurityResponse>>
        {
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetLocationsSecurityResponse
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
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetLocationsCommandHandler : BaseCommandHandler<GetLocationsRequest, List<GetLocationsSecurityResponse>>
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
            public override async Task<List<GetLocationsSecurityResponse>> Handle(GetLocationsRequest request, CancellationToken cancellationToken)
            {
                List<Localizacion> dpts = await repository.GetAll().ToListAsync().ConfigureAwait(false);

                dpts = dpts.OrderBy(c => c.Nombre).ToList();

                var result = dpts.Select(dpt => new GetLocationsSecurityResponse()
                {
                    IdLocation = dpt.Id,
                    Ciudad = dpt.Ciudad,
                    CodPostasl = dpt.CodigoPostal,
                    Direccion = dpt.Direccion1,
                    Name = dpt.Nombre,
                    Pais = dpt.Pais
                }).ToList();

                SendTimeOperationToLogger("GetLocations");

                return result;
            }
        }
    }
}
