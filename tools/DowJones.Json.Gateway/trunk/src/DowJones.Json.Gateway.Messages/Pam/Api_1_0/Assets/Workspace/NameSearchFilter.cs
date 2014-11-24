using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "NameSearchFilter")]
    [XmlType(TypeName = "NameSearchFilter", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class NameSearchFilter : Filter
    {
        [JsonProperty(PropertyName = "name")]
        [XmlElement(Type = typeof(string), ElementName = "name", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "name")]
        public NameSearchCollection __nameSearchCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public NameSearchCollection NameSearchCollection
        {
            get
            {
                if (__nameSearchCollection == null) __nameSearchCollection = new NameSearchCollection();
                return __nameSearchCollection;
            }
            set { __nameSearchCollection = value; }
        }

        [JsonProperty(PropertyName = "searchOperator")]
        [XmlElement(ElementName = "searchOperator", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "searchOperator")]
        public FilterSearchOperator __searchOperator;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __searchOperatorSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public FilterSearchOperator SearchOperator
        {
            get { return __searchOperator; }
            set { __searchOperator = value; __searchOperatorSpecified = true; }
        }
    }
}
