using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "DataContentFilter")]
    [XmlType(TypeName = "DataContentFilter", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class DataContentFilter : Filter
    {
        [JsonProperty(PropertyName = "dataOperator")]
        [XmlElement(ElementName = "dataOperator", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "dataOperator")]
        public DataContentFilterOperator __dataOperator;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __dataOperatorSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DataContentFilterOperator DataOperator
        {
            get { return __dataOperator; }
            set { __dataOperator = value; __dataOperatorSpecified = true; }
        }
    }
}
