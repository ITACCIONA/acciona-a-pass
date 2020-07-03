using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase Obra
    /// </summary>
    public partial class Obra
    {
        /// <summary>
        /// Índice del código obra en el CSV
        /// </summary>
        private static int codigoObraIndex;

        /// <summary>
        /// Índice del nombre de la obra en el CSV
        /// </summary>
        private static int nombreIndex;

        /// <summary>
        /// Índice del estado de la obra en el CSV
        /// </summary>
        private static int IdEstadoObraIndex;

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
            codigoObraIndex = Array.IndexOf(headers, "cod_obra");
            nombreIndex = Array.IndexOf(headers, "obra");
            IdEstadoObraIndex = Array.IndexOf(headers, "estado");
            importActionIndex = Array.IndexOf(headers, OpcionesIntegracion.IntegracionHeaderField);
        }

        /// <summary>
        /// Crea una instancia en función de la fila de datos del CSV
        /// </summary>
        /// <param name="data"></param>
        public Obra(string[] data)
        {
            this.CodigoObra = Obra.codigoObraIndex >= 0 ? data[Obra.codigoObraIndex] : null;
            this.Nombre = Obra.nombreIndex >= 0 ? data[Obra.nombreIndex] : null;
            this.EstadoObra = new EstadoObra();
            this.EstadoObra.Nombre = Obra.IdEstadoObraIndex >= 0 ? data[Obra.IdEstadoObraIndex] : null;
            this.ImportAction = Obra.importActionIndex >= 0 ? data[Obra.importActionIndex] : null;
        }
    }
}
