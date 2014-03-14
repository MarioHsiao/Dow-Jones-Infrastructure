using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Json.Gateway.Common;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    [DataContract]
    public class UserCommerceData : AbstractJsonSerializable, IUserCommerceData
    {
        [DataMember]
        public string AccessPointCode { get; set; }

        [DataMember]
        public string AccessPointCodeUsage { get; set; }

        [DataMember]
        public string BypassClientBilling { get; set; }

        [DataMember]
        public ClientCode ClientCode { get; set; }

        [DataMember]
        public string ClientType { get; set; }

        [DataMember]
        public string CompositeId { get; set; }

        [DataMember]
        public string NetworkPartnerId { get; set; }

        [DataMember]
        public List<Token> Tokens { get; set; }
    }
}