using AccionaCovid.Crosscutting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// Recupera datos de usuario desde la infraestructura
    /// </summary>
    public class UserInfoAccesor : IUserInfoAccesor
    {
        /// <summary>
        /// 
        /// </summary>
        public UserInfoAccesor() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="log"></param>
        public UserInfoAccesor(IHttpContextAccessor httpContextAccessor, ILogger<UserInfoAccesor> log)
        {
            if(httpContextAccessor == null)
                throw new ArgumentNullException(nameof(httpContextAccessor));
            
            try
            {
                var user = httpContextAccessor.HttpContext?.User;
                if (user == null)
                    return;

                IdUser = int.Parse(user.FindFirst("IdUser")?.Value ?? "0");
                UserFullName = user.FindFirst("UserFullName")?.Value;
                UserFullName = string.IsNullOrEmpty(UserFullName) ? "UNAUTHENTICATED USER" : UserFullName;
                Roles = user.Claims
                    .Where(c => c.Type == ClaimTypes.Role ||
                                c.Type == "role")
                    .Select(c => c.Value).ToArray();
            }
            catch(Exception)
            {
                log.LogWarning("[UserInfoAccesor] no se han podido cargar las propiedades de usuario a traves de los claims");
            }
        }

        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public int IdUser { get; set; }

        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Nombre de los roles de usuario
        /// </summary>
        public string[] Roles { get; set; }
    }
}
