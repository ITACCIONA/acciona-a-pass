using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AccionaCovid.Crosscutting
{
    /// <summary>
    /// Extensiones para Json.
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Serializes the object to a JSON string using lowerCamelCase.
        /// </summary>
        /// <param name="value">Object to Serializes</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string ToLowerCamelJson(object value)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(value, settings);
        }
    }
}
