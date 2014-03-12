using System.IO;
using DowJones.Json.Gateway.Extentions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RestSharp;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace DowJones.Json.Gateway.Converters
{
    internal class JsonDotNetJsonConverter : ISerialize
    {
        public JsonSerializer Serializer { get; private set; }

        /// <summary>
        ///     Default serializer
        /// </summary>
        public JsonDotNetJsonConverter()
        {
            ContentType = "application/json";

            Serializer = new JsonSerializer
                         {
                             NullValueHandling = NullValueHandling.Ignore,
                             DefaultValueHandling = DefaultValueHandling.Ignore,
                             ContractResolver = new DefaultContractResolver(),
                             DateTimeZoneHandling = DateTimeZoneHandling.Utc
                         };
            Serializer.Converters.Add(new StringEnumConverter());
        }

        /// <summary>
        ///     Default serializer with overload for allowing custom Json.NET settings
        /// </summary>
        public JsonDotNetJsonConverter(JsonSerializer serializer)
        {
            ContentType = "application/json";
            Serializer = serializer;
        }

        protected internal T Deserialize<T>(IRestResponse response)
        {
            return Deserialize<T>(response.Content);
        }

        protected internal T Deserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        ///     Content type for serialized content
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     Unused for JSON Serialization
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        ///     Unused for JSON Serialization
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        ///     Unused for JSON Serialization
        /// </summary>
        public string RootElement { get; set; }

        public string Serialize(object obj, Formatting formatting = Formatting.None)
        {
            var stringWriter = new StringWriter();
            using (var jsonTextWriter = new JsonTextWriter(stringWriter))
            {
                jsonTextWriter.Formatting = Formatting.None.ConvertTo<Newtonsoft.Json.Formatting>();
                jsonTextWriter.QuoteChar = '"';
                Serializer.Serialize(jsonTextWriter, obj);
                return stringWriter.ToString();
            }
        }
    }
}