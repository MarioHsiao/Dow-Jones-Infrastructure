using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    public class RoutingData : AbstractJsonSerializable, IRoutingData
    {
        public RoutingData()
        {
            TransportType = Properties.Settings.Default.TransportType;
        }        
        
        public int ContentServerAddress { get; set; }
        
        public string TransportType { get; set; }
    }
}
