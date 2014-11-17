using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "ProxyCredentials")]
    [XmlType(TypeName = "ProxyCredentials", Namespace = Declarations.SchemaVersion), Serializable]
    public class ProxyCredentials
    {
        [JsonProperty(PropertyName = "authenticationScheme")]
        [XmlElement(ElementName = "authenticationScheme", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "authenticationScheme")]
        public AuthenticationScheme __authenticationScheme;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __authenticationSchemeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public AuthenticationScheme AuthenticationScheme
        {
            get { return __authenticationScheme; }
            set { __authenticationScheme = value; __authenticationSchemeSpecified = true; }
        }

        [JsonProperty(PropertyName = "userId")]
        [XmlElement(ElementName = "userId", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "userId")]
        public string __userId;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string UserId
        {
            get { return __userId; }
            set { __userId = value; }
        }

        [JsonProperty(PropertyName = "emailId")]
        [XmlElement(ElementName = "emailId", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "emailId")]
        public string __emailAddress;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string EmailAddress
        {
            get { return __emailAddress; }
            set { __emailAddress = value; }
        }

        [JsonProperty(PropertyName = "namespace")]
        [XmlElement(ElementName = "namespace", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "namespace")]
        public string __namespace;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Namespace
        {
            get { return __namespace; }
            set { __namespace = value; }
        }

        [JsonProperty(PropertyName = "password")]
        [XmlElement(ElementName = "password", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "password")]
        public string __password;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Password
        {
            get { return __password; }
            set { __password = value; }
        }

        [JsonProperty(PropertyName = "encryptedToken")]
        [XmlElement(ElementName = "encryptedToken", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "encryptedToken")]
        public string __encryptedToken;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string EncryptedToken
        {
            get { return __encryptedToken; }
            set { __encryptedToken = value; }
        }
    }
}
