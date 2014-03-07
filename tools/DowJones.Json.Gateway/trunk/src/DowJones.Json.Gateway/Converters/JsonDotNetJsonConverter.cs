using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace DowJones.Json.Gateway.Converters
{
    internal class JsonDotNetJsonConverter : IJsonConverter
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
                              ContractResolver = new DefaultContractResolver()
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
            var deserializedObject = JsonConvert.DeserializeObject<T>(response.Content);
            return deserializedObject;
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

        /// <summary>
        ///     Serialize the object as JSON
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>JSON as String</returns>
        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    jsonTextWriter.Formatting = Formatting.Indented;
                    jsonTextWriter.QuoteChar = '"';

                    Serializer.Serialize(jsonTextWriter, obj);

                    var result = stringWriter.ToString();
                    return result;
                }
            }
        }
    }
}