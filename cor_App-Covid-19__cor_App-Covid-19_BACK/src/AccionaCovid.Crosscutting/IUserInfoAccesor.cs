namespace AccionaCovid.Crosscutting
{
    /// <summary>
    /// Interface de datos del usuario de la peticion
    /// </summary>
    public interface IUserInfoAccesor
    {
        /// <summary>
        /// Identificador del usuario
        /// </summary>
        int IdUser { get; set; }

        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
        string UserFullName { get; set; }

        ///// <summary>
        ///// Identificador de Objeto del usuario
        ///// </summary>
        //string UserObjectId { get; set; }

        /// <summary>
        /// Nombre de los roles de usuario
        /// </summary>
        string[] Roles { get; set; }
    }
}
