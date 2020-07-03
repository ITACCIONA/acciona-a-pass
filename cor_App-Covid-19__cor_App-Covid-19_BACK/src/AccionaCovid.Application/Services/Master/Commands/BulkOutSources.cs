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
    public class BulkOutSourcers
    {
        /// <summary>
        /// Petición del volcado de Subcontratas de las contratas
        /// </summary>
        public class BulkOutSourcersRequest : IRequest<bool>
        {
            /// <summary>
            /// Fichero CSV con los Subcontratas
            /// </summary>
            public IFormFile OutSourcersFile { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class BulkOutSourcersCommandHandler : BaseCommandHandler<BulkOutSourcersRequest, bool>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Subcontrata> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public BulkOutSourcersCommandHandler(IRepository<Subcontrata> repository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(BulkOutSourcersRequest request, CancellationToken cancellationToken)
            {
                (string[], string[][]) csv = await BulkUtils.ReadCsvAsync(request.OutSourcersFile, cancellationToken).ConfigureAwait(false);


                string[] headers = csv.Item1;
                string[][] data = csv.Item2;
                if (!data.Any())
                    return false;

                Subcontrata.SetPropertyIndexes(headers);

                List<Subcontrata> newSubcontratas = new List<Subcontrata>();

                HashSet<string> existentCifs = (await repository.GetAll().Select(u => u.Cif).ToListAsync().ConfigureAwait(false)).ToHashSet();

                foreach (string[] dataRow in data)
                {
                    Subcontrata newSubcontrata = new Subcontrata(dataRow);

                    if (existentCifs.Contains(newSubcontrata.Cif))
                        continue;

                    newSubcontrata.LastAction = "CREATE";
                    newSubcontrata.LastActionDate = DateTime.UtcNow;

                    newSubcontratas.Add(newSubcontrata);
                    existentCifs.Add(newSubcontrata.Cif);
                }

                await repository.BulkInsertAsync(newSubcontratas).ConfigureAwait(false); ;

                return true;
            }
        }
    }
}
