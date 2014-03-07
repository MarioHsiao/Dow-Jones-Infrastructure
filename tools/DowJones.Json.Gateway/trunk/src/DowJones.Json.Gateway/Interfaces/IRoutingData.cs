namespace DowJones.Json.Gateway.Interfaces
{
    public interface IRoutingData : IJsonSerializable
    {
        int ContentServerAddress { get; set; }

        string TransportType { get; set; }
    }
}