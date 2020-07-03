using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AccionaCovid.Crosscutting
{
    /// <summary>
    /// Clase que extiende comportamiento de objetos <see cref="String"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>Elimina determinados signos diacríticos existentes en un texto.</summary>
        ///
        /// <param name="text">Texto a revisar.</param>
        ///
        /// <returns>Texto limpio de signos diacríticos.</returns>
        public static string RemoveDiacritics2(this string text)
        {
            var a = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            var e = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            var i = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            var o = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            var u = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            var n = new Regex("[ñ|Ñ]", RegexOptions.Compiled);
            text = a.Replace(text, "a");
            text = e.Replace(text, "e");
            text = i.Replace(text, "i");
            text = o.Replace(text, "o");
            text = u.Replace(text, "u");
            text = n.Replace(text, "n");

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark) stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Elimina los diacríticos.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string text)
        {
            return string.Concat(text.Normalize(NormalizationForm.FormD)
                                     .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark))
                         .Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Cedena de texto a mayusculas invariant o nulo.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToUpperInvariantCheckNull(this string text)
        {
            return text?.ToUpperInvariant();
        }

        /// <summary>
        /// Compara dos cadenas sin tener en cuenta ni acentos ni mayúsculas.
        /// </summary>
        /// <param name="text"> Texto principal a comparar</param>
        /// <param name="text2"> Texto con el que comparar</param>
        /// <returns></returns>
        public static bool EqualIgnoreCultureAndCase(this string text, string text2)
        {
            if (string.IsNullOrWhiteSpace(text) && string.IsNullOrWhiteSpace(text2))
                return true;

            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(text2))
                return false;

            return text.Trim().RemoveDiacritics().Equals(text2.Trim().RemoveDiacritics(), StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Extensión que concatena los miembros de una coleccion utilizando el separador especificado entre todos los miembros.
        /// </summary>
        /// <param name="list">Coleccion que contiene los elementos concatenar.</param>
        /// <param name="separator">Cadena que se va a usar como separador.</param>
        /// <returns>Devuelve la cadena formada con todos los elementos concatenados.</returns>
        public static string ToStringJoinExt<T>(this IEnumerable<T> list, string separator)
        {
            var enumerable = list as IList<T> ?? list.ToList();

            string result;

            if (typeof(double) == typeof(T))
                result = string.Join(separator, enumerable.Select(x => ((double)(object)x).ToString(CultureInfo.InvariantCulture)));
            else if (typeof(decimal) == typeof(T))
                result = string.Join(separator, enumerable.Select(x => ((decimal)(object)x).ToString(CultureInfo.InvariantCulture)));
            else if (typeof(float) == typeof(T))
                result = string.Join(separator, enumerable.Select(x => ((float)(object)x).ToString(CultureInfo.InvariantCulture)));
            else
                result = string.Join(separator, enumerable.Select(x => Convert.ToString(x, CultureInfo.InvariantCulture)));

            return result;
        }
    }
}
