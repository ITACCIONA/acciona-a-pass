using System;
namespace Acciona.Domain.Model.Security
{
    public class UserPaper
    {
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string ApellidosEmpleado { get; set; }
        public string InicialesEmpleado { get; set; }
        public string DNI { get; set; }
        public int EdadEmpleado { get; set; }
        public long NumEmpleado { get; set; }
        public string TelefonoEmpleado { get; set; }
        public string MailEmpleado { get; set; }
    }
}
