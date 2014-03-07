using DowJones.Json.Gateway.Converters;

namespace DowJones.Json.Gateway.Messages.Core
{
    public abstract class AbstractJsonSerializable 
    {
        public string ToJson()
        {
            return JsonDataConverterSingleton.Instance.Serialize(this);
        }
    }
}