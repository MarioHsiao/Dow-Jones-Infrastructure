using System.Collections.Generic;
using DowJones.Json.Gateway.Common;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IUserCommerceData : IJsonSerializable
    {
        string AccessPointCode { get; set; }

        string AccessPointCodeUsage { get; set; }
        
        string BypassClientBilling { get; set; }
        
        ClientCode ClientCode { get; set; }
        
        string ClientType { get; set; }
        
        string CompositeId { get; set; }
        
        string NetworkPartnerId { get; set; }
        
        IEnumerable<Token> Tokens { get; set; }
    }
}