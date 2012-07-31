using System.Runtime.Serialization;

namespace DowJones.Infrastructure
{
    public interface ICredentials
    {
        [DataMember(Name = "token")]
        string Token { get; set; }

        [DataMember(Name = "credentialType")]
        CredentialType CredentialType { get; set; }

        [DataMember(Name = "remoteAddress")]
        string RemoteAddress { get; set; }

        [DataMember(Name = "accessPointCode")]
        string AccessPointCode { get; set; }

        [DataMember(Name = "accessPointCodeUsage")]
        string AccessPointCodeUsage { get; set; }

        [DataMember(Name = "clientCode")]
        string ClientCode { get; set; }

        [DataMember(Name = "cacheKey")]
        string CacheKey { get; set; }

        [DataMember(Name = "proxyUserId")]
        string ProxyUserId { get; set; }

        [DataMember(Name = "proxyUserNamespace")]
        string ProxyUserNamespace { get; set; }
    }

    [DataContract(Name = "credentials", Namespace = "")]
    public class Credentials : ICredentials
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "credentialType")]
        public CredentialType CredentialType { get; set; }

        [DataMember(Name = "remoteAddress")]
        public string RemoteAddress { get; set; }

        [DataMember(Name = "accessPointCode")]
        public string AccessPointCode { get; set; }

        [DataMember(Name = "accessPointCodeUsage")]
        public string AccessPointCodeUsage { get; set; }

        [DataMember(Name = "clientCode")]
        public string ClientCode { get; set; }

        [DataMember(Name = "cacheKey")]
        public string CacheKey { get; set; }

        [DataMember(Name = "proxyUserId")]
        public string ProxyUserId { get; set; }

        [DataMember(Name = "proxyUserNamespace")]
        public string ProxyUserNamespace { get; set; }

        public Credentials() { }
       
        public Credentials(CredentialType credentialType, string token)
        {
            Token = token;
            CredentialType = credentialType;
        }

    }

    [DataContract(Name = "credentialType", Namespace = "")]
    public enum CredentialType
    {
        [EnumMember]
        Session,
        [EnumMember]
        EncryptedToken
    }
}


