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
    public class GetPassportStateColors
    {
        /// <summary>
        /// </summary>
        public class GetPassportStateColorsRequest : IRequest<List<GetPassportStateColorsResponse>>
        {
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetPassportStateColorsResponse
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
            /// Constructor
            /// </summary>
            public GetPassportStateColorsResponse(ColorEstado ep)
            {
                Id = ep.Id;
                Name = ep.Nombre;
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetPassportStateColorsCommandHandler : BaseCommandHandler<GetPassportStateColorsRequest, List<GetPassportStateColorsResponse>>
        {
            private readonly IRepository<ColorEstado> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetPassportStateColorsCommandHandler(IRepository<ColorEstado> repository)
            {
                this.repository = repository;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<List<GetPassportStateColorsResponse>> Handle(GetPassportStateColorsRequest request, CancellationToken cancellationToken)
            {
                List<ColorEstado> estados = await repository.GetAll().ToListAsync().ConfigureAwait(false);

                var result = estados.Select(dpt => new GetPassportStateColorsResponse(dpt)).ToList();

                return result;
            }
        }
    }
}
