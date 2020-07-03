using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace AccionaCovid.Application.Core
{
    /// <summary>
    /// Dto class for enumerations
    /// </summary>
    public class EnumerationDTO
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EnumerationDTO() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="description"></param>
        public EnumerationDTO(String code, String description)
        {
            Code = code;
            Description = description;
        }

        /// <summary>
        /// Método que obtiene los diferentes valores del enumerado ordenados por su descripción
        /// FORMATO del archivo resx: nombreEnumerado + "_" + CampoEnumerado
        /// </summary>
        /// <typeparam name="T"> Enumerado </typeparam>
        /// <param name="manager"> ResourceManager del archivo resx asociado al enumerado </param>
        /// <returns> Lista de EnumerationDTO </returns>
        public static IList<EnumerationDTO> FromEnum<T>(ResourceManager manager) where T : struct, IConvertible
        {
            var typeOfEnum = typeof(T);
            var typeNamePrefix = typeOfEnum.Name + "_";

            if(!typeOfEnum.IsEnum) throw new ArgumentException("T must be an enumerable");

            return Enum.GetValues(typeOfEnum).OfType<T>()
                    .Select(e => new EnumerationDTO(e.ToString(CultureInfo.InvariantCulture), manager.GetString(typeNamePrefix + e.ToString(CultureInfo.InvariantCulture))))
                    .OrderBy(e => e.Description)
                    .ToList();
        }

        /// <summary>
        /// Método que obtiene los diferentes valores del enumerado ordenados por su **valor entero**
        /// FORMATO del archivo resx: nombreEnumerado + "_" + CampoEnumerado
        /// </summary>
        /// <typeparam name="T"> Enumerado </typeparam>
        /// <param name="manager"> ResourceManager del archivo resx asociado al enumerado </param>
        /// <returns> Lista de EnumerationDTO </returns>
        public static IList<EnumerationDTO> FromEnumOrderByValue<T>(ResourceManager manager) where T : struct, IConvertible
        {
            var typeOfEnum = typeof(T);
            var typeNamePrefix = typeOfEnum.Name + "_";

            if(!typeOfEnum.IsEnum) throw new ArgumentException("T must be an enumerable");

            return Enum.GetValues(typeOfEnum).OfType<T>()
                    .OrderBy(e => Convert.ToInt32(e))
                    .Select(e => new EnumerationDTO(e.ToString(CultureInfo.InvariantCulture), manager.GetString(typeNamePrefix + e.ToString(CultureInfo.InvariantCulture))))
                    .ToList();
        }
    }
}
