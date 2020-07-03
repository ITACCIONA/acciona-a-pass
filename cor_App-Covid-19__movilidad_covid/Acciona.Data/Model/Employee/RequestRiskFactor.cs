using System;
using System.Collections.Generic;

namespace Acciona.Data.Model.Employee
{
    public class RequestRiskFactor
    {
        public int idEmployee { get; set; }
        public string fechaFactor { get; set; }
        public IEnumerable<RequestRiskFactorValueData> riskFactorValues { get; set; }
    }
}
