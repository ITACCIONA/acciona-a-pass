using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase partial
    /// </summary>
    public partial class ParametroMedico
    {
        /// <summary>
        /// Enumerado que indica los parametros disponibles
        /// </summary>
        public enum ParameterTypes
        {
            TemperaturaAlta = 1,
            IgG,
            IgM
        }
    }
}
