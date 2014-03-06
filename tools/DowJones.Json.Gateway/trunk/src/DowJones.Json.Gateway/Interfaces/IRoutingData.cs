namespace DowJones.Json.Gateway.Interfaces
{
    public interface IRoutingData
    {
        int ContentServerAddress { get; set; }

        string TransportType { get; set; }
    }
}