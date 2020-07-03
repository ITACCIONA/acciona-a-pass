using System;
namespace Acciona.Domain.Model
{
    public class QRInfo
    {        
        public int IdEmpleado { get; set; }        
        public string PasaporteColor { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public bool Manual { get; set; }
    }
}
