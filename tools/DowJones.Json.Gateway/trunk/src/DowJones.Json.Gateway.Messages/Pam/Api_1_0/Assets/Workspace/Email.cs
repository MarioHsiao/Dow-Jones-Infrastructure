using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "Email")]
    [XmlType(TypeName = "Email", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class Email
    {
        [JsonProperty(PropertyName = "id")]
        [XmlElement(ElementName = "id", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "id")]
        public string __id;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Id
        {
            get { return __id; }
            set { __id = value; }
        }

        [JsonProperty(PropertyName = "displayName")]
        [XmlElement(ElementName = "displayName", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "displayName")]
        public string __displayName;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string DisplayName
        {
            get { return __displayName; }
            set { __displayName = value; }
        }

        public Email()
        {
        }
    }
}
