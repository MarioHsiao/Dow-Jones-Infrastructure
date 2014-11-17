using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "CompliantWorkspacePropertiesItem")]
    [Serializable]
    [XmlType(TypeName = "CompliantWorkspacePropertiesItem", Namespace = Declarations.SchemaVersion)]
    public class CompliantWorkspacePropertiesItem : WorkspacePropertiesItem
    {
        [JsonProperty(PropertyName = "complianceStatus")]
        [XmlElement("complianceStatus")]
        [DataMember(Name = "ComplianceStatus")]
        public ComplianceStatus ComplianceStatus { get; set; }
    }
}
