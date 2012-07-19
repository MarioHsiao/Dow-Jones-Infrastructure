using System.Runtime.Serialization;

namespace DowJones.Infrastructure
{
    [DataContract(Name = "credentials", Namespace = "")]
    public class Credentials
    {
        [DataMember(Name = "token")]
        public string Token;

        [DataMember(Name = "credentialType")]
        public CredentialType CredentialType;

        [DataMember(Name = "remoteAddress")]
        public string RemoteAddress;

        [DataMember(Name = "accessPointCode")]
        public string AccessPointCode;

        [DataMember(Name = "accessPointCodeUsage")]
        public string AccessPointCodeUsage;

        [DataMember(Name = "clientCode")]
        public string ClientCode;

        [DataMember(Name = "cacheKey")]
        public string CacheKey;

        [DataMember(Name = "proxyUserId")]
        public string ProxyUserId;

        [DataMember(Name = "proxyUserNamespace")]
        public string ProxyUserNamespace;

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


