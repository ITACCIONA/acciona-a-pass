using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Acciona.Domain.Utils
{
    public static class StringUtils
    {
        public static string RemoveDiacritics(this string text)
        {
            return string.Concat(
                text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                              UnicodeCategory.NonSpacingMark)
              ).Normalize(NormalizationForm.FormC);
        }

        public static bool IgnoreContains(this string text,string contains)
        {
            if (text == null)
                return false;
            return text.ToUpper().RemoveDiacritics().Contains(contains.ToUpper().RemoveDiacritics());
        }

        public static string FormatNumberDecimal(double number, bool thousands)
        {
            if (thousands)
                return String.Format(CultureInfo.GetCultureInfo("es-ES"), number % 1 == 0 ? "{0:#,##0}" : "{0:#,##0.00}", number);
            else
                return String.Format(CultureInfo.GetCultureInfo("es-ES"), number % 1 == 0 ? "{0:0}" : "{0:0.00}", number);
        }

        public static string FormatNumberDecimalCoin(double? number, bool thousands)
        {
            if (number == null) number = 0;
            if (thousands)
                return String.Format(CultureInfo.GetCultureInfo("es-ES"), number % 1 == 0 ? "{0:#,##0 €}" : "{0:#,##0.00 €}", number);
            else
                return String.Format(CultureInfo.GetCultureInfo("es-ES"), number % 1 == 0 ? "{0:0 €}" : "{0:0.00 €}", number);
        }
    }
}
