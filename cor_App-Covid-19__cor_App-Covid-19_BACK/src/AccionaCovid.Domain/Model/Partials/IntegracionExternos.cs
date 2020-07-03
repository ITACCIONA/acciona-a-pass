using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase IntegracionExternos
    /// </summary>
    public partial class IntegracionExternos
    {
        /// <summary>
        /// Índice del nif en el CSV
        /// </summary>
        private static int nifIndex;

        /// <summary>
        /// Índice del nombre en el CSV
        /// </summary>
        private static int nombreIndex;

        /// <summary>
        /// Índice del apellido1 en el CSV
        /// </summary>
        private static int apellido1Index;

        /// <summary>
        /// Índice del apellido2 en el CSV
        /// </summary>
        private static int apellido2Index;

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
            nombreIndex = Array.IndexOf(headers, "nombre");
            apellido1Index = Array.IndexOf(headers, "apellido1");
            apellido2Index = Array.IndexOf(headers, "apellido2");
            nifIndex = Array.IndexOf(headers, "nif");
            importActionIndex = Array.IndexOf(headers, OpcionesIntegracion.IntegracionHeaderField);
        }

        /// <summary>
        /// Crea una instancia en función de la fila de datos del CSV
        /// </summary>
        /// <param name="data"></param>
        public IntegracionExternos(string[] data)
        {
            this.ImportAction = IntegracionExternos.importActionIndex >= 0 ? data[IntegracionExternos.importActionIndex] : null;

            if (this.ImportAction != OpcionesIntegracion.IntegracionDeleteAction)
            {
                if (IntegracionExternos.nombreIndex >= 0)
                {
                    if (string.IsNullOrWhiteSpace(data[IntegracionExternos.nombreIndex])) 
                        throw new Exception($"Field {nameof(Nombre)} is required.");
                    this.Nombre = data[IntegracionExternos.nombreIndex];
                }
                else
                    this.Nombre = null;

                this.Apellido1 = IntegracionExternos.apellido1Index >= 0 ? data[IntegracionExternos.apellido1Index] : null;
                this.Apellido2 = IntegracionExternos.apellido2Index >= 0 ? data[IntegracionExternos.apellido2Index] : null;
                this.NifOriginal = IntegracionExternos.nifIndex >= 0 ? data[IntegracionExternos.nifIndex] : null;
            }
            this.LastAction = "CREATE";
            this.LastActionDate = DateTime.UtcNow;
            this.UltimaModif = DateTime.UtcNow;

        }
    }
}
