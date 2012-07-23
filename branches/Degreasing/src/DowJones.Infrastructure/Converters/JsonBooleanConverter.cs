// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonBooleanConverter.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   Sometimes Twitter returns "0" for "true", instead of true, and we've even seen
//   "13" for true. For those, and possibly future issues, this converter exists.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Newtonsoft.Json;

namespace DowJones.Infrastructure.Converters
{

    /// <summary>
    /// Sometimes Twitter returns "0" for "true", instead of true, and we've even seen
    ///   "13" for true. For those, and possibly future issues, this converter exists.
    /// </summary>
    public class JsonBooleanConverter : JsonConverterBase
    {
        #region Public Methods

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///   <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return CanConvert<bool>(objectType);
        }

        /// <summary>
        /// Reads the JSON.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns>
        /// The read JSON.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var value = reader.Value.ToString();
            var wonkyBool = value.Equals("0") ? true : value.Equals("1") ? false : TryConvertBool(value);
            return wonkyBool;
        }

        /// <summary>
        /// Writes the JSON.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is string)
            {
                var boolean = value.Equals("0");
                writer.WriteValue(boolean.ToString());
            }

            if (value is bool || value is bool?)
            {
                writer.WriteValue(value.ToString());
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tries the convert bool.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The try convert bool.
        /// </returns>
        private static bool TryConvertBool(string value)
        {
            bool result;
            if (bool.TryParse(value, out result))
            {
                return result;
            }

            return false;
        }

        #endregion
    }
}