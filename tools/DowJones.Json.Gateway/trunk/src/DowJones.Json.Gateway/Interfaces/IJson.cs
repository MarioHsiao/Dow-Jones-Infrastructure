using DowJones.Json.Gateway.Converters;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IJsonSerializable
    {
        ISerializer Serializer { get; set; }

        string ToJson();
    }
}