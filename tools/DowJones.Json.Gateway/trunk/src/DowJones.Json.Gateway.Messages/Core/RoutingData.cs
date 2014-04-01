using System.Runtime.Serialization;
using DowJones.Json.Gateway.Common;
using DowJones.Json.Gateway.Interfaces;
using Environment = DowJones.Json.Gateway.Common.Environment;

namespace DowJones.Json.Gateway.Messages.Core
{
    [DataContract]
    public class RoutingData : AbstractJsonSerializable, IRoutingData
    {
        public RoutingData()
        {
            TransportType = Properties.Settings.Default.TransportType;
            ServerUri = Properties.Settings.Default.ServerUri;
            Environment = Environment.Proxy;
        }        
        
        [DataMember]
        public int ContentServerAddress { get; set; }
        
        [DataMember]
        public string TransportType { get; set; }

        [IgnoreDataMember]
        public Environment Environment { get; set; }

        [IgnoreDataMember]
        public JsonSerializer Serializer { get; set; }

        [IgnoreDataMember]
        public string ServerUri { get; set; }
    }
}
