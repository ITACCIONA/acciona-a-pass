using AccionaCovid.Crosscutting;
using System.Collections.Generic;

namespace AccionaCovid.WebApi
{
    /// <summary>
    /// Información de usuario para los HostedServices
    /// </summary>
    public class UserInfoAccesorHostedServices : IUserInfoAccesor
    {
        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public int IdUser { get => 1; set => throw new System.NotImplementedException(); }

        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
        public string UserFullName { get => "Hosted Service"; set => throw new System.NotImplementedException(); }

        /// <summary>
        /// Nombre de los roles de usuario
        /// </summary>
        public string[] Roles { get; set; }
    }
}