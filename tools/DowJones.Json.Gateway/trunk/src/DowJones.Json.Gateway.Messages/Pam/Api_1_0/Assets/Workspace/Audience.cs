using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "Audience")]
    [XmlType(TypeName = "Audience", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class Audience
    {
        [JsonProperty(PropertyName = "audienceOptions")]
        [XmlElement(ElementName = "audienceOptions", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "audienceOptions")]
        public AudienceOptions __audienceOptions;

        [JsonIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __audienceOptionsSpecified;

        [JsonIgnore]
        [IgnoreDataMember]
        public AudienceOptions AudienceOptions
        {
            get { return __audienceOptions; }
            set { __audienceOptions = value; __audienceOptionsSpecified = true; }
        }

        [JsonProperty(PropertyName = "proxyCredentials")]
        [XmlElement(Type = typeof(ProxyCredentials), ElementName = "proxyCredentials", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "proxyCredentials")]
        public ProxyCredentials __proxyCredentials;

        [JsonIgnore]
        [IgnoreDataMember]
        public ProxyCredentials ProxyCredentials
        {
            get
            {
                if (__proxyCredentials == null) __proxyCredentials = new ProxyCredentials();
                return __proxyCredentials;
            }
            set { __proxyCredentials = value; }
        }

        [JsonProperty(PropertyName = "profileId")]
        [XmlElement(ElementName = "profileId", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "profileId")]
        public string __profileId;

        [JsonIgnore]
        [IgnoreDataMember]
        public string ProfileId
        {
            get { return __profileId; }
            set { __profileId = value; }
        }

    }
}
