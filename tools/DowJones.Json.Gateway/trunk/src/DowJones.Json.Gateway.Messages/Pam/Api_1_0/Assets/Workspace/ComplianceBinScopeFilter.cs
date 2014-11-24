using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "ComplianceBinScopeFilter")]
    [XmlType(TypeName = "ComplianceBinScopeFilter", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class ComplianceBinScopeFilter : Filter
    {
        [JsonProperty(PropertyName = "scope")]
        [XmlElement(ElementName = "scope", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "scope")]
        public ComplianceBinScope __scope = ComplianceBinScope.Personal;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __scopeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ComplianceBinScope Scope
        {
            get { return __scope; }
            set { __scope = value; __scopeSpecified = true; }
        }
    }
}
