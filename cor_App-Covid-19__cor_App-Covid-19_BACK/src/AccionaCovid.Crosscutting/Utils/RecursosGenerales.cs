using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.Crosscutting
{
    public class RecursosGenerales
    {
        /// <summary>
        /// Ruta de los ficheros IFC
        /// </summary>
        private string ifcFilesPath = string.Empty;

        /// <summary>
        /// Variable con la ruta raíz de los ficheros IFC
        /// </summary>
        public string IFCFilesPath
        {
            get { return ifcFilesPath; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RecursosGenerales(string ifcFilesPath)
        {
            this.ifcFilesPath = ifcFilesPath;
        }
    }
}
