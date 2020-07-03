using AccionaCovid.Application.Core;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Master
{
    /// <summary>
    /// Servicio de volcado de departamentos
    /// </summary>
    public class BulkWorkAward
    {
        /// <summary>
        /// Petición del volcado de las obras de las contratas
        /// </summary>
        public class BulkWorkAwardRequest : IRequest<BulkWorkAwardResult>
        {
            /// <summary>
            /// Fichero CSV con las Obras
            /// </summary>
            public IFormFile WorkAwardFile { get; set; }
        }

        public class BulkWorkAwardResult
        {
            public List<string> Errores { get; set; } = new List<string>();
        }
        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class BulkWorkAwardCommandHandler : BaseCommandHandler<BulkWorkAwardRequest, BulkWorkAwardResult>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<AdjudicacionTrabajoObra> repository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Obra> obraRepository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Subcontrata> subcontrataRepository;


            /// <summary>
            /// Constructor
            /// </summary>
            public BulkWorkAwardCommandHandler(IRepository<AdjudicacionTrabajoObra> repository, IRepository<Obra> obraRepository, IRepository<Subcontrata> subcontrataRepository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
                this.obraRepository = obraRepository ?? throw new ArgumentNullException(nameof(obraRepository));
                this.subcontrataRepository = subcontrataRepository ?? throw new ArgumentNullException(nameof(subcontrataRepository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<BulkWorkAwardResult> Handle(BulkWorkAwardRequest request, CancellationToken cancellationToken)
            {
                List<Exception> errores = new List<Exception>();

                (string[], string[][]) csv = await BulkUtils.ReadCsvAsync(request.WorkAwardFile, cancellationToken).ConfigureAwait(false);

                string[] headers = csv.Item1;
                string[][] data = csv.Item2;
                if (!data.Any())
                    return new BulkWorkAwardResult();

                AdjudicacionTrabajoObra.SetPropertyIndexes(headers);


                List<AdjudicacionTrabajoObra> newAdjudicacionObras = new List<AdjudicacionTrabajoObra>();

                Dictionary<string, int> obrasDict = await obraRepository.GetAll()
                    .ToDictionaryAsync(div => div.CodigoObra, div => div.Id)
                    .ConfigureAwait(false);

                Dictionary<string, int> subContratasDict = await subcontrataRepository.GetAll()
                    .ToDictionaryAsync(div => div.Cif, div => div.Id)
                    .ConfigureAwait(false);

                HashSet<string> existentAwardWorks = (await repository.GetAll()
                    .Select(adj => $"{adj.IdObra}__{adj.IdSubcontrata}")
                    .ToListAsync().ConfigureAwait(false)).ToHashSet();


                int intOrder = -50000000;
                foreach (string[] dataRow in data)
                {
                    try
                    {
                        AdjudicacionTrabajoObra newAdjudicacionObra = new AdjudicacionTrabajoObra(dataRow);

                        if (!obrasDict.ContainsKey(newAdjudicacionObra.Obra.CodigoObra))
                        {
                            errores.Add(new Exception($"Fila {intOrder + 50000002}: La obra: {newAdjudicacionObra.Obra.CodigoObra} no existe en la BBDD"));
                            continue;
                        }
                        newAdjudicacionObra.IdObra = obrasDict[newAdjudicacionObra.Obra.CodigoObra];
                        newAdjudicacionObra.Obra = null;


                        if (!subContratasDict.ContainsKey(newAdjudicacionObra.Subcontrata.Cif))
                        {
                            errores.Add(new Exception($"Fila {intOrder + 50000002}: La subcontrata: {newAdjudicacionObra.Subcontrata.Cif} no existe en la BBDD"));
                            continue;
                        }
                        newAdjudicacionObra.IdSubcontrata = subContratasDict[newAdjudicacionObra.Subcontrata.Cif];
                        newAdjudicacionObra.Subcontrata = null;

                        //Si la adjudicación ya existía no se vuelve a insertar
                        if (existentAwardWorks.Contains($"{newAdjudicacionObra.IdObra}__{newAdjudicacionObra.IdSubcontrata}"))
                            continue;

                        newAdjudicacionObra.LastAction = "CREATE";
                        newAdjudicacionObra.LastActionDate = DateTime.UtcNow;

                        newAdjudicacionObras.Add(newAdjudicacionObra);
                        existentAwardWorks.Add($"{newAdjudicacionObra.IdObra}__{newAdjudicacionObra.IdSubcontrata}");
                    }
                    finally
                    {
                        intOrder++;
                    }
                }

                await repository.BulkInsertAsync(newAdjudicacionObras).ConfigureAwait(false); ;

                return new BulkWorkAwardResult
                {
                    Errores = errores.Select(ex => ex.Message).ToList()
                };
            }
        }
    }
}
