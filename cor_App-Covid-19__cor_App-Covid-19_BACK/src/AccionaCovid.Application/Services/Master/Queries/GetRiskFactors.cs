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
    /// Servicio que obtiene la informacion maestro de factores de riesgo
    /// </summary>
    public class GetRiskFactors
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetRiskFactorsRequest : IRequest<List<GetRiskFactorsResponse>>
        {
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetRiskFactorsResponse
        {
            /// <summary>
            /// Identificador del factor de riesgo
            /// </summary>
            public int IdRiskFactor { get; set; }

            /// <summary>
            /// Nombre del factor de riesgo
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public GetRiskFactorsResponse(FactorRiesgo tipo)
            {
                IdRiskFactor = tipo.Id;
                Name = tipo.Nombre;
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetRiskFactorsCommandHandler : BaseCommandHandler<GetRiskFactorsRequest, List<GetRiskFactorsResponse>>
        {
            private readonly IRepository<FactorRiesgo> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetRiskFactorsCommandHandler(IRepository<FactorRiesgo> repository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<List<GetRiskFactorsResponse>> Handle(GetRiskFactorsRequest request, CancellationToken cancellationToken)
            {
                List<FactorRiesgo> estados = await repository.GetAll().ToListAsync().ConfigureAwait(false);

                var result = estados.Select(dpt => new GetRiskFactorsResponse(dpt)).ToList();

                return result;
            }
        }
    }
}
