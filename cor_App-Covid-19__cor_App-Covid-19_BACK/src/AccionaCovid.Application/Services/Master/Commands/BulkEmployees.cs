using AccionaCovid.Application.Core;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using AccionaCovid.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Master
{
    /// <summary>
    /// Servicio de volcado de empleados
    /// </summary>
    public class BulkEmployees
    {
        /// <summary>
        /// Petición del volcado de departamentos
        /// </summary>
        public class BulkEmployeesRequest : IRequest<BulkEmployeesResult>
        {
            /// <summary>
            /// Fichero CSV con localizaciones
            /// </summary>
            public IFormFile EmployeesFile { get; set; }
        }

        public class BulkEmployeesResult
        {
            public List<string> Errores { get; set; } = new List<string>();
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class BulkEmployeesCommandHandler : BaseCommandHandler<BulkEmployeesRequest, BulkEmployeesResult>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<UsuarioWorkDay> repository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> empleadoRepository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Departamento> departamentoRepository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Localizacion> localizacionRepository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Division> divisionRepository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<EstadoPasaporte> estadoPasaporteRepository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Role> roleRepository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<EmpleadoRole> empleadoRoleRepository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<FichaLaboral> fichaLaboralRepository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<FichaMedica> fichaMedicaRepository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Tecnologia> tecnologiaRepository;


            /// <summary>
            /// Constructor
            /// </summary>
            public BulkEmployeesCommandHandler(IRepository<UsuarioWorkDay> repository, IRepository<Empleado> empleadoRepository,
                IRepository<Departamento> departamentoRepository, IRepository<Localizacion> localizacionRepository, IRepository<Division> divisionRepository,
                IRepository<EstadoPasaporte> estadoPasaporteRepository, IRepository<FichaLaboral> fichaLaboralRepository,
                IRepository<FichaMedica> fichaMedicaRepository, IRepository<Role> roleRepository, IRepository<EmpleadoRole> empleadoRoleRepository,
                IRepository<Tecnologia> tecnologiaRepository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
                this.empleadoRepository = empleadoRepository ?? throw new ArgumentNullException(nameof(empleadoRepository));
                this.departamentoRepository = departamentoRepository ?? throw new ArgumentNullException(nameof(departamentoRepository));
                this.localizacionRepository = localizacionRepository ?? throw new ArgumentNullException(nameof(localizacionRepository));
                this.divisionRepository = divisionRepository ?? throw new ArgumentNullException(nameof(divisionRepository));
                this.estadoPasaporteRepository = estadoPasaporteRepository ?? throw new ArgumentNullException(nameof(estadoPasaporteRepository));
                this.roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
                this.fichaLaboralRepository = fichaLaboralRepository ?? throw new ArgumentNullException(nameof(fichaLaboralRepository));
                this.fichaMedicaRepository = fichaMedicaRepository ?? throw new ArgumentNullException(nameof(fichaMedicaRepository));
                this.empleadoRoleRepository = empleadoRoleRepository ?? throw new ArgumentNullException(nameof(empleadoRoleRepository));
                this.tecnologiaRepository = tecnologiaRepository ?? throw new ArgumentNullException(nameof(tecnologiaRepository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<BulkEmployeesResult> Handle(BulkEmployeesRequest request, CancellationToken cancellationToken)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                (string[], string[][]) csv = await BulkUtils.ReadCsvAsync(request.EmployeesFile, cancellationToken).ConfigureAwait(false);


                string[] headers = csv.Item1;
                string[][] data = csv.Item2;
                if (!data.Any())
                    return new BulkEmployeesResult();

                UsuarioWorkDay.SetPropertyIndexes(headers);

                var divisionDict = await divisionRepository.GetAll()
                    .ToDictionaryAsync(div => div.IdWorkday, div => div.Id)
                    .ConfigureAwait(false);

                var departamentosDict = await departamentoRepository.GetAll()
                    .ToDictionaryAsync(div => div.IdWorkday, div => div.Id)
                    .ConfigureAwait(false);

                var localizacionesDict = await localizacionRepository.GetAll()
                    .ToDictionaryAsync(div => div.Nombre, div => div.Id)
                    .ConfigureAwait(false);

                var tecnologiasDict = await tecnologiaRepository.GetAll()
                    .ToDictionaryAsync(div => div.Nombre, div => div.Id)
                    .ConfigureAwait(false);

                List<UsuarioWorkDay> newUsuariosWorkday = new List<UsuarioWorkDay>();
                List<Empleado> newEmpleados = new List<Empleado>();

                List<Exception> errores = new List<Exception>();
                Dictionary<long, Empleado> empleadosDict = new Dictionary<long, Empleado>();

                HashSet<long> existentIdWorkday = (await repository.GetAll().Select(u => u.IdWorkDay).ToListAsync().ConfigureAwait(false)).ToHashSet();

                int intOrder = -50000000;

                for (int i = 0; i < data.Length; i++)
                {
                    try
                    {
                        string[] dataRow = data[i];
                        UsuarioWorkDay newUsuarioWorkday = new UsuarioWorkDay(dataRow);
                        newUsuarioWorkday.Id = intOrder;

                        if (existentIdWorkday.Contains(newUsuarioWorkday.IdWorkDay))
                            continue;

                        Empleado newEmpleado = new Empleado(newUsuarioWorkday);
                        newEmpleado.Id = intOrder;
                        newEmpleado.IdFichaLaboralNavigation.Id = intOrder;
                        newEmpleado.IdFichaMedicaNavigation.Id = intOrder;

                        if (!divisionDict.ContainsKey(newUsuarioWorkday.Division))
                            throw new Exception($"No existe la división {newUsuarioWorkday.Division} en el maestro de divisiones");
                        newEmpleado.IdFichaLaboralNavigation.IdDivision = divisionDict[newUsuarioWorkday.Division];
                        
                        if (!string.IsNullOrEmpty(newUsuarioWorkday.Tecnologia))
                        {
                            if (!tecnologiasDict.ContainsKey(newUsuarioWorkday.Tecnologia))
                                throw new Exception($"No existe la tecnologia {newUsuarioWorkday.Tecnologia} en el maestro de tecnologias");
                            newEmpleado.IdFichaLaboralNavigation.IdTecnologia = tecnologiasDict[newUsuarioWorkday.Tecnologia];
                        }
                        
                        if (!departamentosDict.ContainsKey(newUsuarioWorkday.Departamento))
                            throw new Exception($"No existe el departamento {newUsuarioWorkday.Departamento} en el maestro de departamentos");
                        newEmpleado.IdFichaLaboralNavigation.IdDepartamento = departamentosDict[newUsuarioWorkday.Departamento];

                        if (!localizacionesDict.ContainsKey(newUsuarioWorkday.Localizacion))
                            throw new Exception($"No existe la localización {newUsuarioWorkday.Localizacion} en el maestro de localizaciones");
                        newEmpleado.IdFichaLaboralNavigation.IdLocalizacion = localizacionesDict[newUsuarioWorkday.Localizacion];

                        newUsuariosWorkday.Add(newUsuarioWorkday);
                        newEmpleados.Add(newEmpleado);
                        empleadosDict.Add(newUsuarioWorkday.IdWorkDay, newEmpleado);
                    }
                    catch (Exception ex)
                    {
                        errores.Add(new Exception($"Error parseando empleado: fila {i + 2}: {ex.Message}", ex));
                    }
                    finally
                    {
                        intOrder++;
                    }
                }

                SendTimeOperationToLogger("Create Entities");

                repository.UnitOfWork.BeginTransaction();

                // BULK WORKDAY
                await repository.BulkInsertAsync(newUsuariosWorkday).ConfigureAwait(false);

                newUsuariosWorkday.Select(c => { c.Empleado.First().IdUsuarioWorkDay = c.Id; return c; }).ToList();

                SendTimeOperationToLogger("Bulk Workday Entities");

                // BULK  FICHA MEDICA
                List<FichaMedica> listaFichMedica = newEmpleados.Select(c => c.IdFichaMedicaNavigation).ToList();
                await fichaMedicaRepository.BulkInsertAsync(listaFichMedica).ConfigureAwait(false);

                listaFichMedica.Select(c => { c.Empleado.First().IdFichaMedica = c.Id; return c; }).ToList();

                SendTimeOperationToLogger("Bulk FichaMedica Entities");

                // BULK FICHA LABORAL
                List<FichaLaboral> listaFichLaboral = newEmpleados.Select(c => c.IdFichaLaboralNavigation).ToList();
                await fichaLaboralRepository.BulkInsertAsync(listaFichLaboral).ConfigureAwait(false);

                listaFichLaboral.Select(c => { c.Empleado.First().IdFichaLaboral = c.Id; return c; }).ToList();

                SendTimeOperationToLogger("Bulk Ficha Laboral Entities");

                // BULK EMPLEADOS
                await empleadoRepository.BulkInsertAsync(newEmpleados).ConfigureAwait(false);

                SendTimeOperationToLogger("Bulk Empleados Entities");

                // BULK ACTUALIZACION RESPONSABLES
                listaFichLaboral = await UpdateResponsableFichaLaboral(listaFichLaboral).ConfigureAwait(false);
                await fichaLaboralRepository.BulkUpdateAsync(listaFichLaboral).ConfigureAwait(false);

                SendTimeOperationToLogger("Bulk Responsables");

                // BULK ROLE EMPLEADO
                
                List<EmpleadoRole> listaEmpleadoRole = new List<EmpleadoRole>();

                Role roleEmpleado = await roleRepository.GetAll().FirstOrDefaultAsync(c => c.Nombre == "Empleado").ConfigureAwait(false);

                newEmpleados = newEmpleados.OrderBy(c => c.Id).ToList();

                newEmpleados.Select(c =>
                {
                    int flag = 1;

                    listaEmpleadoRole.Add(new EmpleadoRole()
                    {
                        IdEmpleado = c.Id,
                        IdRole = roleEmpleado.Id,
                        LastAction = "CREATE",
                        LastActionDate = DateTime.UtcNow,
                        Id = flag
                    });

                    flag++;

                    return c;
                }).ToList();

                await empleadoRoleRepository.BulkInsertWithOutSetOutputAsync(listaEmpleadoRole).ConfigureAwait(false);

                SendTimeOperationToLogger("Bulk Roles");

                repository.UnitOfWork.Commit();

                SendTimeOperationToLogger("Commit");

                return new BulkEmployeesResult
                {
                    Errores = errores.Select(ex => ex.Message).ToList()
                };
            }

            /// <summary>
            /// Agregar identificadores de responsables en la ficha laboral
            /// </summary>
            /// <param name="listaFichLaboral"></param>
            /// <returns></returns>
            private async Task<List<FichaLaboral>> UpdateResponsableFichaLaboral(List<FichaLaboral> listaFichLaboral)
            {
                string query = "SELECT c.[IdFichaLaboral] as Id, d.id as IdResponsableDirecto FROM[dbo].[UsuarioWorkDay] a, [dbo].[UsuarioWorkDay] b, [dbo].[Empleado] c, [dbo].[Empleado] d " +
                    "WHERE b.[IdResponsable] = a.[IdWorkDay] and c.[IdUsuarioWorkDay] = b.Id and d.[IdUsuarioWorkDay] = a.Id Order by Id";

                var result = await fichaLaboralRepository.FromSqlRaw(query).Select(c => new { c.Id, c.IdResponsableDirecto }).ToListAsync().ConfigureAwait(false);

                listaFichLaboral.ForEach(c =>
                {
                    var respo = result.FirstOrDefault(d => d.Id == c.Id);

                    if (respo != null)
                    {
                        c.IdResponsableDirecto = respo.IdResponsableDirecto;
                    }

                });

                return listaFichLaboral;
            }
        }
    }
}
