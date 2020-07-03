using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase devision
    /// </summary>
    public partial class Division
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
            nombreIndex = Array.IndexOf(headers, "nombre");
        }

        /// <summary>
        /// Crea una instancia en función de la fila de datos del CSV
        /// </summary>
        /// <param name="data"></param>
        public Division(string[] data)
        {
            this.Nombre = Division.nombreIndex >= 0 ? data[Division.nombreIndex] : null;
        }
    }
}
