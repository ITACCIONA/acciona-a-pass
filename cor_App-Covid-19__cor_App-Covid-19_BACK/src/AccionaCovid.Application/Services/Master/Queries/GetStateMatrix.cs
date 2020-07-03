using AccionaCovid.Application.Core;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static AccionaCovid.Domain.Model.Partials.Idioma;

namespace AccionaCovid.Application.Services.Master
{
    /// <summary>
    /// </summary>
    public class GetStateMatrix
    {
        /// <summary>
        /// </summary>
        public class GetStateMatrixRequest : IRequest<List<GetStateMatrixResponse>>
        {
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetStateMatrixResponse
        {
            /// <summary>
            /// Identificaodr del estado
            /// </summary>
            public int IdState { get; set; }

            /// <summary>
            /// Nombre del esatdo
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// Nombre del color
            /// </summary>
            public int IdColor { get; set; }

            /// <summary>
            /// Nombre del color
            /// </summary>
            public string Color { get; set; }

            /// <summary>
            /// Dias de validez
            /// </summary>
            public int? DiasValidez { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public bool? Pcrant { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public bool? Pcrult { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public bool? TestInmuneIgG { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public bool? TestInmuneIgM { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public bool? Comment { get; set; }

            /// <summary>
            /// Nombre en español
            /// </summary>
            public string SpanishName { get; set; }

            /// <summary>
            /// Nombre en ingles
            /// </summary>
            public string EnglishName { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public GetStateMatrixResponse(EstadoPasaporte ep)
            {
                IdState = ep.Id;
                Key = ep.Nombre;
                Color = ep.IdColorEstadoNavigation?.Nombre;
                IdColor = ep.IdColorEstado;
                SpanishName = ep.EstadoPasaporteIdioma.FirstOrDefault(c => c.Idioma == IdiomaTypes.es.ToString())?.Nombre ?? string.Empty;
                EnglishName = ep.EstadoPasaporteIdioma.FirstOrDefault(c => c.Idioma == IdiomaTypes.en.ToString())?.Nombre ?? string.Empty;
                DiasValidez = ep.DiasValidez;
                Pcrant = ep.Pcrant;
                Pcrult = ep.Pcrult;
                TestInmuneIgG = ep.TestInmuneIgG;
                TestInmuneIgM = ep.TestInmuneIgM;
                Comment = ep.Comment;
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetStateMatrixCommandHandler : BaseCommandHandler<GetStateMatrixRequest, List<GetStateMatrixResponse>>
        {
            private readonly IRepository<EstadoPasaporte> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetStateMatrixCommandHandler(IRepository<EstadoPasaporte> repository)
            {
                this.repository = repository;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<List<GetStateMatrixResponse>> Handle(GetStateMatrixRequest request, CancellationToken cancellationToken)
            {
                List<EstadoPasaporte> estados = await repository.GetAll()
                    .Include(c => c.IdColorEstadoNavigation)
                    .Include(c => c.EstadoPasaporteIdioma)
                    .ToListAsync().ConfigureAwait(false);

                var result = estados.Select(dpt => new GetStateMatrixResponse(dpt)).ToList();

                return result;
            }
        }
    }
}
