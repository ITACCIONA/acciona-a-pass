using System;
using System.Collections;
using System.Collections.Generic;

namespace Acciona.Data.Model.Employee
{
    public class AlertsResponse
    {
        public IEnumerable<AlertData> alertsNoRead { get; set; }
        public IEnumerable<AlertData> alertsRead { get; set; }
    }
}
