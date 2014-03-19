using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace DowJones.Json.Gateway.Converters
{
    public interface IDataConverterDecorator : ISerialize
    {
        T Deserialize<T>(string str);
    }

    public abstract class DataConverterDecorator : ISerializer, IDeserializer, IDataConverterDecorator
    {
        public abstract string Serialize<T>(T obj, Formatting formatting);

        public abstract string Serialize<T>(T obj);

        public abstract string Serialize(object obj);
        public string RootElement { get; set; }
        
        public string Namespace { get; set; }
        
        public string DateFormat { get; set; }
        
        string ISerializer.Namespace { get; set; }

        string ISerializer.DateFormat { get; set; }
        
        public abstract T Deserialize<T>(IRestResponse response);

        public abstract T Deserialize<T>(string str);

        public abstract string ContentType { get; set; }

        public abstract string Serialize(object obj, Formatting formatting);
    }

}