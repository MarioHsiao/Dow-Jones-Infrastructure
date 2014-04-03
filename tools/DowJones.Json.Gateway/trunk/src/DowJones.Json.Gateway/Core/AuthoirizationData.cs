using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Json.Gateway.Common;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    [DataContract]
    public class AuthorizationData : AbstractJsonSerializable, IAuthorizationData
    {
        public AuthorizationData()
        {
            Tokens = new List<Token>();
        }

        [DataMember]
        public List<AuthComponent> AuthComponents { get; set; }

        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public string CustomerType { get; set; }

        [DataMember]
        public string RuleCode { get; set; }

        [DataMember]
        public string TimeZone { get; set; }

        [DataMember]
        public IEnumerable<Token> Tokens { get; set; }
    }
}