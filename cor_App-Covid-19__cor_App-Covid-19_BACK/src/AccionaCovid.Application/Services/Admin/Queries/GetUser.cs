using AccionaCovid.Application.Core;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Admin
{
    /// <summary>
    /// Servicio que obtiene un usuario
    /// </summary>
    public class GetUser
    {
        /// <summary>
        /// </summary>
        public class GetUserRequest : IRequest<GetUserResponse>
        {
        }

        /// <summary>
        /// Informacion de un pais
        /// </summary>
        public class GetUserResponse
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public GetUserResponse()
            {
                RoleName = new List<string>();
            }

            /// <summary>
            /// Id de empleado para poder hacer consultas posteriores
            /// </summary>
            public int IdEmpleado { get; set; }

            /// <summary>
            /// Nombre de usuario completo para mostrar en el menú
            /// </summary>
            public string NombreUsuario { get; set; }

            /// <summary>
            /// Iniciales del usuario
            /// </summary>
            public string InicialesUsuario { get; set; }

            /// <summary>
            /// Role del usuario
            /// </summary>
            public List<string> RoleName { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetUserCommandHandler : BaseCommandHandler<GetUserRequest, GetUserResponse>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetUserCommandHandler(IRepository<Empleado> repository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<GetUserResponse> Handle(GetUserRequest request, CancellationToken cancellationToken)
            {
                Empleado firstEmpleado = await repository.GetAll()
                    .Include(c => c.EmpleadoRole)
                        .ThenInclude(c => c.IdRoleNavigation)
                    .FirstOrDefaultAsync(c => c.Id == IdUser).ConfigureAwait(false);

                if(firstEmpleado == null)
                {
                    return new GetUserResponse();
                }

                return new GetUserResponse()
                {
                    IdEmpleado = firstEmpleado.Id,
                    InicialesUsuario = firstEmpleado.Iniciales,
                    NombreUsuario = firstEmpleado.NombreCompleto,
                    RoleName = firstEmpleado.EmpleadoRole.Select(c => c.IdRoleNavigation.Nombre).ToList()
                };
            }
        }
    }
}
