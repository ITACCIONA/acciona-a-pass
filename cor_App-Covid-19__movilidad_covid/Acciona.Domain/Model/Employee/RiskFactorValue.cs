using System;
namespace Acciona.Domain.Model.Employee
{
    public class RiskFactorValue
    {
        public DateTime FechaFactor { get; set; }
        public int IdRiskFactor { get; set; }
        public string Name { get; set; }
        public bool? Value { get; set; }
    }
}
