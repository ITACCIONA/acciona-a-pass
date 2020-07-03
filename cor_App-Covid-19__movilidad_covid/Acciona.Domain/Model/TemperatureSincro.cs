using System;
namespace Acciona.Domain.Model
{
    public class TemperatureSincro
    {
        public int IdEmpleado { get; set; }
        public string IdDevice { get; set; }
        public bool IsTemperatureOver { get; set; }
        public long Date { get; set; }
    }
}
