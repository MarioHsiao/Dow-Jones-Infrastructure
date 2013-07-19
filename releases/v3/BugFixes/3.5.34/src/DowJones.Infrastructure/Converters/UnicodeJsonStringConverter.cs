// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnicodeJsonStringConverter.cs" company="Dow Jones">
// © 2011 Dow Jones Factiva
// </copyright>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Infrastructure.Converters
{                            
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UnicodeJsonStringConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var buffer = new StringBuilder();
            buffer.Append("\"");
            var stringValue = (string)value;
            foreach (var c in stringValue)
            {
                var code = (int)c;
                switch (c)
                {
                    case '\"':
                        buffer.Append("\\\"");
                        break;
                    case '\\':
                        buffer.Append("\\\\");
                        break;
                    case '\r':
                        buffer.Append("\\r");
                        break;
                    case '\n':
                        buffer.Append("\\n");
                        break;
                    case '\t':
                        buffer.Append("\\t");
                        break;
                    case '\b':
                        buffer.Append("\\b");
                        break;
                    case '/':
                        buffer.Append("\\/");
                        break;
                    case '\f':
                        buffer.Append("\\f");
                        break;
                    default:
                        if (code > 127)
                        {
                            buffer.AppendFormat("\\u{0:x4}", code);
                        }
                        else
                        {
                            buffer.Append(c);
                        }

                        break;
                }
            }

            buffer.Append("\"");

            writer.WriteRawValue(buffer.ToString());
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="objectType">The object type.</param>
        /// <param name="existingValue">The existing value.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Automatic conversion to string from other types
            if (reader.ValueType != objectType && objectType == typeof(string))
            {
                return reader.Value.ToString();
            }

            return reader.Value;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">The object type.</param>
        /// <returns>
        ///   <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
