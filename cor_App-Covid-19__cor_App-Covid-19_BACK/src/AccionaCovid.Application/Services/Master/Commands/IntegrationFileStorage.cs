using AccionaCovid.Application.Core;
using AccionaCovid.Crosscutting;
using AccionaCovid.Crosscutting.Domain;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using AccionaCovid.Domain.Repositories;
using AccionaCovid.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Master
{
    /// <summary>
    /// Servicio de volcado de empleados
    /// </summary>
    public class IntegrationFileStorage
    {
        /// <summary>
        /// Petición del volcado de departamentos
        /// </summary>
        public class IntegrationFileStorageRequest : IRequest<IntegrationFileStorageResult> { }

        public class IntegrationFileStorageResult
        {
            public List<string> Errores { get; set; } = new List<string>();
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class IntegrationFileStorageCommandHandler : BaseCommandHandler<IntegrationFileStorageRequest, IntegrationFileStorageResult>
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
            /// Servicio de almacenamiento de ficheros
            /// </summary>
            private readonly IFileStorageService fileStorageService;

            /// <summary>
            /// repositorio manejador de ficheros zip
            /// </summary>
            private readonly IZipFilesRepository zipFilesRepository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IResultadosIntegracionRepository resultadosIntegracionRepository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Tecnologia> tecnologiaRepository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Area> areaRepository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Region> regionRepository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Pais> paisRepository;

            /// <summary>
            /// Opciones de configuracion del sistema de file storage
            /// </summary>
            private readonly IntegrationSettings integrationSettings;

            /// <summary>
            /// Constructor
            /// </summary>
            public IntegrationFileStorageCommandHandler(IRepository<UsuarioWorkDay> repository, IRepository<Empleado> empleadoRepository,
                IRepository<Departamento> departamentoRepository, IRepository<Localizacion> localizacionRepository, IRepository<Division> divisionRepository,
                IRepository<EstadoPasaporte> estadoPasaporteRepository, IRepository<FichaLaboral> fichaLaboralRepository,
                IRepository<FichaMedica> fichaMedicaRepository, IRepository<Role> roleRepository, IRepository<EmpleadoRole> empleadoRoleRepository,
                IResultadosIntegracionRepository resultadosIntegracionRepository, 
                IRepository<Area> areaRepository, IRepository<Region> regionRepository, IRepository<Pais> paisRepository,
                IRepository<Tecnologia> tecnologiaRepository, IFileStorageService fileStorageService,
                IZipFilesRepository zipFilesRepository, IntegrationSettings integrationSettings)
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
                this.resultadosIntegracionRepository = resultadosIntegracionRepository ?? throw new ArgumentNullException(nameof(resultadosIntegracionRepository));
                this.tecnologiaRepository = tecnologiaRepository ?? throw new ArgumentNullException(nameof(tecnologiaRepository));
                this.areaRepository = areaRepository ?? throw new ArgumentNullException(nameof(areaRepository));
                this.regionRepository = regionRepository ?? throw new ArgumentNullException(nameof(regionRepository));
                this.paisRepository = paisRepository ?? throw new ArgumentNullException(nameof(paisRepository));
                this.fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));
                this.integrationSettings = integrationSettings ?? throw new ArgumentNullException(nameof(integrationSettings));
                this.zipFilesRepository = zipFilesRepository ?? throw new ArgumentNullException(nameof(zipFilesRepository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<IntegrationFileStorageResult> Handle(IntegrationFileStorageRequest request, CancellationToken cancellationToken)
            {
                IEnumerable<string> fileList = await fileStorageService.ListFilesAsync(integrationSettings.IntegrationPath).ConfigureAwait(false);

                //Comprobamos si los ficheros han sido procesados
                List<string> fileListProcessed = resultadosIntegracionRepository
                    .GetBy(r => fileList.Contains(r.OriginFileName))
                    .Select(r => r.OriginFileName)
                    .Distinct().ToList();

                List<string> fileListToProcess = fileList.Except(fileListProcessed).ToList();
                Regex regex = new Regex(integrationSettings.FileNamePatern);
                foreach (string fileToProcess in fileListToProcess.OrderBy(fn => fn))
                {
                    if (!regex.IsMatch(fileToProcess))
                        continue;
                    byte[] fileContent = await fileStorageService.GetFileContentAsync($"{integrationSettings.IntegrationPath}/{fileToProcess}");
                    Dictionary<string, string> intFiles = zipFilesRepository.GetZippedFilesContentStringAsync(fileContent);

                    string fileLocations = string.Empty, fileDepartments = string.Empty, fileEmployees = string.Empty;

                    foreach (var item in intFiles)
                    {
                        if (item.Key.StartsWith("CCL")) fileLocations = item.Value;
                        else if (item.Key.StartsWith("CCO")) fileDepartments = item.Value;
                        else if (item.Key.StartsWith("CD")) fileEmployees = item.Value;
                    }

                    try
                    {
                        //Fijamos una fecha para todas las trazas de la integración
                        resultadosIntegracionRepository.IntegrationProcessDateTime = DateTime.Now;
                        //Establecemos el fichero zip de migración para luego poder filtrar los procesados
                        resultadosIntegracionRepository.ProcessedFileName = fileToProcess;

                        //Todo el proceso está aropado por una transacción
                        repository.UnitOfWork.BeginTransaction();

                        //Integración localizaciones
                        (string[], string[][]) csvLocations = BulkUtils.ReadCsvFromStringAsync(fileLocations);
                        await ImportLocalizations(csvLocations).ConfigureAwait(false);
                        await localizacionRepository.SaveChangesAsync().ConfigureAwait(false);
                        SendTimeOperationToLogger("Bulk Localizaciones");
                        //Integración Departamentos
                        (string[], string[][]) csvDapartments = BulkUtils.ReadCsvFromStringAsync(fileDepartments);
                        await ImportDepartments(csvDapartments).ConfigureAwait(false);
                        await departamentoRepository.SaveChangesAsync().ConfigureAwait(false);
                        SendTimeOperationToLogger("Bulk Departamentos");
                        //Integración Empleados
                        (string[], string[][]) csvEmployees = BulkUtils.ReadCsvFromStringAsync(fileEmployees);
                        await ImportEmployees(csvEmployees).ConfigureAwait(false);

                        //Guardamos todos los cambios
                        await resultadosIntegracionRepository.SaveChangesAsync().ConfigureAwait(false);

                        repository.UnitOfWork.Commit();

                        SendTimeOperationToLogger("Commit");
                    }
                    catch (Exception)
                    {
                        repository.UnitOfWork.Rollback();
                        throw;
                    }
                }

                return new IntegrationFileStorageResult();
            }

            private async Task ImportEmployees((string[], string[][]) csvEmployees)
            {
                string[] headers = csvEmployees.Item1;
                string[][] data = csvEmployees.Item2;
                if (!data.Any())
                    return;

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

                List<UsuarioWorkDay> importUsuariosWorkday = new List<UsuarioWorkDay>();
                List<Empleado> importEmpleados = new List<Empleado>();

                List<Exception> errores = new List<Exception>();
                resultadosIntegracionRepository.AddMessage("EMPLEADOS", $"Se han encontrado {data.Length - 1} filas para importar.");
                for (int i = 0; i < data.Length; i++)
                {
                    try
                    {
                        string[] dataRow = data[i];
                        UsuarioWorkDay importUsuarioWorkday = new UsuarioWorkDay(dataRow);

                        if (importUsuarioWorkday.ImportAction != OpcionesIntegracion.IntegracionDeleteAction)
                        {
                            Empleado importEmpleado = new Empleado(importUsuarioWorkday);
                            importEmpleado.IdUsuarioWorkDayNavigation = importUsuarioWorkday;

                            if (!divisionDict.ContainsKey(importUsuarioWorkday.Division))
                                throw new Exception($"No existe la división {importUsuarioWorkday.Division} en el maestro de divisiones");
                            importEmpleado.IdFichaLaboralNavigation.IdDivision = divisionDict[importUsuarioWorkday.Division];

                            if (!string.IsNullOrEmpty(importUsuarioWorkday.Tecnologia))
                            {
                                if (!tecnologiasDict.ContainsKey(importUsuarioWorkday.Tecnologia))
                                    throw new Exception($"No existe la tecnologia {importUsuarioWorkday.Tecnologia} en el maestro de tecnologias");
                                importEmpleado.IdFichaLaboralNavigation.IdTecnologia = tecnologiasDict[importUsuarioWorkday.Tecnologia];
                            }

                            if (!departamentosDict.ContainsKey(importUsuarioWorkday.Departamento))
                                throw new Exception($"No existe el departamento {importUsuarioWorkday.Departamento} en el maestro de departamentos");
                            importEmpleado.IdFichaLaboralNavigation.IdDepartamento = departamentosDict[importUsuarioWorkday.Departamento];

                            if (!localizacionesDict.ContainsKey(importUsuarioWorkday.Localizacion))
                                throw new Exception($"No existe la localización {importUsuarioWorkday.Localizacion} en el maestro de localizaciones");
                            importEmpleado.IdFichaLaboralNavigation.IdLocalizacion = localizacionesDict[importUsuarioWorkday.Localizacion];
                            importEmpleados.Add(importEmpleado);
                        }
                        importUsuariosWorkday.Add(importUsuarioWorkday);
                    }
                    catch (Exception ex)
                    {
                        StringBuilder sbErrorMeessage = new StringBuilder($"Error parseando empleado: fila {i + 2} ");
                        sbErrorMeessage.AppendLine($"Exception Message: {ex.Message}");
                        sbErrorMeessage.AppendLine($"Exception StackTrace: {ex.StackTrace}");
                        sbErrorMeessage.AppendLine($"Line content: {string.Join("; ", data[i])}");
                        errores.Add(new Exception(sbErrorMeessage.ToString(), ex));
                    }
                }

                errores.ForEach(e => resultadosIntegracionRepository.AddError("EMPLEADOS", e.Message));

                SendTimeOperationToLogger("Create Entities");

                await BulkInsertNewEmployees(importUsuariosWorkday, importEmpleados).ConfigureAwait(false);

                await BulkUpdateNewEmployees(importUsuariosWorkday, importEmpleados).ConfigureAwait(false);

                await BulkDeleteNewEmployees(importUsuariosWorkday).ConfigureAwait(false);
            }

            private async Task BulkDeleteNewEmployees(IEnumerable<UsuarioWorkDay> importUsuariosWorkday)
            {
                List<long> IdsDeleteUsuariosWorkDay = importUsuariosWorkday
                    .Where(u => u.ImportAction == OpcionesIntegracion.IntegracionDeleteAction)
                    .Select(u => u.IdWorkDay).ToList();

                if (!IdsDeleteUsuariosWorkDay.Any())
                    return;

                var updateUsuariosWorkday = await repository.GetBy(u => IdsDeleteUsuariosWorkDay.Contains(u.IdWorkDay))
                    .ToListAsync().ConfigureAwait(false);

                //Traceamos los usuario marcados como delete que no existen en BBDD
                IdsDeleteUsuariosWorkDay.Except(updateUsuariosWorkday.Select(u => u.IdWorkDay)).ForEach(idError =>
                {
                    resultadosIntegracionRepository.AddError("EMPLEADOS",
                        $"El UsuarioWorkday con idWorkday {idError} estaba marcado como borrado, pero no existe en la tabla de destino.");
                });

                // WORKDAY
                resultadosIntegracionRepository.AddCountDelete("EMPLEADOS", updateUsuariosWorkday);
                repository.RemoveRange(updateUsuariosWorkday);

                SendTimeOperationToLogger("Bulk delete Workday Entities");

                var deleteEmpleados = await empleadoRepository
                    .GetBy(e => IdsDeleteUsuariosWorkDay.Contains(e.IdUsuarioWorkDayNavigation.IdWorkDay))
                    .Include(e => e.IdUsuarioWorkDayNavigation)
                    .Include(e => e.IdFichaLaboralNavigation)
                    .Include(e => e.IdFichaMedicaNavigation)
                    .Include(e => e.EmpleadoRole)
                    .ToListAsync().ConfigureAwait(false);

                IdsDeleteUsuariosWorkDay.Except(deleteEmpleados.Select(e => Convert.ToInt64(e.IdUsuarioWorkDayNavigation.IdWorkDay))).ForEach(idError =>
                {
                    resultadosIntegracionRepository.AddError("EMPLEADOS",
                        $"El empleado con IdUsuarioWorkDayNavigation.IdWorkDay {idError} estaba marcado como borrado, pero no existe en la tabla de destino.");
                });

                var empleadoRoles = deleteEmpleados.SelectMany(e => e.EmpleadoRole).ToList();
                resultadosIntegracionRepository.AddCountDelete("EMPLEADOS", empleadoRoles);
                empleadoRoleRepository.RemoveRange(empleadoRoles);
                SendTimeOperationToLogger("Delete EmpleadosRole Entities");

                List<FichaMedica> fichasMedicas = deleteEmpleados.Select(e => e.IdFichaMedicaNavigation).ToList();

                // FICHAS MEDICAS
                resultadosIntegracionRepository.AddCountDelete("EMPLEADOS", fichasMedicas);
                fichaMedicaRepository.RemoveRange(fichasMedicas);
                SendTimeOperationToLogger("DELETE fichasMedica");

                List<FichaLaboral> fichasLaborales = deleteEmpleados.Select(e => e.IdFichaLaboralNavigation).ToList();

                // FICHAS LABORALES
                resultadosIntegracionRepository.AddCountDelete("EMPLEADOS", fichasLaborales);
                fichaLaboralRepository.RemoveRange(fichasLaborales);
                SendTimeOperationToLogger("Delete fichasLaborales");

                // EMPLEADOS
                resultadosIntegracionRepository.AddCountDelete("EMPLEADOS", deleteEmpleados);
                empleadoRepository.RemoveRange(deleteEmpleados);
                await empleadoRoleRepository.SaveChangesAsync().ConfigureAwait(false);
                SendTimeOperationToLogger("Delete Empleados Entities");
            }

            private async Task BulkUpdateNewEmployees(IEnumerable<UsuarioWorkDay> importUsuariosWorkday, IEnumerable<Empleado> importEmpleados)
            {
                List<long> IdsUpdateUsuariosWorkDay = importUsuariosWorkday
                    .Where(u => u.ImportAction == OpcionesIntegracion.IntegracionUpdateAction)
                    .Select(u => u.IdWorkDay).ToList();

                if (!IdsUpdateUsuariosWorkDay.Any())
                    return;

                var updateUsuariosWorkday = await repository.GetBy(u => IdsUpdateUsuariosWorkDay.Contains(u.IdWorkDay))
                    .ToListAsync().ConfigureAwait(false);

                //Actualizamos los datos de la entidad existente
                updateUsuariosWorkday.ForEach(uu =>
                {
                    var userWDTemp = importUsuariosWorkday.First(u => u.IdWorkDay == uu.IdWorkDay);
                    uu.IdWorkDay = userWDTemp.IdWorkDay;
                    uu.Nombre = userWDTemp.Nombre;
                    uu.NumEmpleado = userWDTemp.NumEmpleado;
                    uu.Genero = userWDTemp.Genero;
                    uu.UltimaModif = userWDTemp.UltimaModif;
                    uu.Nif = userWDTemp.Nif;
                    uu.Mail = userWDTemp.Mail;
                    uu.Telefono = userWDTemp.Telefono;
                    uu.Apellido1 = userWDTemp.Apellido1;
                    uu.Apellido2 = userWDTemp.Apellido2;
                    uu.Departamento = userWDTemp.Departamento;
                    uu.Division = userWDTemp.Division;
                    uu.EsServicioMedico = userWDTemp.EsServicioMedico;
                    uu.FechaNacimiento = userWDTemp.FechaNacimiento;
                    uu.IdResponsable = userWDTemp.IdResponsable;
                    uu.Localizacion = userWDTemp.Localizacion;
                    uu.MailCorporativo = userWDTemp.MailCorporativo;
                    uu.TelefonoCorporativo = userWDTemp.TelefonoCorporativo;
                    uu.Upn = userWDTemp.Upn;
                    uu.InterAcciona = userWDTemp.InterAcciona;
                });

                //Traceamos los marcados como actualización que no existen en BBDD
                IdsUpdateUsuariosWorkDay.Except(updateUsuariosWorkday.Select(u => u.IdWorkDay)).ForEach(idError =>
                {
                    resultadosIntegracionRepository.AddError("EMPLEADOS",
                        $"El UsuarioWorkday con idWorkday {idError} estaba marcado como actualización, pero no existe en la tabla de destino.");
                });

                // WORKDAY
                resultadosIntegracionRepository.AddCountUpdate("EMPLEADOS", updateUsuariosWorkday);
                repository.UpdateRange(updateUsuariosWorkday);

                SendTimeOperationToLogger("Update Workday Entities");

                var updateEmpleados = await empleadoRepository
                    .GetBy(e => IdsUpdateUsuariosWorkDay.Contains(e.IdUsuarioWorkDayNavigation.IdWorkDay))
                    .Include(e => e.IdUsuarioWorkDayNavigation)
                    .ToListAsync().ConfigureAwait(false);

                //Actualizamos los datos de la entidad
                updateEmpleados.ForEach(ue =>
                {
                    var empleadoTemp = importEmpleados
                        .First(e => e.IdUsuarioWorkDayNavigation.IdWorkDay == ue.IdUsuarioWorkDayNavigation.IdWorkDay);
                    ue.Nombre = empleadoTemp.Nombre;
                    ue.NumEmpleado = empleadoTemp.NumEmpleado;
                    ue.Genero = empleadoTemp.Genero;
                    ue.UltimaModif = empleadoTemp.UltimaModif;
                    ue.Apellido = empleadoTemp.Apellido;
                    ue.Mail = empleadoTemp.Mail;
                    ue.Nif = empleadoTemp.Nif;
                    ue.Telefono = empleadoTemp.Telefono;
                    ue.Bloqueado = empleadoTemp.Bloqueado;
                    ue.FechaNacimiento = empleadoTemp.FechaNacimiento;
                    ue.Upn = empleadoTemp.Upn;
                    ue.EsServicioMedico = empleadoTemp.EsServicioMedico;
                    ue.InterAcciona = empleadoTemp.InterAcciona;
                });

                //Traceamos los marcados como actualización que no existen en BBDD
                IdsUpdateUsuariosWorkDay.Except(updateEmpleados.Select(e => Convert.ToInt64(e.IdUsuarioWorkDayNavigation.IdWorkDay))).ForEach(idError =>
                {
                    resultadosIntegracionRepository.AddError("EMPLEADOS",
                        $"El empleado con IdUsuarioWorkDayNavigation.IdWorkDay {idError} estaba marcado como actualización, pero no existe en la tabla de destino.");
                });

                // EMPLEADOS
                resultadosIntegracionRepository.AddCountUpdate("EMPLEADOS", updateEmpleados);
                empleadoRepository.UpdateRange(updateEmpleados);
                await empleadoRoleRepository.SaveChangesAsync().ConfigureAwait(false);
                SendTimeOperationToLogger("Update Empleados Entities");
            }

            private async Task BulkInsertNewEmployees(IEnumerable<UsuarioWorkDay> importUsuariosWorkday, IEnumerable<Empleado> importEmpleados)
            {
                List<long> IdsNewUsuariosWorkDay = importUsuariosWorkday
                    .Where(u => u.ImportAction == OpcionesIntegracion.IntegracionCreateAction)
                    .Select(u => u.IdWorkDay).ToList();

                if (!IdsNewUsuariosWorkDay.Any())
                    return;

                var newUsuariosWorkday = importUsuariosWorkday.Where(u => IdsNewUsuariosWorkDay.Contains(u.IdWorkDay)).ToList();
                var newEmpleados = importEmpleados.Where(e => IdsNewUsuariosWorkDay.Contains(e.IdUsuarioWorkDayNavigation.IdWorkDay)).ToList();

                List<long> idsWorkdayToInsert = newUsuariosWorkday.Select(u => u.IdWorkDay).ToList();
                List<long> idsAlreadyExists = repository.GetBy(u => idsWorkdayToInsert.Contains(u.IdWorkDay)).Select(u => u.IdWorkDay).ToList();

                //Excluimos los identificadores de usuarios workday que ya existen en la BBDD
                newUsuariosWorkday = newUsuariosWorkday.FindAll(u => !idsAlreadyExists.Contains(u.IdWorkDay));
                //Si ya existía el usuario workday, afecta al resto del proceso de inserciones.
                newEmpleados = newEmpleados.FindAll(u => !idsAlreadyExists.Contains(u.IdUsuarioWorkDayNavigation.IdWorkDay));

                idsAlreadyExists.ForEach(id =>
                {
                    resultadosIntegracionRepository.AddError("EMPLEADOS",
                        $"No ha podido ser insertado el registro con idWorkday ({id}) marcado como CREATE por existir ya en la BBDD.");
                });

                // WORKDAY
                resultadosIntegracionRepository.AddCountInsert("EMPLEADOS", newUsuariosWorkday);
                repository.AddRange(newUsuariosWorkday);

                SendTimeOperationToLogger("Bulk insert Workday Entities");

                // FICHA TECNICA
                List<FichaMedica> listaFichMedica = newEmpleados.Select(c => c.IdFichaMedicaNavigation).ToList();
                resultadosIntegracionRepository.AddCountInsert("EMPLEADOS", listaFichMedica);
                fichaMedicaRepository.AddRange(listaFichMedica);

                SendTimeOperationToLogger("Bulk insert FichaMedica Entities");

                // FICHA LABORAL
                List<FichaLaboral> listaFichLaboral = newEmpleados.Select(c => c.IdFichaLaboralNavigation).ToList();
                resultadosIntegracionRepository.AddCountInsert("EMPLEADOS", listaFichLaboral);
                fichaLaboralRepository.AddRange(listaFichLaboral);

                SendTimeOperationToLogger("Bulk insert Ficha Laboral Entities");

                // EMPLEADOS
                resultadosIntegracionRepository.AddCountInsert("EMPLEADOS", newEmpleados);
                empleadoRepository.AddRange(newEmpleados);
                SendTimeOperationToLogger("Bulk insert Empleados Entities");

                // ACTUALIZACION RESPONSABLES
                listaFichLaboral = await UpdateResponsableFichaLaboral(listaFichLaboral).ConfigureAwait(false);
                resultadosIntegracionRepository.AddCountInsert("EMPLEADOS", listaFichLaboral);
                fichaLaboralRepository.AddRange(listaFichLaboral);

                // BULK ROLE EMPLEADO

                List<EmpleadoRole> listaEmpleadoRole = new List<EmpleadoRole>();

                Role roleEmpleado = await roleRepository.GetAll().FirstOrDefaultAsync(c => c.Nombre == "Empleado").ConfigureAwait(false);

                newEmpleados = newEmpleados.OrderBy(c => c.Id).ToList();

                newEmpleados.ForEach(c => listaEmpleadoRole.Add(new EmpleadoRole()
                {
                    IdEmpleadoNavigation = c,
                    IdRole = roleEmpleado.Id,
                }));
                resultadosIntegracionRepository.AddCountInsert("EMPLEADOS", listaEmpleadoRole);
                empleadoRoleRepository.AddRange(listaEmpleadoRole);
                await empleadoRoleRepository.SaveChangesAsync().ConfigureAwait(false);
                SendTimeOperationToLogger("Bulk insert Roles");
            }

            private async Task ImportDepartments((string[], string[][]) csvDepartments)
            {
                string[] headers = csvDepartments.Item1;
                string[][] data = csvDepartments.Item2;
                if (!data.Any())
                    return;

                Departamento.SetPropertyIndexes(headers);

                List<Departamento> importedDepartamentos = new List<Departamento>();

                resultadosIntegracionRepository.AddMessage("DEPARTAMENTOS", $"Se han encontrado {data.Length - 1} filas para importar.");
                foreach (string[] dataRow in data)
                {
                    Departamento newDepartamento = new Departamento(dataRow);
                    importedDepartamentos.Add(newDepartamento);
                }

                var newDepartamentos = importedDepartamentos.Where(d => d.ImportAction?.ToUpperInvariant() == OpcionesIntegracion.IntegracionCreateAction).ToList();

                List<long> idsNewDepartments = newDepartamentos.Select(d => d.IdWorkday).ToList();
                List<long> idsDepartmentAlreadyExists = await departamentoRepository.GetBy(d => idsNewDepartments.Contains(d.IdWorkday)).Select(d => d.IdWorkday)
                    .ToListAsync().ConfigureAwait(false);

                idsDepartmentAlreadyExists.ForEach(id =>
                {
                    resultadosIntegracionRepository.AddError("DEPARTAMENTOS",
                        $"No ha podido ser insertado el registro con idWorkday ({id}) marcado como CREATE por existir ya en la BBDD.");
                });

                newDepartamentos = newDepartamentos.FindAll(d => !idsDepartmentAlreadyExists.Contains(d.IdWorkday));
                resultadosIntegracionRepository.AddCountInsert("DEPARTAMENTOS", newDepartamentos);
                departamentoRepository.AddRange(newDepartamentos);

                //Buscamos los registros marcados como UPDATE
                var updateDepartamentos = importedDepartamentos.Where(d => d.ImportAction?.ToUpperInvariant() == OpcionesIntegracion.IntegracionUpdateAction).ToList();
                if (updateDepartamentos.Any())
                {
                    List<long> IdWorkdayList = updateDepartamentos.Select(d => d.IdWorkday).ToList();
                    var oldDepartamentos = await departamentoRepository.GetBy(d => IdWorkdayList.Contains(d.IdWorkday)).ToListAsync().ConfigureAwait(false);
                    oldDepartamentos.ForEach(od =>
                    {
                        var depTemp = updateDepartamentos.FirstOrDefault(l => l.IdWorkday == od.IdWorkday);
                        od.Nombre = depTemp.Nombre;
                    });

                    //Revisamos si ya existían anteriormente, si no, traceamos el error
                    var errorlocations = IdWorkdayList.Where(di => !oldDepartamentos.Any(d => d.IdWorkday == di)).ToList();
                    errorlocations.ForEach(di =>
                        resultadosIntegracionRepository.AddError("DEPARTAMENTOS",
                            $"El departamento con id {di} estaba marcada como actualización, pero no existe en el maestro de departamentos."));

                    //Actualizamos
                    resultadosIntegracionRepository.AddCountUpdate("DEPARTAMENTOS", oldDepartamentos);
                    departamentoRepository.UpdateRange(oldDepartamentos);
                }
            }

            private async Task ImportLocalizations((string[], string[][]) csvLocalizations)
            {
                string[] headers = csvLocalizations.Item1;
                string[][] data = csvLocalizations.Item2;
                if (!data.Any())
                    return;

                Localizacion.SetPropertyIndexes(headers);
                Area.SetPropertyIndexes(headers);
                Region.SetPropertyIndexes(headers);
                Pais.SetPropertyIndexes(headers);

                List<Area> existentAreas = (await areaRepository.GetAll()
                    .Select(a => new {
                        a.Region.IdPais,
                        Area = a
                    }).ToListAsync().ConfigureAwait(false))
                    .Select(x => {
                        var aNew = x.Area;
                        aNew.IdPais = x.IdPais;
                        return aNew;
                    }).ToList();
                List<Region> existentRegions = await regionRepository.GetAll().ToListAsync().ConfigureAwait(false);
                List<Pais> existentPaises = await paisRepository.GetAll().ToListAsync().ConfigureAwait(false);

                List<Localizacion> importedLocalizaciones = new List<Localizacion>();
                List<Area> newAreas = new List<Area>();
                List<Region> newRegions = new List<Region>();
                List<Pais> newPaises = new List<Pais>();

                resultadosIntegracionRepository.AddMessage("LOCALIZACIONES", $"Se han encontrado {data.Length - 1} filas para importar.");
                //Preparamos los datos importados
                foreach (string[] dataRow in data)
                {
                    Localizacion newLocalizacion = new Localizacion(dataRow);
                    Area newArea = new Area(dataRow);
                    Region newRegion = new Region(dataRow);
                    Pais newPais = new Pais(dataRow);

                    if (!string.IsNullOrEmpty(newPais.Nombre))
                    {
                        if (existentPaises.Any(p => p.Nombre == newPais.Nombre))
                            newPais = existentPaises.Single(p => p.Nombre == newPais.Nombre);
                        else if (newPaises.Any(p => p.Nombre == newPais.Nombre))
                            newPais = newPaises.Single(p => p.Nombre == newPais.Nombre);
                        else
                            newPaises.Add(newPais);

                        if (!string.IsNullOrEmpty(newRegion.Nombre))
                        {
                            if (existentRegions.Any(p => p.Nombre == newRegion.Nombre))
                                newRegion = existentRegions.Single(p => p.Nombre == newRegion.Nombre);
                            else if (newRegions.Any(p => p.Nombre == newRegion.Nombre))
                                newRegion = newRegions.Single(p => p.Nombre == newRegion.Nombre);
                            else
                            {
                                newRegion.Pais = newPais;
                                newRegions.Add(newRegion);
                            }
                        }
                        else if (!string.IsNullOrEmpty(newArea.Nombre))
                        {
                            if (existentRegions.Any(p => p.Nombre == newArea.Nombre))
                                newRegion = existentRegions.Single(p => p.Nombre == newArea.Nombre);
                            else if (newRegions.Any(p => p.Nombre == newArea.Nombre))
                                newRegion = newRegions.Single(p => p.Nombre == newArea.Nombre);
                            else
                            {
                                newRegion.Nombre = newArea.Nombre;
                                newRegion.Pais = newPais;
                                newRegions.Add(newRegion);
                            }
                        }

                        if (!string.IsNullOrEmpty(newArea.Nombre))
                        {
                            if (existentAreas.Any(p => p.Nombre == newArea.Nombre && p.IdPais == newPais.Id))
                                newArea = existentAreas.Single(p => p.Nombre == newArea.Nombre && p.IdPais == newPais.Id);
                            else if (newAreas.Any(p => p.Nombre == newArea.Nombre && p.IdPais == newPais.Id))
                                newArea = newAreas.Single(p => p.Nombre == newArea.Nombre && p.IdPais == newPais.Id);
                            else
                            {
                                newArea.Region = newRegion;
                                newArea.IdPais = newPais.Id;
                                newAreas.Add(newArea);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(newArea.Nombre))
                        newLocalizacion.Area = newArea;

                    importedLocalizaciones.Add(newLocalizacion);
                }

                var newLocalizaciones = importedLocalizaciones.Where(l => l.ImportAction?.ToUpperInvariant() == OpcionesIntegracion.IntegracionCreateAction).ToList();

                List<string> idsNewLocation = newLocalizaciones.Select(d => d.Nombre).ToList();

                //Comprobamos si los nuevos ya existen para tracear el error
                List<string> idsLocationAlreadyExists = await localizacionRepository.GetBy(d => idsNewLocation.Contains(d.Nombre)).Select(d => d.Nombre)
                    .ToListAsync().ConfigureAwait(false);
                idsLocationAlreadyExists.ForEach(id =>
                    resultadosIntegracionRepository.AddError("LOCALIZACIONES",
                        $"No ha podido ser insertado el registro con nombre ({id}) marcado como CREATE por existir ya en la BBDD."));

                //Insertamos
                resultadosIntegracionRepository.AddCountInsert("LOCALIZACIONES", newPaises);
                paisRepository.AddRange(newPaises);

                resultadosIntegracionRepository.AddCountInsert("LOCALIZACIONES", newRegions);
                regionRepository.AddRange(newRegions);

                resultadosIntegracionRepository.AddCountInsert("LOCALIZACIONES", newAreas);
                areaRepository.AddRange(newAreas);

                newLocalizaciones = newLocalizaciones.FindAll(d => !idsLocationAlreadyExists.Contains(d.Nombre));
                resultadosIntegracionRepository.AddCountInsert("LOCALIZACIONES", newLocalizaciones);
                localizacionRepository.AddRange(newLocalizaciones);

                //Buscamos los marcados como actualización
                var updateLocalizaciones = importedLocalizaciones.Where(l => l.ImportAction?.ToUpperInvariant() == OpcionesIntegracion.IntegracionUpdateAction).ToList();
                if (updateLocalizaciones.Any())
                {
                    List<string> locationNames = updateLocalizaciones.Select(l => l.Nombre).ToList();
                    var oldLocalizaciones = await localizacionRepository.GetBy(l => locationNames.Contains(l.Nombre)).ToListAsync().ConfigureAwait(false);
                    oldLocalizaciones.ForEach(ol =>
                    {
                        var locTemp = updateLocalizaciones.FirstOrDefault(l => l.Nombre == ol.Nombre);
                        ol.Ciudad = locTemp.Ciudad;
                        ol.CodigoPostal = locTemp.CodigoPostal;
                        ol.Direccion1 = locTemp.Direccion1;
                        ol.Pais = locTemp.Pais;
                    });

                    //Traceamos los marcados como UPDATE, pero que no existen
                    var errorlocations = locationNames.Where(ln => !oldLocalizaciones.Any(l => l.Nombre == ln)).ToList();
                    errorlocations.ForEach(l => resultadosIntegracionRepository.AddError("LOCALIZACIONES",
                        $"La localizacion {l} estaba marcada como actualización, pero no existe en el maestro de localizaciones."));

                    //Actualizamos
                    resultadosIntegracionRepository.AddCountUpdate("LOCALIZACIONES", oldLocalizaciones);
                    localizacionRepository.UpdateRange(oldLocalizaciones);
                }
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
