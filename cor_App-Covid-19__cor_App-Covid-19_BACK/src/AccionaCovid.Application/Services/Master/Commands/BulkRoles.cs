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
    /// Servicio de volcado de roles
    /// </summary>
    public class BulkRoles
    {
        /// <summary>
        /// Petición del volcado de roles
        /// </summary>
        public class BulkRolesRequest : IRequest<bool>
        {
            /// <summary>
            /// Fichero CSV con roles
            /// </summary>
            public IFormFile RolesFile { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class BulkRolesCommandHandler : BaseCommandHandler<BulkRolesRequest, bool>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Role> repositoryRole;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<EmpleadoRole> repositoryEmpleadoRole;

            /// <summary>
            /// Constructor
            /// </summary>
            public BulkRolesCommandHandler(IRepository<Empleado> repositoryEmpleado, IRepository<Role> repositoryRole, IRepository<EmpleadoRole> repositoryEmpleadoRole)
            {
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado));
                this.repositoryRole = repositoryRole ?? throw new ArgumentNullException(nameof(repositoryRole));
                this.repositoryEmpleadoRole = repositoryEmpleadoRole ?? throw new ArgumentNullException(nameof(repositoryEmpleadoRole));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(BulkRolesRequest request, CancellationToken cancellationToken)
            {
                (string[], string[][]) csv = await BulkUtils.ReadCsvAsync(request.RolesFile, cancellationToken).ConfigureAwait(false);

                string[] headers = csv.Item1;
                // Identificar los índices donde están los datos de IdUsuarioWorkday y Rol
                int idUsuarioWorkDayIndex = Array.IndexOf(headers, "IDWD");
                int roleIndex = Array.IndexOf(headers, "ROL");


                string[][] data = csv.Item2;

                // Si no hay datos salida
                if (!data.Any())
                    return false;

                // eliminar filas sin IdUsuarioWorkday
                data = data.Where(a => !string.IsNullOrWhiteSpace(a[idUsuarioWorkDayIndex])).ToArray();

                // eliminar duplicados
                data = data.GroupBy(g => g[idUsuarioWorkDayIndex])
                    .Select(g => g.Last())
                    .ToArray();

                // Si no hay datos salida
                if (!data.Any())
                    return false;

                // traerse todos los empleados
                long[] idUsuariosWorkday = data.Select(a => long.Parse(a[idUsuarioWorkDayIndex])).ToArray();

                var empleadosDictionary = await repositoryEmpleado.GetAll()
                        .Include(e => e.IdUsuarioWorkDayNavigation)
                        .Include(e => e.EmpleadoRole)
                            .ThenInclude(er => er.IdRoleNavigation)
                        .Where(e => e.IdUsuarioWorkDay != null && idUsuariosWorkday.Contains(e.IdUsuarioWorkDayNavigation.IdWorkDay))
                        .ToDictionaryAsync(x => x.IdUsuarioWorkDayNavigation.IdWorkDay, x => x)
                        .ConfigureAwait(false);

                // traerse todos los roles del CSV
                var roles = data.Select(a => a[roleIndex]).Distinct().ToArray();

                // traducir roles del CSV
                var rolesTranslations = roles.Select(s => new { s, dbName = Role.TranslateCsvToDbRoleName(s) }).ToDictionary(x => x.s, x => x.dbName);

                // excluir nulos y no reconocidos
                var rolesDbName = rolesTranslations.Values.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

                // diccionario de roles  
                var rolesDictionary = await repositoryRole.GetAll()
                    .Where(r => rolesDbName.Contains(r.Nombre))
                    .ToDictionaryAsync(r => r.Nombre, r => r)
                    .ConfigureAwait(false);



                foreach (string[] dataRow in data)
                {
                    Empleado emp = empleadosDictionary[long.Parse(dataRow[idUsuarioWorkDayIndex])];
                    string newRol = dataRow[roleIndex];
                    string newRolDbName = rolesTranslations[newRol];

                    // Roles del empleado exceptuando Empleado y Administrator
                    var currentAppRoles = emp.EmpleadoRole
                        .Where(x => x.IdRoleNavigation.Nombre != Role.RoleNames.Empleado.ToString() && x.IdRoleNavigation.Nombre != Role.RoleNames.Administrator.ToString())
                        .ToArray();

                    // borrado
                    if (newRolDbName == null)
                    {
                        for (int i = currentAppRoles.Length - 1; i >= 0; i--)
                        {
                            repositoryEmpleadoRole.DeletedRange(new EmpleadoRole[] { currentAppRoles[i] });

                        }
                        continue;
                    }

                    // modificación // nuevo
                    if (!currentAppRoles.Select(r => r.IdRoleNavigation.Nombre).Contains(newRolDbName))
                    {
                        for (int i = currentAppRoles.Length - 1; i >= 0; i--)
                        {
                            repositoryEmpleadoRole.DeletedRange(new EmpleadoRole[] { currentAppRoles[i] });
                        }
                        repositoryEmpleadoRole.Add(new EmpleadoRole
                        {
                            IdEmpleado = emp.Id,
                            IdRole = rolesDictionary[newRolDbName].Id
                        }); ;
                    }

                }
                await repositoryEmpleadoRole.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }
        }
    }
}
