using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase departamento
    /// </summary>
    public partial class Departamento
    {
        /// <summary>
        /// Índice del identificador en workday en el CSV
        /// </summary>
        private static int idWorkdayIndex;

        /// <summary>
        /// Índice del nombre en el CSV
        /// </summary>
        private static int nombreIndex;

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
            idWorkdayIndex = Array.IndexOf(headers, "Supervisory Code");
            nombreIndex = Array.IndexOf(headers, "Supervisory Name");
            importActionIndex = Array.IndexOf(headers, OpcionesIntegracion.IntegracionHeaderField);
        }

        /// <summary>
        /// Crea una instancia en función de la fila de datos del CSV
        /// </summary>
        /// <param name="data"></param>
        public Departamento(string[] data)
        {
            try { this.IdWorkday = Departamento.idWorkdayIndex >= 0 ? Convert.ToInt64(data[Departamento.idWorkdayIndex]) : 0; } catch (Exception ex) { throw new Exception($"Incorrect format for field {nameof(this.IdWorkday)}", ex); }
            this.Nombre = Departamento.nombreIndex >= 0 ? data[Departamento.nombreIndex] : null;
            this.ImportAction = Departamento.importActionIndex >= 0 ? data[Departamento.importActionIndex] : null;
        }
    }
}
