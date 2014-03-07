using DowJones.Json.Gateway.Converters;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IJsonSerializable
    {
        string ToJson();
    }
}