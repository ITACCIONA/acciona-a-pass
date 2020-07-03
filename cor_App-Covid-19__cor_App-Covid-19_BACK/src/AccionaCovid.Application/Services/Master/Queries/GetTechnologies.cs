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
    /// Servicio que obtiene la informacion maestro de tecnologías
    /// </summary>
    public class GetTechnologies
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetTechnologiesRequest : IRequest<List<GetTechnologiesResponse>>
        {
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetTechnologiesResponse
        {
            /// <summary>
            /// Identificador de la tecnología
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Nombre de la tecnología
            /// </summary>
            public string Name { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetTechnologiesCommandHandler : BaseCommandHandler<GetTechnologiesRequest, List<GetTechnologiesResponse>>
        {
            private readonly IRepository<Tecnologia> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetTechnologiesCommandHandler(IRepository<Tecnologia> repository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<List<GetTechnologiesResponse>> Handle(GetTechnologiesRequest request, CancellationToken cancellationToken)
            {
                var dpts = await repository
                    .GetAll()
                    .Select(t => new { t.Id, t.Nombre })
                    .OrderBy(t => t.Nombre)
                    .ToListAsync().ConfigureAwait(false);

                var result = dpts.Select(dpt => new GetTechnologiesResponse
                {
                    Id = dpt.Id,
                    Name = dpt.Nombre
                }).ToList();

                SendTimeOperationToLogger("GetTechnologies");

                return result;
            }
        }
    }
}
