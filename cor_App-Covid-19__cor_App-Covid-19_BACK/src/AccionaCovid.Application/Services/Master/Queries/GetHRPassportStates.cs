using AccionaCovid.Application.Core;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Master
{
    /// <summary>
    /// </summary>
    public class GetHRPassportStates
    {
        /// <summary>
        /// </summary>
        public class GetHRPassportStatesRequest : IRequest<List<GetHRPassportStatesResponse>>
        {
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetHRPassportStatesResponse
        {
            /// <summary>
            /// Identificaodr del estado
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Nombre del esatdo
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Nombre del color
            /// </summary>
            public string Color { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public GetHRPassportStatesResponse(EstadoPasaporte ep, string idioma)
            {
                Id = ep.Id;
                Name = ep.EstadoPasaporteIdioma.FirstOrDefault(c => c.Idioma == idioma)?.Nombre ?? ep.Nombre;
                Color = ep.IdColorEstadoNavigation?.Nombre;
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetPassportStatesCommandHandler : BaseCommandHandler<GetHRPassportStatesRequest, List<GetHRPassportStatesResponse>>
        {
            private readonly IRepository<EstadoPasaporte> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetPassportStatesCommandHandler(IRepository<EstadoPasaporte> repository)
            {
                this.repository = repository;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<List<GetHRPassportStatesResponse>> Handle(GetHRPassportStatesRequest request, CancellationToken cancellationToken)
            {
                List<EstadoPasaporte> estados = await repository.GetAll()
                    .Include(c => c.IdColorEstadoNavigation)
                    .Include(c => c.EstadoPasaporteIdioma)
                    .Where(c => c.EstadoId == (int)EstadoPasaporte.AllowedRRHHPassportStatesId.AsintomaticoPcrReconvertidoIGG ||
                                c.EstadoId == (int)EstadoPasaporte.AllowedRRHHPassportStatesId.SintomaticoPcrReconvertidoIGG)
                    .ToListAsync().ConfigureAwait(false);

                var result = estados.Select(dpt => new GetHRPassportStatesResponse(dpt, Idioma)).ToList();

                return result;
            }
        }
    }
}
