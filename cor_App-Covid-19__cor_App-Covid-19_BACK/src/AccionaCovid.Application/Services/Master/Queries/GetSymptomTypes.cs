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
    /// Servicio que obtiene la informacion maestro de tipos de sintomas
    /// </summary>
    public class GetSymptomTypes
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetSymptomTypesRequest : IRequest<List<GetSymptomTypesResponse>>
        {
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetSymptomTypesResponse
        {
            /// <summary>
            /// Identificador del tipo de sintoma
            /// </summary>
            public int IdSymptomTypes { get; set; }

            /// <summary>
            /// Nombre del tipo de sintoma
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public GetSymptomTypesResponse(TipoSintomas tipo)
            {
                IdSymptomTypes = tipo.Id;
                Name = tipo.Nombre;
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetSymptomTypesCommandHandler : BaseCommandHandler<GetSymptomTypesRequest, List<GetSymptomTypesResponse>>
        {
            /// <summary>
            /// Repositorio
            /// </summary>
            private readonly IRepository<TipoSintomas> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetSymptomTypesCommandHandler(IRepository<TipoSintomas> repository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<List<GetSymptomTypesResponse>> Handle(GetSymptomTypesRequest request, CancellationToken cancellationToken)
            {
                List<TipoSintomas> estados = await repository.GetAll().ToListAsync().ConfigureAwait(false);

                var result = estados.Select(dpt => new GetSymptomTypesResponse(dpt)).ToList();

                return result;
            }
        }
    }
}
