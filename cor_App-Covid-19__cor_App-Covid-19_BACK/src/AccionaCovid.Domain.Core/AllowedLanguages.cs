using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AccionaCovid.Domain.Core
{
    public class AllowedLanguages
    {
        private AllowedLanguages()
        {
            Languages = new List<CultureInfo>();
        }

        public static AllowedLanguages Instance { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly AllowedLanguages instance = new AllowedLanguages();
        }

        /// <summary>
        /// Listado de lenguajes permitidos
        /// </summary>
        public List<CultureInfo> Languages { get; set; }
    }
}
