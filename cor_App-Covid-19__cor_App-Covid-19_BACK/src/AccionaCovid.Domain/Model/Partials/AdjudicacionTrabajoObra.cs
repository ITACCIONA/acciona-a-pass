using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase AdjudicacionTrabajoObra
    /// </summary>
    public partial class AdjudicacionTrabajoObra
    {
        /// <summary>
        /// Índice del código de la obra en el CSV
        /// </summary>
        private static int codigoObraIndex;

        /// <summary>
        /// Índice del cif en el CSV
        /// </summary>
        private static int cifIndex;

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
            cifIndex = Array.IndexOf(headers, "cif_empresa");
            codigoObraIndex = Array.IndexOf(headers,"cod_obra");
            importActionIndex = Array.IndexOf(headers, OpcionesIntegracion.IntegracionHeaderField);
        }

        /// <summary>
        /// Crea una instancia en función de la fila de datos del CSV
        /// </summary>
        /// <param name="data"></param>
        public AdjudicacionTrabajoObra(string[] data)
        {
            this.Subcontrata = new Subcontrata();
            this.Obra = new Obra();
            this.Subcontrata.Cif  = AdjudicacionTrabajoObra.cifIndex >= 0 ? data[AdjudicacionTrabajoObra.cifIndex] : null;
            this.Obra.CodigoObra  = AdjudicacionTrabajoObra.codigoObraIndex >= 0 ? data[AdjudicacionTrabajoObra.codigoObraIndex] : null;
            this.ImportAction = AdjudicacionTrabajoObra.importActionIndex >= 0 ? data[AdjudicacionTrabajoObra.importActionIndex] : null;
        }
    }
}
