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
    /// Servicio que obtiene la informacion maestro de paises
    /// </summary>
    public class GetCountries
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetCountriesRequest : IRequest<List<GetCountriesResponse>>
        {
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetCountriesResponse
        {
            /// <summary>
            /// Nombre del pais
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public GetCountriesResponse(string country)
            {
                Name = country;
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetCountriesCommandHandler : BaseCommandHandler<GetCountriesRequest, List<GetCountriesResponse>>
        {
            private readonly IRepository<Localizacion> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetCountriesCommandHandler(IRepository<Localizacion> repository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<List<GetCountriesResponse>> Handle(GetCountriesRequest request, CancellationToken cancellationToken)
            {
                List<string> countries = await repository.GetAll().Select(c => c.Pais).Distinct().ToListAsync().ConfigureAwait(false);

                countries = countries.OrderBy(c => c).ToList();

                var result = countries.Select(pais => new GetCountriesResponse(pais));

                return result.ToList();
            }
        }
    }
}
