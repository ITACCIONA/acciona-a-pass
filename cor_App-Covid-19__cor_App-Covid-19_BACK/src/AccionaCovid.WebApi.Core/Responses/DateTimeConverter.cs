
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            //MM/dd/yyyy HH:mm
            writer.WriteStringValue(value.ToString("yyyy/MM/dd HH:mm"));
        }
    }
}
