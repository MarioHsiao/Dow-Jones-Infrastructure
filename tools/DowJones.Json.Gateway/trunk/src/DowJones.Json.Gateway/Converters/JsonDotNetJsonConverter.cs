using System.IO;
using System.Runtime.Serialization.Formatters;
using DowJones.Json.Gateway.Extensions;
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
                             DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                             TypeNameHandling = TypeNameHandling.All,
                             MissingMemberHandling = MissingMemberHandling.Ignore,
                             //PreserveReferencesHandling = PreserveReferencesHandling.All,
                             TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                             
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

        public T Deserialize<T>(IRestResponse response)
        {
            return Deserialize<T>(response.Content);

        }

        public T Deserialize<T>(string str)
        {
            var reader = new JsonTextReader(new StringReader(str));
            return Serializer.Deserialize<T>(reader);
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

        public string Serialize<T>(T obj)
        {
            return Serialize(obj, Formatting.None);
        }

        /// <summary>
        ///     Unused for JSON Serialization
        /// </summary>
        public string RootElement { get; set; }

        public string Serialize(object obj, Formatting formatting)
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