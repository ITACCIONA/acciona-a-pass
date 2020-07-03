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
    /// Servicio de volcado de empleados externos
    /// </summary>
    public class BulkEmployeesExternal
    {
        /// <summary>
        /// Petición del volcado de empleados exernos
        /// </summary>
        public class BulkEmployeesExternalRequest : IRequest<BulkEmployeesExternalResult>
        {
            /// <summary>
            /// Fichero CSV con empleados
            /// </summary>
            public IFormFile EmployeesFile { get; set; }
            public string Origen { get; set; }
        }

        public class BulkEmployeesExternalResult
        {
            public List<string> Errores { get; set; } = new List<string>();
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class BulkEmployeesExternalCommandHandler : BaseCommandHandler<BulkEmployeesExternalRequest, BulkEmployeesExternalResult>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<IntegracionExternos> repository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> empleadoRepository;

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
            private readonly IRepository<AdjudicacionTrabajoObra> adjudicacionObraRepository;


            /// <summary>
            /// Constructor
            /// </summary>
            public BulkEmployeesExternalCommandHandler(IRepository<IntegracionExternos> repository, IRepository<Empleado> empleadoRepository,
                IRepository<EstadoPasaporte> estadoPasaporteRepository, IRepository<FichaLaboral> fichaLaboralRepository,
                IRepository<FichaMedica> fichaMedicaRepository, IRepository<Role> roleRepository, IRepository<EmpleadoRole> empleadoRoleRepository
                , IRepository<AdjudicacionTrabajoObra> adjudicacionObraRepository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
                this.empleadoRepository = empleadoRepository ?? throw new ArgumentNullException(nameof(empleadoRepository));
                //repository para adjudicación obra
                this.adjudicacionObraRepository = adjudicacionObraRepository ?? throw new ArgumentNullException(nameof(adjudicacionObraRepository));
                this.estadoPasaporteRepository = estadoPasaporteRepository ?? throw new ArgumentNullException(nameof(estadoPasaporteRepository));
                this.roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
                this.fichaLaboralRepository = fichaLaboralRepository ?? throw new ArgumentNullException(nameof(fichaLaboralRepository));
                this.fichaMedicaRepository = fichaMedicaRepository ?? throw new ArgumentNullException(nameof(fichaMedicaRepository));
                this.empleadoRoleRepository = empleadoRoleRepository ?? throw new ArgumentNullException(nameof(empleadoRoleRepository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<BulkEmployeesExternalResult> Handle(BulkEmployeesExternalRequest request, CancellationToken cancellationToken)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                

                (string[], string[][]) csv = await BulkUtils.ReadCsvAsync(request.EmployeesFile, cancellationToken).ConfigureAwait(false);


                string[] headers = csv.Item1;
                string[][] data = csv.Item2;
                if (!data.Any())
                    return new BulkEmployeesExternalResult();

                IntegracionExternos.SetPropertyIndexes(headers);

                //adjudicación obra diccionario
                //var adjudicacionDict = await adjudicacionObraRepository.GetAll()
                //     .ToDictionaryAsync(div => div.IdObra, div => div.Id)
                //     .ConfigureAwait(false);

                List<IntegracionExternos> newIntegracionExternos = new List<IntegracionExternos>();
                List<Empleado> newEmpleados = new List<Empleado>();

                List<Exception> errores = new List<Exception>();
                Dictionary<string, Empleado> empleadosDict = new Dictionary<string, Empleado>();

                HashSet<string> existentIdIntegracionExternos = (await repository.GetAll().Select(u => u.Nif).ToListAsync().ConfigureAwait(false)).ToHashSet();
                HashSet<string> existentNifs = new HashSet<string>();

                int intOrder = -50000000;
                //int intOrder = empleadoRepository.GetAll().Last().Id;

                foreach (string[] dataRow in data)
                {
                    try
                    {
                        IntegracionExternos newIntegracionExterno = new IntegracionExternos(dataRow);
                        newIntegracionExterno.Id = intOrder;
                        newIntegracionExterno.Origen = request.Origen;

                        //Formateamos el NIF
                        newIntegracionExterno.Nif = newIntegracionExterno.NifOriginal.Trim().ToUpper().Replace("-", "").Replace("_", "").Replace("/", ""); 

                        //Comprobamos que en la bbdd no se hubiera creado ya ese empleado
                        if (existentIdIntegracionExternos.Contains(newIntegracionExterno.Nif))
                        {
                            //errores.Add(new Exception($"Fila {intOrder + 50000002}: El usuario con NIF: {newIntegracionExterno.Nif} ya está registrado en integracion externo."));
                            continue;
                        }
                        //Comprobamos que no se haya insertado ya 
                        if (existentNifs.Contains(newIntegracionExterno.Nif))
                        {
                            //errores.Add(new Exception($"Fila {intOrder + 50000002}: El usuario con NIF: {newIntegracionExterno.Nif} está repetido."));
                            continue;
                        }
                        if (newIntegracionExterno.Nif.Length > 13)
                        {
                            errores.Add(new Exception($"Fila {intOrder + 50000002}: El usuario con NIF: {newIntegracionExterno.Nif} no se ha insertado correctamente. Formato de DNI conflictivo"));
                            continue;
                        }

                        Empleado newEmpleado = new Empleado(newIntegracionExterno);
                        newEmpleado.Id = intOrder;
                        newEmpleado.IdFichaLaboralNavigation.Id = intOrder;
                        newEmpleado.IdFichaMedicaNavigation.Id = intOrder;
                        newEmpleado.InterAcciona = false;

                        //if (!adjudicacionDict.ContainsKey(newIntegracionExterno.codobra))
                        //    throw new Exception($"No existe la obra  {newIntegracionExterno.codobra} en el maestro de obras");
                        //newEmpleado.AdjudicacionTrabajoObra.IdDivision = divisionDict[newIntegracionExterno.Division];

                        newIntegracionExternos.Add(newIntegracionExterno);
                        newEmpleados.Add(newEmpleado);
                        empleadosDict.Add(newIntegracionExterno.Nif, newEmpleado);

                        existentNifs.Add(newIntegracionExterno.Nif);
                    }
                    catch (Exception ex)
                    {
                        errores.Add(ex);
                    }
                    finally
                    {
                        intOrder++;
                    }
                }

                SendTimeOperationToLogger("Create Entities");

                repository.UnitOfWork.BeginTransaction();

                // BULK WORKDAY
                await repository.BulkInsertAsync(newIntegracionExternos).ConfigureAwait(false);

                newIntegracionExternos.Select(c => { c.Empleado.First().IdIntegracionExternos = c.Id; return c; }).ToList();

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

                return new BulkEmployeesExternalResult
                {
                    Errores = errores.Select(ex => ex.Message).ToList()
                };
            }

        }
    }
}
