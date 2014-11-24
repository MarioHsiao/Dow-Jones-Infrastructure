using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "MastHeadData")]
    [XmlType(TypeName = "MastHeadData", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class MastHeadData
    {
        [JsonProperty(PropertyName = "textLarge")]
        [XmlElement(ElementName = "textLarge", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "textLarge")]
        public string __textLarge;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string TextLarge
        {
            get { return __textLarge; }
            set { __textLarge = value; }
        }

        [JsonProperty(PropertyName = "textSmall")]
        [XmlElement(ElementName = "textSmall", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "textSmall")]
        public string __textSmall;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string TextSmall
        {
            get { return __textSmall; }
            set { __textSmall = value; }
        }
    }
}
