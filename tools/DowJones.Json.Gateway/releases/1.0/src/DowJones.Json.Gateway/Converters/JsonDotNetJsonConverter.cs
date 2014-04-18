using System.IO;
using System.Runtime.Serialization.Formatters;
using DowJones.Json.Gateway.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace DowJones.Json.Gateway.Converters
{
    internal class JsonDotNetJsonConverter : ISerialize
    {
        public JsonSerializer Serializer { get; private set; }

        /// <summary>
        ///     Default serializer
        /// </summary>
        /// 
        public JsonDotNetJsonConverter()
        {
            Serializer = new JsonSerializer
                         {
                             NullValueHandling = NullValueHandling.Ignore,
                             DefaultValueHandling = DefaultValueHandling.Ignore,
                             ContractResolver = new DefaultContractResolver(),
                             DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                             TypeNameHandling = TypeNameHandling.Auto,
                             Binder = new TypeNameSerializationBinder(),
                             MissingMemberHandling = MissingMemberHandling.Ignore,
                             //PreserveReferencesHandling = PreserveReferencesHandling.All,
                             TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
                         };
            Serializer.Converters.Add(new StringEnumConverter {AllowIntegerValues = false});
        }

        /// <summary>
        ///     Default serializer with overload for allowing custom Json.NET settings
        /// </summary>
        public JsonDotNetJsonConverter(JsonSerializer serializer)
        {
            Serializer = serializer;
        }

        public T Deserialize<T>(string str)
        {
            str = str.Replace("\"__type\":", "\"$type\":");
            var reader = new JsonTextReader(new StringReader(str));
            return Serializer.Deserialize<T>(reader);
        }


        public string Serialize<T>(T obj, Formatting formatting)
        {
            var stringWriter = new StringWriter();
            using (var jsonTextWriter = new JsonTextWriter(stringWriter))
            {
                jsonTextWriter.Formatting = Formatting.None.ConvertTo<Newtonsoft.Json.Formatting>();
                jsonTextWriter.QuoteChar = '"';
                Serializer.Serialize(jsonTextWriter, obj, typeof(T));
                return stringWriter.ToString();
            }
        }

        public string Serialize<T>(T obj)
        {
            return Serialize(obj, Formatting.None);
        }
    }
}