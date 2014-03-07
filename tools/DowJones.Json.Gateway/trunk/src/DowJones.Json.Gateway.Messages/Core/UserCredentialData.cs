using System.Collections.Generic;
using DowJones.Json.Gateway.Core;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    public class UserCredentialData : AbstractJsonSerializable, IUserCredentialData
    {
        public UserCredentialData()
        {
            IpAddress = "127.0.0.1";
        }

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

       
    }
}