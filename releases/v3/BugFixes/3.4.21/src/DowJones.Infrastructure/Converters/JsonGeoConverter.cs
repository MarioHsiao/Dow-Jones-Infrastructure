// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonGeoConverter.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   This converter exists to convert geo-spatial coordinates.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DowJones.Infrastructure.Converters
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using DowJones.Infrastructure.Models.SocialMedia;
    using Newtonsoft.Json;

    /// <summary>
    /// This converter exists to convert geo-spatial coordinates.
    /// </summary>
    public class JsonGeoConverter : JsonConverterBase
    {
        #region Constants and Fields

        /// <summary>
        /// The geo template.
        /// </summary>
        private const string GeoTemplate = "\"geo\":{{\"type\":\"Point\",\"coordinates\":[{0}, {1}]}}";

        #endregion

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
            return CanConvert<TwitterGeoLocation.GeoCoordinates>(objectType);
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

            if (reader.TokenType != JsonToken.StartArray)
            {
                return null;
            }

            reader.Read();
            var coords = new double[2];
            if (reader.TokenType == JsonToken.Float)
            {
                coords[0] = (double)reader.Value;
                reader.Read();
            }

            if (reader.TokenType == JsonToken.Float)
            {
                coords[1] = (double)reader.Value;
                reader.Read();
            }

            var latitude = coords[0];
            var longitude = coords[1];

            return new TwitterGeoLocation.GeoCoordinates { Latitude = latitude, Longitude = longitude };
        }

        /// <summary>
        /// Writes the JSON.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is TwitterGeoLocation))
            {
                return;
            }

            var location = (TwitterGeoLocation)value;
            var latitude = location.Coordinates.Latitude.ToString(CultureInfo.InvariantCulture);
            var longitude = location.Coordinates.Longitude.ToString(CultureInfo.InvariantCulture);
            var json = string.Format(GeoTemplate, latitude, longitude);
            writer.WriteValue(json);
        }

        #endregion
    }
}