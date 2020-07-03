using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AccionaCovid.Crosscutting
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, string> IsValidLatinString<T>(this IRuleBuilder<T, string> ruleBuilder) 
        {
            Regex regex = new Regex("[A-Za-z0-9ñÑçÇáéíóúÁÉÍÓÚàèìòùÀÈÌÒÙäëïöüÄËÏÖÜâêîôûÂÊÎÔÛ\\p{Zs}_-]+");
            return ruleBuilder.Must(strToValidate => string.IsNullOrWhiteSpace(strToValidate) || regex.IsMatch(strToValidate.ToString())).WithMessage("Invalid latin string");
        }
    }
}
