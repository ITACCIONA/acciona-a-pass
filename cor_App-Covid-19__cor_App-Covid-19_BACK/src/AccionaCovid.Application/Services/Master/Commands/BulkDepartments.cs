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
    public class BulkDepartments
    {
        /// <summary>
        /// Petición del volcado de departamentos
        /// </summary>
        public class BulkDepartmentsRequest : IRequest<bool>
        {
            /// <summary>
            /// Fichero CSV con localizaciones
            /// </summary>
            public IFormFile DepartmentsFile { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class BulkDepartmentsCommandHandler : BaseCommandHandler<BulkDepartmentsRequest, bool>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Departamento> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public BulkDepartmentsCommandHandler(IRepository<Departamento> repository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(BulkDepartmentsRequest request, CancellationToken cancellationToken)
            {
                (string[], string[][]) csv = await BulkUtils.ReadCsvAsync(request.DepartmentsFile, cancellationToken).ConfigureAwait(false);


                string[] headers = csv.Item1;
                string[][] data = csv.Item2;
                if (!data.Any())
                    return false;

                Departamento.SetPropertyIndexes(headers);

                List<Departamento> newDepartamentos = new List<Departamento>();

                List<long> existentIdWorkday = await repository.GetAll().Select(u => u.IdWorkday).ToListAsync().ConfigureAwait(false);

                foreach (string[] dataRow in data)
                {
                    Departamento newDepartamento = new Departamento(dataRow);

                    if (existentIdWorkday.Contains(newDepartamento.IdWorkday))
                        continue;

                    newDepartamento.LastAction = "CREATE";
                    newDepartamento.LastActionDate = DateTime.UtcNow;

                    newDepartamentos.Add(newDepartamento);
                }

                await repository.BulkInsertAsync(newDepartamentos).ConfigureAwait(false); ;

                return true;
            }
        }
    }
}
