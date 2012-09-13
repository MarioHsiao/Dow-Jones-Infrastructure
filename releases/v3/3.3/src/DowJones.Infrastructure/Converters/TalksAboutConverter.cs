﻿// -----------------------------------------------------------------------
// <copyright file="TalksAboutConverter.cs" company="Dow Jones & Co.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DowJones.Infrastructure.Converters
{
    using System;
    using System.Diagnostics.Contracts;
    using DowJones.Infrastructure.Models.SocialMedia;
    using Newtonsoft.Json;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TalksAboutConverter : JsonConverterBase
    {
        #region Overrides of JsonConverter

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is TalksAbout)
            {
                writer.WriteValue(value.ToString());
            }
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String && !string.IsNullOrWhiteSpace((string)reader.Value))
            {
                var temp = new TalksAbout();
                var value = (string)reader.Value;
                var rawData = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (rawData.Length > 0)
                {
                    temp.AddRange(rawData);
                    return temp;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///   <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return CanConvert<TalksAbout>(objectType);
        }

        #endregion
    }
}