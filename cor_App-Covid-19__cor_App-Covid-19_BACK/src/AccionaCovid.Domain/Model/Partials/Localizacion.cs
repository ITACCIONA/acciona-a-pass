using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase localizacion
    /// </summary>
    public partial class Localizacion
    {
        /// <summary>
        /// Índice del nombre en el CSV
        /// </summary>
        private static int nombreIndex;

        /// <summary>
        /// Índice del pís en el CSV
        /// </summary>
        private static int paisIndex;

        /// <summary>
        /// Índice de la direccion1 en el CSV
        /// </summary>
        private static int direccion1Index;

        /// <summary>
        /// Índice del ciudad en el CSV
        /// </summary>
        private static int ciudadIndex;

        /// <summary>
        /// Índice del código postal en el CSV
        /// </summary>
        private static int codigoPostalIndex;

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
            nombreIndex = Array.IndexOf(headers, "Location");
            paisIndex = Array.IndexOf(headers, "Country");
            direccion1Index = Array.IndexOf(headers, "AddressLineData"); 
            ciudadIndex = Array.IndexOf(headers, "Country_Region_Description");
            codigoPostalIndex = Array.IndexOf(headers, "Postal_Code");
            importActionIndex = Array.IndexOf(headers, OpcionesIntegracion.IntegracionHeaderField);
        }

        /// <summary>
        /// Crea una instancia en función de la fila de datos del CSV
        /// </summary>
        /// <param name="data"></param>
        public Localizacion(string[] data)
        {
            this.Nombre = Localizacion.nombreIndex >= 0 ? data[Localizacion.nombreIndex] : null;
            this.Pais = Localizacion.paisIndex >= 0 ? data[Localizacion.paisIndex] : null;
            this.Direccion1 = Localizacion.direccion1Index >= 0 ? data[Localizacion.direccion1Index] : null;
            this.Ciudad = Localizacion.ciudadIndex >= 0 ? data[Localizacion.ciudadIndex] : null;
            this.CodigoPostal = Localizacion.codigoPostalIndex >= 0 ? data[Localizacion.codigoPostalIndex] : null;
            this.ImportAction = Localizacion.importActionIndex >= 0 ? data[Localizacion.importActionIndex] : null;
        }
    }
}
