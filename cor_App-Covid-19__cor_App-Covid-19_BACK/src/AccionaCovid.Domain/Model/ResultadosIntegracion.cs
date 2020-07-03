using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.Domain.Model
{
    public class ResultadosIntegracion : Entity<ResultadosIntegracion>
    {
        /// <summary>
        /// Fecha del resultado
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Bloque o Fichero origen
        /// </summary>
        public string Bloque { get; set; }

        /// <summary>
        /// Mensaje
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Nombre del fichero del que parte la integración
        /// </summary>
        public string OriginFileName { get; set; }

        /// <summary>
        /// Indica si el mensaje es origen de un error
        /// </summary>
        public bool EsError { get; set; }
    }
}
