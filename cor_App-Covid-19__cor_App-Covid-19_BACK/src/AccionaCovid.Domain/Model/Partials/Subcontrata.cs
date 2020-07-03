using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase Subcontrata
    /// </summary>
    public partial class Subcontrata
    {
        /// <summary>
        /// Índice del cif en el CSV
        /// </summary>
        private static int cifIndex;

        /// <summary>
        /// Índice de la empresa en el CSV
        /// </summary>
        private static int empresaIndex;

        /// <summary>
        /// Índice de la acción de importación a realizar
        /// </summary>
        private static int importActionIndex;

        /// <summary>
        /// Ación a realizar al importar el dato
        /// </summary>
        [NotMapped]
        public string ImportAction { get; set; }

        /// <summary>
        /// Define los índices de las propiedades a partir de la cabecera del CSV
        /// </summary>
        /// <param name="headers">Cabecera del CSV</param>
        public static void SetPropertyIndexes(string[] headers)
        {
            cifIndex = Array.IndexOf(headers, "cif");
            empresaIndex = Array.IndexOf(headers,"empresa");
            importActionIndex = Array.IndexOf(headers, OpcionesIntegracion.IntegracionHeaderField);
        }

        /// <summary>
        /// Crea una instancia en función de la fila de datos del CSV
        /// </summary>
        /// <param name="data"></param>
        public Subcontrata(string[] data)
        {
            this.Cif = Subcontrata.cifIndex >= 0 ? data[Subcontrata.cifIndex] : null;
            this.Nombre  = Subcontrata.empresaIndex >= 0 ? data[Subcontrata.empresaIndex] : null;
            this.ImportAction = Subcontrata.importActionIndex >= 0 ? data[Subcontrata.importActionIndex] : null;
        }
    }
}
