using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase localizacion
    /// </summary>
    public partial class Pais
    {
        /// <summary>
        /// Índice del nombre en el CSV
        /// </summary>
        private static int nombreIndex;

        /// <summary>
        /// Define los índices de las propiedades a partir de la cabecera del CSV
        /// </summary>
        /// <param name="headers">Cabecera del CSV</param>
        public static void SetPropertyIndexes(string[] headers)
        {
            nombreIndex = Array.IndexOf(headers, "Country");
        }

        /// <summary>
        /// Crea una instancia en función de la fila de datos del CSV
        /// </summary>
        /// <param name="data"></param>
        public Pais(string[] data)
        {
            this.Nombre = Pais.nombreIndex >= 0 ? data[Pais.nombreIndex] : null;
        }
    }
}
