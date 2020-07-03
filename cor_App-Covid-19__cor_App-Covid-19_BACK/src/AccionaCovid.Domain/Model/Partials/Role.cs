using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase devision
    /// </summary>
    public partial class Role
    {
        public enum RoleNames
        {
            ServicioMedico,
            PRL,
            Empleado,
            Administrator,
            Comunicacion,
            RRHH
        }

        public static string TranslateCsvToDbRoleName(string csvRoleName)
        {
            switch (csvRoleName?.Trim().ToUpper())
            {
                case "PRL":
                    return RoleNames.PRL.ToString();
                case "SERVICIO MEDICO":
                    return RoleNames.ServicioMedico.ToString();
                case "HR":
                    return RoleNames.RRHH.ToString();
                default:
                    return null;
            }
        }
    }
}
