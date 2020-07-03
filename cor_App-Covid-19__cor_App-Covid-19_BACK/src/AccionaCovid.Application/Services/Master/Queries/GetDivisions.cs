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
    /// Servicio que obtiene la informacion maestro de divisiones
    /// </summary>
    public class GetDivisions
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetDivisionsRequest : IRequest<List<GetDivisionsResponse>>
        {
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetDivisionsResponse
        {
            /// <summary>
            /// Identificador de la division
            /// </summary>
            public int IdDivision { get; set; }

            /// <summary>
            /// Nombre de la division
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public GetDivisionsResponse(string name, int id)
            {
                IdDivision = id;
                Name = name;
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetDivisionsCommandHandler : BaseCommandHandler<GetDivisionsRequest, List<GetDivisionsResponse>>
        {
            private readonly IRepository<Division> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetDivisionsCommandHandler(IRepository<Division> repository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<List<GetDivisionsResponse>> Handle(GetDivisionsRequest request, CancellationToken cancellationToken)
            {
                List<Division> dpts = await repository.GetAll().ToListAsync().ConfigureAwait(false);

                dpts = dpts.OrderBy(c => c.Nombre).ToList();

                var result = dpts.Select(dpt => new GetDivisionsResponse(dpt.Nombre, dpt.Id));

                return result.ToList();
            }
        }
    }
}
