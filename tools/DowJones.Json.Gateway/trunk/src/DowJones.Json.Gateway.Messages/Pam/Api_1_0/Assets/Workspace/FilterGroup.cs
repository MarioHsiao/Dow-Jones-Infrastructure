using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "FilterGroup")]
    [XmlType(TypeName = "FilterGroup", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [XmlInclude(typeof(DataContentFilter))]
    [XmlInclude(typeof(IdSearchFilter))]
    [XmlInclude(typeof(NameSearchFilter))]
    [XmlInclude(typeof(DateFilter))]
    public class FilterGroup
    {
        [JsonProperty(PropertyName = "filter")]
        [XmlElement(Type = typeof(Filter), ElementName = "filter", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "filter")]
        public FilterCollection __filterCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public FilterCollection FilterCollection
        {
            get
            {
                if (__filterCollection == null) __filterCollection = new FilterCollection();
                return __filterCollection;
            }
            set { __filterCollection = value; }
        }

        [JsonProperty(PropertyName = "operator")]
        [XmlElement(ElementName = "operator", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "operator")]
        public FilterOperator __operator;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __operatorSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public FilterOperator Operator
        {
            get { return __operator; }
            set { __operator = value; __operatorSpecified = true; }
        }
    }
}
