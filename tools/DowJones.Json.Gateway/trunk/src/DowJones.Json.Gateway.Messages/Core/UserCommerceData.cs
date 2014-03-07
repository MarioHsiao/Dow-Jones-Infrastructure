using System.Collections.Generic;
using DowJones.Json.Gateway.Core;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    public class UserCommerceData : AbstractJsonSerializable, IUserCommerceData
    {
        public string AccessPointCode { get; set; }

        public string AccessPointCodeUsage { get; set; }

        public string BypassClientBilling { get; set; }

        public ClientCode ClientCode { get; set; }

        public string ClientType { get; set; }

        public string CompositeId { get; set; }

        public string NetworkPartnerId { get; set; }
        
        public List<Token> Tokens { get; set; }
    }
}