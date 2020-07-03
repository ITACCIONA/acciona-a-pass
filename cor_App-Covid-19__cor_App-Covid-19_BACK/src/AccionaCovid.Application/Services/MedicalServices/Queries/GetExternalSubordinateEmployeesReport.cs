using AccionaCovid.Application.Core;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.MedicalServices
{
    /// <summary>
    /// Servicio
    /// </summary>
    public class GetExternalSubordinateEmployeesReport
    {
        /// <summary>
        /// </summary>
        public class GetExternalSubordinateEmployeesReportRequest : IRequest<IEnumerable<GetExternalSubordinateEmployeesReportResponse>>
        {
            /// <summary>
            /// Nombre del empleado
            /// </summary>
            public string SortOrder { get; set; }

            /// <summary>
            /// Nombre del empleado
            /// </summary>
            public bool? OrderByDescending { get; set; }

            /// <summary>
            /// Nombre del empleado
            /// </summary>
            public string Nombre { get; set; }

            public int[] Divisiones { get; set; }

            public string[] Paises { get; set; }

            public int[] Regiones { get; set; }

            public int[] Areas { get; set; }

            public int[] Localizaciones { get; set; }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class GetExternalSubordinateEmployeesReportResponse
        {
            /// <summary>
            /// Id empleado
            /// </summary>
            public int IdEmpleado { get; set; }

            /// <summary>
            /// Nombre del empleado externo
            /// </summary>
            public string Nombre { get; set; }

            /// <summary>
            /// DNI del empleado externo
            /// </summary>
            public string DNI { get; set; }

            /// <summary>
            /// Localización del empleado externo
            /// </summary>
            public string Localizacion { get; set; }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class GetExternalSubordinateEmployeesReportMedicalList
        {
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetExternalSubordinateEmployeesReportCommandHandler : BaseCommandHandler<GetExternalSubordinateEmployeesReportRequest, IEnumerable<GetExternalSubordinateEmployeesReportResponse>>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetExternalSubordinateEmployeesReportCommandHandler(IRepository<Empleado> repositoryEmpleado)
            {
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado)); ;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<IEnumerable<GetExternalSubordinateEmployeesReportResponse>> Handle(GetExternalSubordinateEmployeesReportRequest request, CancellationToken cancellationToken)
            {
                bool descending = (request.OrderByDescending ?? false);

                // QUERY
                var queryPass = repositoryEmpleado.GetAll()
                    .Include(e => e.IdFichaLaboralNavigation)
                        .ThenInclude(fl => fl.IdLocalizacionNavigation)
                    .Include(e => e.AspNetUsers)
                    .Where(e => e.IdFichaLaboralNavigation.IdResponsableDirecto == IdUser &&
                                e.IdFichaLaboralNavigation.IsExternal == true &&
                                e.AspNetUsers.Any());


                Expression<Func<Empleado, bool>> filterExpression = e => true;
                // FILTERS
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    filterExpression = filterExpression.And(e => e.Nombre.Contains(request.Nombre) ||
                   e.Apellido.Contains(request.Nombre) ||
                   (e.Nombre + " " + e.Apellido).Contains(request.Nombre));
                }
                if (request.Divisiones?.Any() == true)
                {
                    filterExpression = filterExpression.And(e => e.IdFichaLaboralNavigation.IdDivision != null &&
                                                                 request.Divisiones.Contains(e.IdFichaLaboralNavigation.IdDivision.Value));
                }
                if (request.Paises?.Any() == true)
                {
                    filterExpression = filterExpression.And(e => request.Paises.Contains(e.IdFichaLaboralNavigation.IdLocalizacionNavigation.Pais));
                }
                if (request.Regiones?.Any() == true)
                {
                    filterExpression = filterExpression.And(e => request.Regiones.Contains(e.IdFichaLaboralNavigation.IdLocalizacionNavigation.Area.IdRegion));
                }
                if (request.Areas?.Any() == true)
                {
                    filterExpression = filterExpression.And(e => e.IdFichaLaboralNavigation.IdLocalizacionNavigation.IdArea != null &&
                                                                 request.Areas.Contains(e.IdFichaLaboralNavigation.IdLocalizacionNavigation.IdArea.Value));
                }
                if (request.Localizaciones?.Any() == true)
                {
                    filterExpression = filterExpression.And(e => request.Localizaciones.Contains(e.IdFichaLaboralNavigation.IdLocalizacion.GetValueOrDefault()));
                }
                queryPass = queryPass.Where(filterExpression);

                // ORDERS
                switch (request.SortOrder)
                {
                    case "Nombre":
                        queryPass = descending ? queryPass.OrderByDescending(e => e.Nombre) : queryPass.OrderBy(e => e.Nombre);
                        break;
                    case "DNI":
                        queryPass = descending ? queryPass.OrderByDescending(e => e.Nif) : queryPass.OrderBy(e => e.Nif);
                        break;
                    case "Localizacion":
                        queryPass = descending ? queryPass.OrderByDescending(e => e.IdFichaLaboralNavigation.IdLocalizacionNavigation.Nombre) : queryPass.OrderBy(e => e.IdFichaLaboralNavigation.IdLocalizacionNavigation.Nombre);
                        break;
                    default:
                        queryPass = descending ? queryPass.OrderByDescending(e => e.Nombre) : queryPass.OrderBy(e => e.Nombre);
                        break;
                }

                // PAGGING
                int numElements = await queryPass.CountAsync().ConfigureAwait(false);
                var result = await queryPass
                    .Select(x => new GetExternalSubordinateEmployeesReportResponse() { 
                        DNI = x.Nif,
                        IdEmpleado = x.Id,
                        Nombre = x.NombreCompleto,
                        Localizacion = x.IdFichaLaboralNavigation.IdLocalizacionNavigation.Nombre
                    })
                    .ToListAsync().ConfigureAwait(false);

                SendTimeOperationToLogger("ExternalSubordinateEmployeeList Query");

                return result;
            }
        }
    }
}
