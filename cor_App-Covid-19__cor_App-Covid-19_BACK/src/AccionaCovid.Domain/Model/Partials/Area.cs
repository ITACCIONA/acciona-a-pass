using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase localizacion
    /// </summary>
    public partial class Area
    {
        /// <summary>
        /// Índice del nombre en el CSV
        /// </summary>
        private static int nombreIndex;

        /// <summary>
        /// Identioficador del pais con fines logicos
        /// </summary>
        [NotMapped]
        public int IdPais { get; set; }

        /// <summary>
        /// Define los índices de las propiedades a partir de la cabecera del CSV
        /// </summary>
        /// <param name="headers">Cabecera del CSV</param>
        public static void SetPropertyIndexes(string[] headers)
        {
            nombreIndex = Array.IndexOf(headers, "Provincia");
        }

        /// <summary>
        /// Crea una instancia en función de la fila de datos del CSV
        /// </summary>
        /// <param name="data"></param>
        public Area(string[] data)
        {
            this.Nombre = Area.nombreIndex >= 0 ? data[Area.nombreIndex] : null;
        }
    }
}
