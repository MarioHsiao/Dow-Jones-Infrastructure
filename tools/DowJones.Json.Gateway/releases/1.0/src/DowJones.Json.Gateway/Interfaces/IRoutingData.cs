using System.Collections.Generic;
using DowJones.Json.Gateway.Common;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IRoutingData : IJsonSerializable
    {
        bool ChunkingFlag { get; set; }

        ushort ContentServerAddress { get; set; }

        ulong ContentServerPortId { get; set; }

        ushort ContextId { get; set; }

        string DeliveryService { get; set; }

        string FileOpen { get; set; }

        string FileSave { get; set; }

        ulong Flags { get; set; }

        ushort FunctionType { get; set; }

        string HttpEndPoint { get; set; }

        ushort MajorServiceVersion { get; set; }

        ushort MinorServiceVersion { get; set; }

        bool MoreDataFlag { get; set; }

        ushort SequenceNo { get; set; }

        ushort ServiceType { get; set; }

        ushort SourceAddress { get; set; }

        ulong TimeStamp { get; set; }

        ulong TransactionId { get; set; }

        string TransactionType { get; set; }

        string TransportType { get; set; }

        IEnumerable<Token> Tokens { get; set; }

        Environment Environment { get; set; }

        JsonSerializer Serializer { get; set; }

        string ServerUri { get; set; }
    }
}