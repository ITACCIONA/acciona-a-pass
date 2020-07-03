using AccionaCovid.Application.Core;
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
    public class BulkWorks
    {
        /// <summary>
        /// Petición del volcado de las obras de las contratas
        /// </summary>
        public class BulkWorksRequest : IRequest<bool>
        {
            /// <summary>
            /// Fichero CSV con las Obras
            /// </summary>
            public IFormFile WorksFile { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class BulkWorksCommandHandler : BaseCommandHandler<BulkWorksRequest, bool>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Obra> repository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<EstadoObra> estadoObraRepository;


            /// <summary>
            /// Constructor
            /// </summary>
            public BulkWorksCommandHandler(IRepository<Obra> repository, IRepository<EstadoObra> estadoObraRepository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
                this.estadoObraRepository = estadoObraRepository ?? throw new ArgumentNullException(nameof(estadoObraRepository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(BulkWorksRequest request, CancellationToken cancellationToken)
            {
                (string[], string[][]) csv = await BulkUtils.ReadCsvAsync(request.WorksFile, cancellationToken).ConfigureAwait(false);


                string[] headers = csv.Item1;
                string[][] data = csv.Item2;
                if (!data.Any())
                    return false;

                Obra.SetPropertyIndexes(headers);

                var estadosObraDict = await estadoObraRepository.GetAll()
                   .ToDictionaryAsync(div => div.Nombre, div => div.Id)
                   .ConfigureAwait(false);

                List<Obra> newObras = new List<Obra>();

                HashSet<string> existentWorks = (await repository.GetAll().Select(u => u.CodigoObra).ToListAsync().ConfigureAwait(false)).ToHashSet();

                foreach (string[] dataRow in data)
                {
                    Obra newObra = new Obra(dataRow);

                    if (existentWorks.Contains(newObra.CodigoObra))
                        continue;

                    newObra.LastAction = "CREATE";
                    newObra.LastActionDate = DateTime.UtcNow;

                    if (!string.IsNullOrEmpty(newObra.EstadoObra.Nombre))
                    {
                        if (!estadosObraDict.ContainsKey(newObra.EstadoObra.Nombre))
                            throw new Exception($"No el estado de la obra {newObra.EstadoObra.Nombre} en el maestro de tecnologias");
                        newObra.IdEstadoObra = estadosObraDict[newObra.EstadoObra.Nombre];
                    }

                    newObras.Add(newObra);
                    existentWorks.Add(newObra.CodigoObra);
                }

                await repository.BulkInsertAsync(newObras).ConfigureAwait(false); ;

                return true;
            }
        }
    }
}
