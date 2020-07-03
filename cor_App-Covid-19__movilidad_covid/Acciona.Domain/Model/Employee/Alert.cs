using System;
namespace Acciona.Domain.Model.Employee
{
    public class Alert
    {
        public int IdAlert { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public DateTime FechaNotificacion { get; set; }
        public bool Read { get; set; }
    }
}
