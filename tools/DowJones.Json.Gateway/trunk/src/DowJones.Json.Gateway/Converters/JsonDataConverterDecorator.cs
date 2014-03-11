using RestSharp;
using RestSharp.Serializers;
using RestSharp.Deserializers;

namespace DowJones.Json.Gateway.Converters
{
    class JsonDataConverterDecorator : ISerializer, IDeserializer
    {
        private readonly JsonDotNetJsonConverter _jsonConverter;

        public JsonDataConverterDecorator(JsonDotNetJsonConverter converter)
        {
            _jsonConverter = converter;
        }

        public string Serialize(object obj)
        {
            return _jsonConverter.Serialize(obj);
        }

        public T Deserialize<T>(IRestResponse response)
        {
            return _jsonConverter.Deserialize<T>(response);
        }

        protected internal T Deserialize<T>(string str)
        {
            return _jsonConverter.Deserialize<T>(str);
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
        public string ContentType { get; set; }
    }
}