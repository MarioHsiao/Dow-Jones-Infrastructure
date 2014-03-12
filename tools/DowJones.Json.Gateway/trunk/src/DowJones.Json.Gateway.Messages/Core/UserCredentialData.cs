using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Json.Gateway.Common;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    [DataContract(Name = "UserCredentialData")]
    public class UserCredentialData : AbstractJsonSerializable, IUserCredentialData
    {
        public UserCredentialData()
        {
            IpAddress = "127.0.0.1";
        }

        [DataMember]
        public string AccountId { get; set; }

        [DataMember]
        public string CallingUrl { get; set; }

        [DataMember]
        public string EncryptedToken { get; set; }

        [DataMember(Name = "EID")]
        public string EncryptedUserId { get; set; }

        [DataMember(Name = "UUID")]
        public string UserGuidId { get; set; }

        [DataMember]
        public string IpAddress { get; set; }

        [DataMember]
        public string Namespace { get; set; }

        [DataMember]
        public string ProxyNamespace { get; set; }

        [DataMember]
        public string ProxyUserId { get; set; }

        [DataMember]
        public string ReferringUrl { get; set; }

        [DataMember]
        public string SessionId { get; set; }

        [DataMember]
        public List<Token> Tokens { get; set; }

        [DataMember]
        public string UserFlavor { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string UserPassword { get; set; }       
    }
}