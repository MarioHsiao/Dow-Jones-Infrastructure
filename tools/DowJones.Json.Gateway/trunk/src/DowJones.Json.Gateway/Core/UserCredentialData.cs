
using System.Collections.Generic;
using DowJones.Json.Gateway.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Json.Gateway.Core
{
    public class UserCredentialData : IUserCredentialData
    {
        public string AccountId { get; set; }

        public string CallingUrl { get; set; }

        public string EncryptedToken { get; set; }

        public string IpAddress { get; set; }

        public string Namespace { get; set; }

        public string ProxyNamespace { get; set; }

        public string ProxyUserId { get; set; }

        public string ReferringUrl { get; set; }

        public string SessionId { get; set; }

        public List<Token> Tokens { get; set; }

        public string UserFlavor { get; set; }

        public string UserId { get; set; }

        public string UserPassword { get; set; }
        
        public string ToJson()
        {
            var ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver(),
            });

            return !string.IsNullOrEmpty(ignored) ? string.Format("{0}", ignored) : string.Empty;
        }
    }
}