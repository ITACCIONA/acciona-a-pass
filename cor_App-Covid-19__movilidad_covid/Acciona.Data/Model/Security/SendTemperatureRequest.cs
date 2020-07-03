using System;
using System.Collections.Generic;

namespace Acciona.Data.Model.Security
{
    public class SendTemperatureRequest
    {
        public int idEmployee { get; set; }
        public string idDevice { get; set; }        
        public bool isTemperatureOverThreshold { get; set; }
        public string meditionDateTime { get; set; }
    }
}