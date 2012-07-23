// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameValueCollectionConverter.cs" company="Dow Jones">
//      © 2011 Dow Jones Factiva
// </copyright>
// <summary>
//   Defines the NameValuePair type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Linq;
using Newtonsoft.Json;

namespace DowJones.Infrastructure.Converters
{
    using System.Diagnostics.Contracts;

    /// <summary>
    ///  A JsonConverter for Name-Value Pairs
    /// </summary>
    internal class NameValueCollectionConverter : JsonConverterBase

    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is NameValueCollection))
            {
                return;
            }

            var collection = (NameValueCollection)value;
            var container = collection.AllKeys.Select(key => new NameValuePair
                                                                 {
                                                                     Name = key,
                                                                     Value = collection[key]
                                                                 }).ToList();

            var serialized = JsonConvert.SerializeObject(container);

            writer.WriteRawValue(serialized);
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
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
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
            if (objectType == null)
            {
                return false;
            }
            var t = IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
            return typeof(NameValueCollection).IsAssignableFrom(t);
        }
    }

    /// <summary>
    ///   A class that represents a name-value pair
    /// </summary>
    internal class NameValuePair
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value
        {
            get;
            set;
        }
    }
}
