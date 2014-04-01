using DowJones.Json.Gateway.Common;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IRoutingData : IJsonSerializable
    {
        int ContentServerAddress { get; set; }

        string TransportType { get; set; }

        Environment Environment { get; set; }

        JsonSerializer Serializer { get; set; }

        string ServerUri { get; set; }
    }
}

