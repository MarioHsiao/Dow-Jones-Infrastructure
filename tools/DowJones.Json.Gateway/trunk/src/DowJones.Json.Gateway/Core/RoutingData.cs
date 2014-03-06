using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Core
{
    public class RoutingData : IRoutingData
    {        
        public int ContentServerAddress { get; set; }
        public string TransportType { get; set; }
    }
}
