using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Converters
{
    class JsonSerializerFactory
    {
        public static DataConverterDecorator Create(JsonSerializer serializer )
        {
            return serializer == JsonSerializer.DataContract ? DataContractConverterDecoratorSingleton.Instance : JsonDotNetConverterDecoratorSingleton.Instance;
        }

    }
}
