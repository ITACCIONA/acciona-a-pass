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
    /// Servicio que obtiene la informacion maestro de departamentos
    /// </summary>
    public class GetDepartments
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetDepartmentsRequest : IRequest<List<GetDepartmentsResponse>>
        {
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetDepartmentsResponse
        {
            /// <summary>
            /// Identificador del departamento
            /// </summary>
            public int IdDepartment { get; set; }

            /// <summary>
            /// Nombre del departamento
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public GetDepartmentsResponse(string name, int id)
            {
                IdDepartment = id;
                Name = name;
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetDepartmentsCommandHandler : BaseCommandHandler<GetDepartmentsRequest, List<GetDepartmentsResponse>>
        {
            private readonly IRepository<Departamento> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetDepartmentsCommandHandler(IRepository<Departamento> repository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<List<GetDepartmentsResponse>> Handle(GetDepartmentsRequest request, CancellationToken cancellationToken)
            {
                List<Departamento> dpts = await repository.GetAll().ToListAsync().ConfigureAwait(false);

                dpts = dpts.OrderBy(c => c.Nombre).ToList();

                var result = dpts.Select(dpt => new GetDepartmentsResponse(dpt.Nombre, dpt.Id));

                return result.ToList();
            }
        }
    }
}
