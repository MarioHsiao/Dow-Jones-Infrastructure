using RestSharp;

namespace DowJones.Json.Gateway.Converters
{
    class JsonDotNetDataConverterDecorator : DataConverterDecorator
    {
        private readonly JsonDotNetJsonConverter _jsonConverter;

        public JsonDotNetDataConverterDecorator(JsonDotNetJsonConverter converter)
        {
            ContentType = "application/json";
            _jsonConverter = converter;
        }

        public override string Serialize<T>(T obj)
        {
            return _jsonConverter.Serialize(obj);
        }

        public override string Serialize(object obj)
        {
            return _jsonConverter.Serialize(obj);
        }

        public override T Deserialize<T>(IRestResponse response)
        {
            return _jsonConverter.Deserialize<T>(response);
        }

        public override sealed string ContentType { get; set; }

        protected internal override T Deserialize<T>(string str)
        {
            return _jsonConverter.Deserialize<T>(str);
        }

        public override string Serialize(object obj, Formatting formatting)
        {
            throw new System.NotImplementedException();
        }
    }
}