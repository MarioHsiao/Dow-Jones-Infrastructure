using System.Collections.Generic;
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
            Environment = Properties.Settings.Default.Environment;
            Serializer = Properties.Settings.Default.JsonSerializer;
            Tokens = new List<Token>();
        }

        [DataMember]
        public bool ChunkingFlag { get; set; }

        [DataMember]
        ushort IRoutingData.ContentServerAddress { get; set; }

        [DataMember]
        public ulong ContentServerPortId { get; set; }

        [DataMember]
        public ushort ContextId { get; set; }

        [DataMember]
        public string DeliveryService { get; set; }

        [DataMember]
        public string FileOpen { get; set; }

        [DataMember]
        public string FileSave { get; set; }


        [DataMember]
        public ulong Flags { get; set; }

        [DataMember]
        public ushort FunctionType { get; set; }

        [DataMember]
        public string ServiceUrl { get; set; }

        [DataMember]
        public ushort MajorServiceVersion { get; set; }

        [DataMember]
        public ushort MinorServiceVersion { get; set; }

        [DataMember]
        public bool MoreDataFlag { get; set; }

        [DataMember]
        public ushort SequenceNo { get; set; }

        [DataMember]
        public ushort ServiceType { get; set; }

        [DataMember]
        public ushort SourceAddress { get; set; }

        [DataMember]
        public ulong TimeStamp { get; set; }

        [DataMember]
        public ulong TransactionId { get; set; }
        
        [DataMember]
        public string TransactionType { get; set; }

        [DataMember]
        public int ContentServerAddress { get; set; }
        
        [DataMember]
        public string TransportType { get; set; }

        public IEnumerable<Token> Tokens { get; set; }

        [IgnoreDataMember]
        public Environment Environment { get; set; }

        [IgnoreDataMember]
        public JsonSerializer Serializer { get; set; }

        [IgnoreDataMember]
        public string ServerUri { get; set; }
    }
}
