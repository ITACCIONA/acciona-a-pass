using AccionaCovid.Application.Core;
using AccionaCovid.Application.Resources;
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
    public class GetEmployeesReport
    {
        /// <summary>
        /// </summary>
        public class GetEmployeesReportRequest : IRequest<IEnumerable<GetEmployeesReportResponse>>
        {
            public string Format { get; set; }
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

            public int[] Tecnologias { get; set; }

            public string[] Paises { get; set; }

            public int[] Regiones { get; set; }

            public int[] Areas { get; set; }

            public int[] Localizaciones { get; set; }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class GetEmployeesReportResponse
        {
            /// <summary>
            /// Id empleado
            /// </summary>
            public int IdEmpleado { get; set; }

            /// <summary>
            /// Nombre del empleado
            /// </summary>
            public string Nombre { get; set; }

            /// <summary>
            /// NumEmpleado
            /// </summary>
            public long NumEmpleado { get; set; }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class GetEmployeesReportMedSerResponse : GetEmployeesReportResponse
        {
            /// <summary>
            /// Genro
            /// </summary>
            public string Genero { get; set; }

            /// <summary>
            /// Edad
            /// </summary>
            public int? Edad { get; set; }

            /// <summary>
            /// Edad
            /// </summary>
            public DateTime UltiModifi { get; set; }

            /// <summary>
            /// Estado
            /// </summary>
            public string Estado { get; set; }

            /// <summary>
            /// Color
            /// </summary>
            public string ColorPasaporte { get; set; }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class GetEmployeesReportPRLResponse : GetEmployeesReportResponse
        {
            /// <summary>
            /// Color
            /// </summary>
            public string ColorPasaporte { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetEmployeesReportCommandHandler : BaseCommandHandler<GetEmployeesReportRequest, IEnumerable<GetEmployeesReportResponse>>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Pasaporte> repositoryPasaporte;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<EstadoPasaporte> repositoryEstadoPasaporte;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetEmployeesReportCommandHandler(IRepository<Pasaporte> repositoryPasaporte, IRepository<Empleado> repositoryEmpleado, IRepository<EstadoPasaporte> repositoryEstadoPasaporte)
            {
                this.repositoryPasaporte = repositoryPasaporte ?? throw new ArgumentNullException(nameof(repositoryPasaporte));
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado));
                this.repositoryEstadoPasaporte = repositoryEstadoPasaporte ?? throw new ArgumentNullException(nameof(repositoryEstadoPasaporte));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<IEnumerable<GetEmployeesReportResponse>> Handle(GetEmployeesReportRequest request, CancellationToken cancellationToken)
            {
                bool descending = (request.OrderByDescending ?? false);

                // QUERY
                var queryPass = repositoryPasaporte.GetAll()
                    .Include(c => c.IdEstadoPasaporteNavigation)
                        .ThenInclude(e => e.IdColorEstadoNavigation)
                    .Include(c => c.IdEstadoPasaporteNavigation)
                        .ThenInclude(e => e.EstadoPasaporteIdioma)
                    .Include(c => c.IdEstadoPasaporteNavigation)
                        .ThenInclude(e => e.IdTipoEstadoNavigation)
                    .Include(c => c.IdEmpleadoNavigation)
                        .ThenInclude(c => c.IdFichaLaboralNavigation)
                            .ThenInclude(c => c.IdLocalizacionNavigation)
                                .ThenInclude(c => c.Area)
                    .Where(c => c.Activo.Value);

                Expression<Func<Pasaporte, bool>> filterExpression = p => true;
                // FILTERS
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    filterExpression = filterExpression.And(s => s.IdEmpleadoNavigation.Nombre.Contains(request.Nombre) ||
                   s.IdEmpleadoNavigation.Apellido.Contains(request.Nombre) ||
                   (s.IdEmpleadoNavigation.Nombre + " " + s.IdEmpleadoNavigation.Apellido).Contains(request.Nombre));
                }
                if (request.Divisiones?.Any() == true)
                {
                    filterExpression = filterExpression.And(p => p.IdEmpleadoNavigation.IdFichaLaboralNavigation.IdDivision != null &&
                                                                 request.Divisiones.Contains(p.IdEmpleadoNavigation.IdFichaLaboralNavigation.IdDivision.Value));
                }
                if (request.Tecnologias?.Any() == true)
                {
                    filterExpression = filterExpression.And(p => p.IdEmpleadoNavigation.IdFichaLaboralNavigation.IdTecnologia != null &&
                                                                 request.Tecnologias.Contains(p.IdEmpleadoNavigation.IdFichaLaboralNavigation.IdTecnologia.Value));
                }
                if (request.Paises?.Any() == true)
                {
                    filterExpression = filterExpression.And(p => request.Paises.Contains(p.IdEmpleadoNavigation.IdFichaLaboralNavigation.IdLocalizacionNavigation.Pais));
                }
                if (request.Regiones?.Any() == true)
                {
                    filterExpression = filterExpression.And(p => request.Regiones.Contains(p.IdEmpleadoNavigation.IdFichaLaboralNavigation.IdLocalizacionNavigation.Area.IdRegion));
                }
                if (request.Areas?.Any() == true)
                {
                    filterExpression = filterExpression.And(p => p.IdEmpleadoNavigation.IdFichaLaboralNavigation.IdLocalizacionNavigation.IdArea != null &&
                                                                 request.Areas.Contains(p.IdEmpleadoNavigation.IdFichaLaboralNavigation.IdLocalizacionNavigation.IdArea.Value));
                }
                if (request.Localizaciones?.Any() == true)
                {
                    filterExpression = filterExpression.And(p => request.Localizaciones.Contains(p.IdEmpleadoNavigation.IdFichaLaboralNavigation.IdLocalizacion.GetValueOrDefault()));
                }
                queryPass = queryPass.Where(filterExpression);

                // ORDERS
                switch (request.SortOrder)
                {
                    case "Nombre":
                        queryPass = descending ? queryPass.OrderByDescending(s => s.IdEmpleadoNavigation.Nombre) : queryPass.OrderBy(s => s.IdEmpleadoNavigation.Nombre);
                        break;
                    case "NumEmpleado":
                        queryPass = descending ? queryPass.OrderByDescending(p => p.IdEmpleadoNavigation.NumEmpleado ?? p.IdEmpleado) : queryPass.OrderBy(p => p.IdEmpleadoNavigation.NumEmpleado ?? p.IdEmpleado);
                        break;
                    case "Edad":
                        queryPass = descending ? queryPass.OrderByDescending(s => s.IdEmpleadoNavigation.FechaNacimiento) : queryPass.OrderBy(s => s.IdEmpleadoNavigation.FechaNacimiento);
                        break;
                    case "Genero":
                        queryPass = descending ? queryPass.OrderByDescending(s => s.IdEmpleadoNavigation.Genero) : queryPass.OrderBy(s => s.IdEmpleadoNavigation.Genero);
                        break;
                    case "UltimaModif":
                        queryPass = descending ? queryPass.OrderByDescending(s => s.IdEmpleadoNavigation.UltimaModif) : queryPass.OrderBy(s => s.IdEmpleadoNavigation.UltimaModif);
                        break;
                    case "Estado":
                        queryPass = descending ? queryPass.OrderByDescending(s => s.IdEstadoPasaporteNavigation.IdTipoEstadoNavigation.Prioridad) : queryPass.OrderBy(s => s.IdEstadoPasaporteNavigation.IdTipoEstadoNavigation.Prioridad);
                        break;
                    default:
                        queryPass = descending ? queryPass.OrderByDescending(s => s.IdEmpleadoNavigation.Nombre) : queryPass.OrderBy(s => s.IdEmpleadoNavigation.Nombre);
                        break;
                }

                //Comprobamos el perfil PRL para filtrar las localizaciones, pero que no sea ServiciosMedicos o RRHH, lo vería todo
                //if (Roles.Contains("PRL") && !Roles.Contains("ServicioMedico") && !Roles.Contains("RRHHDesc") && !Roles.Contains("RRHHCent"))
                //{
                //    //Obtenemos los Ámbitos del PRL
                //    var queryAmbitosResult = await repositoryEmpleado.GetBy(e => e.Id == this.IdUser)
                //                                    .Select(e => new {
                //                                        IdAreaList = e.AmbitoAccesoEmpleadoArea.Select(aaea => aaea.IdArea).ToList(),
                //                                        IdPaisList = e.AmbitoAccesoEmpleadoPais.Select(aaea => aaea.Pais.Nombre).ToList(),
                //                                        IdAreaRegionList = e.AmbitoAccesoEmpleadoRegion.SelectMany(aaea => aaea.Region.Area.Select(a => a.Id)).ToList()
                //                                    }).FirstOrDefaultAsync().ConfigureAwait(false);
                //    //Agregamos los filtros pertinentes
                //    if (queryAmbitosResult.IdAreaList.Any() || queryAmbitosResult.IdAreaRegionList.Any())
                //    {
                //        List<int> idsTemp = new List<int>();
                //        idsTemp.AddRange(queryAmbitosResult.IdAreaList);
                //        idsTemp.AddRange(queryAmbitosResult.IdAreaRegionList);
                //        queryPass = queryPass.Where(p => p.IdEmpleadoNavigation.LocalizacionEmpleados.Any(l => l.IdLocalizacionNavigation.IdArea != null && idsTemp.Contains(l.IdLocalizacionNavigation.IdArea.Value)));
                //    }
                //    if (queryAmbitosResult.IdPaisList.Any())
                //    {
                //        List<string> idsTemp = new List<string>();
                //        idsTemp.AddRange(queryAmbitosResult.IdPaisList);
                //        queryPass.Where(p => p.IdEmpleadoNavigation.LocalizacionEmpleados.Any(l => idsTemp.Contains(l.IdLocalizacionNavigation.Pais)));
                //    }
                //    //Si no tiene ningún ámbito, pero es PRL no podrá ver nada
                //    if (!queryAmbitosResult.IdAreaList.Any() && !queryAmbitosResult.IdAreaRegionList.Any() && !queryAmbitosResult.IdPaisList.Any())
                //    {
                //        queryPass = queryPass.Where(p => false);
                //    }
                //}

                // PAGGING
                int numElements = await queryPass.CountAsync().ConfigureAwait(false);
                var passports = await queryPass.ToListAsync().ConfigureAwait(false);

                SendTimeOperationToLogger("EmployeeList Query");

                if (Roles.Contains("ServicioMedico"))
                {
                    return passports.Select(pasaporte => new GetEmployeesReportMedSerResponse()
                    {
                        IdEmpleado = pasaporte.IdEmpleado,
                        Genero = pasaporte.IdEmpleadoNavigation.Genero != null ? ((Empleado.Gender)pasaporte.IdEmpleadoNavigation.Genero).ToString() : null,
                        Nombre = pasaporte.IdEmpleadoNavigation.NombreCompleto,
                        NumEmpleado = pasaporte.IdEmpleadoNavigation.NumEmpleado ?? pasaporte.IdEmpleado,
                        UltiModifi = pasaporte.IdEmpleadoNavigation.UltimaModif,
                        Estado = pasaporte.IdEstadoPasaporteNavigation?.EstadoPasaporteIdioma.FirstOrDefault(c => c.Idioma == this.Idioma)?.Nombre ?? pasaporte.IdEstadoPasaporteNavigation?.Nombre,
                        ColorPasaporte = Data.ResourceManager.GetString(pasaporte.IdEstadoPasaporteNavigation?.IdColorEstadoNavigation?.Nombre),
                        Edad = !pasaporte.IdEmpleadoNavigation.FechaNacimiento.HasValue ? default(int?) :
                                    pasaporte.IdEmpleadoNavigation.CalcularEdad()
                    }).ToList();
                }
                else if(Roles.Contains("PRL"))
                {
                    return passports.Select(pasaporte => new GetEmployeesReportPRLResponse()
                    {
                        IdEmpleado = pasaporte.IdEmpleado,
                        Nombre = pasaporte.IdEmpleadoNavigation.NombreCompleto,
                        NumEmpleado = pasaporte.IdEmpleadoNavigation.NumEmpleado ?? pasaporte.IdEmpleado,
                        ColorPasaporte = Data.ResourceManager.GetString(pasaporte.IdEstadoPasaporteNavigation?.IdColorEstadoNavigation?.Nombre)
                    }).ToList();
                }
                else
                {
                    return passports.Select(pasaporte => new GetEmployeesReportResponse()
                    {
                        IdEmpleado = pasaporte.IdEmpleado,
                        Nombre = pasaporte.IdEmpleadoNavigation.NombreCompleto,
                        NumEmpleado = pasaporte.IdEmpleadoNavigation.NumEmpleado ?? pasaporte.IdEmpleado,
                    }).ToList();
                }
            }
        }
    }
}
