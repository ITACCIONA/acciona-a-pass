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
    public class GetExternalSubordinateEmployees
    {
        /// <summary>
        /// </summary>
        public class GetExternalSubordinateEmployeesRequest : IRequest<GetExternalSubordinateEmployeesResponse>
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

            /// <summary>
            /// Nombre del empleado
            /// </summary>
            public int? Page { get; set; }

            public int[] Divisiones { get; set; }

            public string[] Paises { get; set; }

            public int[] Regiones { get; set; }

            public int[] Areas { get; set; }

            public int[] Localizaciones { get; set; }

        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class GetExternalSubordinateEmployeesResponse
        {
            public GetExternalSubordinateEmployeesResponse(List<Empleado> empleados, string idioma)
            {
                Employees = new List<GetExternalSubordinateEmployeesMedicalList>();

                foreach (Empleado empleado in empleados)
                {
                    GetExternalSubordinateEmployeesMedicalList newEmpl = new GetExternalSubordinateEmployeesMedicalList()
                    {
                        IdEmpleado = empleado.Id,
                        DNI = empleado.Nif,
                        Nombre = empleado.NombreCompleto,
                        Localizacion = empleado.IdFichaLaboralNavigation?.IdLocalizacionNavigation?.Nombre
                    };

                    Employees.Add(newEmpl);
                }
            }

            /// <summary>
            /// Lista interna de objetos de tipo T.
            /// </summary>
            public List<GetExternalSubordinateEmployeesMedicalList> Employees { get; protected set; }

            /// <summary>
            /// Número de página.
            /// </summary>
            public int Page { get; set; }

            /// <summary>
            /// Elementos totales.
            /// </summary>
            public int NumElements { get; set; }

            /// <summary>
            /// Número de elementos por página.
            /// </summary>
            public int ElementsPerPage { get; set; }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class GetExternalSubordinateEmployeesMedicalList
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
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetExternalSubordinateEmployeesCommandHandler : BaseCommandHandler<GetExternalSubordinateEmployeesRequest, GetExternalSubordinateEmployeesResponse>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetExternalSubordinateEmployeesCommandHandler(IRepository<Empleado> repositoryEmpleado)
            {
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado)); ;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<GetExternalSubordinateEmployeesResponse> Handle(GetExternalSubordinateEmployeesRequest request, CancellationToken cancellationToken)
            {
                int pageSize = 50;
                int pageNumber = (request.Page ?? 1);
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
                var employees = await queryPass.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync().ConfigureAwait(false);

                SendTimeOperationToLogger("ExternalSubordinateEmployeeList Query");

                return new GetExternalSubordinateEmployeesResponse(employees, this.Idioma) { ElementsPerPage = pageSize, NumElements = numElements, Page = pageNumber };
            }
        }
    }
}
