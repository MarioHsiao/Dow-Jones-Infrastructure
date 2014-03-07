using DowJones.Json.Gateway.Converters;

namespace DowJones.Json.Gateway.Messages.Core
{
    public abstract class AbstractJsonSerializable
    {
        public ISerializer Serializer { get; set; }
        
        public string ToJson()
        {
            var serializer = Serializer ?? JsonDataConverterSingleton.Instance;
            return serializer.Serialize(this);
        }
    }
}