// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonDateTimeConverter.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   A converter for parsing multiple Twitter API date formats.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DowJones.Infrastructure.Converters
{
    using System;
    using System.Diagnostics.Contracts;
    using Newtonsoft.Json;
    using DowJones.Infrastructure.Models.SocialMedia;

    /// <summary>
    /// A converter for parsing multiple Twitter API date formats.
    /// </summary>
    public class JsonDateTimeConverter : JsonConverterBase
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
            return CanConvert<DateTime>(objectType);
        }

        /// <summary>
        /// Reads the JSON.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns>
        /// The read json.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var value = reader.Value.ToString();
            var date = TwitterDateTime.ConvertToDateTime(value);

            return date;
        }

        /// <summary>
        /// Writes the JSON.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is TwitterDateTime)
            {
                writer.WriteValue(value.ToString());
            }

            if (!(value is DateTime))
            {
                return;
            }

            var dateTime = (DateTime)value;
            var converted = TwitterDateTime.ConvertFromDateTime(dateTime, TwitterDateFormat.RestApi);

            writer.WriteValue(converted);
        }

        #endregion
    }
}