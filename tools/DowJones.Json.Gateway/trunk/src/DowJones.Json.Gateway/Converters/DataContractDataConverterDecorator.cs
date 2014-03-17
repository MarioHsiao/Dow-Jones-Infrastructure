using RestSharp;

namespace DowJones.Json.Gateway.Converters
{
    class DataContractDataConverterDecorator : DataConverterDecorator
    {
        private readonly DataContractJsonConverter _jsonConverter;

        public DataContractDataConverterDecorator(DataContractJsonConverter converter)
        {
            ContentType = "application/json";
            _jsonConverter = converter;
        }

        public override string Serialize(object obj)
        {
            return _jsonConverter.Serialize(obj);
        }

        public T Deserialize<T>(RestResponse response)
            where T : new()
        {
            return _jsonConverter.Deserialize<T>(response);
        }

        protected internal override T Deserialize<T>(string str)
        {
            return _jsonConverter.Deserialize<T>(str);
        }

        public override T Deserialize<T>(IRestResponse response)
        {
            return _jsonConverter.Deserialize<T>(response);
        }

        public override sealed string ContentType { get; set; }
        public override string Serialize(object obj, Formatting formatting)
        {
            return Serialize(obj);
        }
    }
}