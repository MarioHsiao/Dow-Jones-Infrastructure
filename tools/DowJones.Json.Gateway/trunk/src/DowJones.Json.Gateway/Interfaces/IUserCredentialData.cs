using System.Collections.Generic;
using DowJones.Json.Gateway.Common;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IUserCredentialData : IJsonSerializable
    {
        string AccountId { get; set; }
        string CallingUrl { get; set; }
        string EncryptedToken { get; set; }
        string IpAddress { get; set; }
        string Namespace { get; set; }
        string ProxyNamespace { get; set; }
        string ProxyUserId { get; set; }
        string ReferringUrl { get; set; }
        string SessionId { get; set; }
        List<Token> Tokens { get; set; }
        string UserFlavor { get; set; }
        string UserId { get; set; }
        string UserPassword { get; set; }
    }
}