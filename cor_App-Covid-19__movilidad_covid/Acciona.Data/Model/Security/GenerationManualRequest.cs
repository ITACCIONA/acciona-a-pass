using System;
using System.Collections.Generic;

namespace Acciona.Data.Model.Security
{
    public class GenerationManualRequest
    {
        public int idEmployee { get; set; }        
        public bool isGreenPaper { get; set; }        
        public string registrationDateTime { get; set; }
    }
}