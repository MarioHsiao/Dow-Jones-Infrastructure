using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "CompliantArticleItem")]
    [Serializable]
    [XmlType(TypeName = "CompliantArticleItem", Namespace = Declarations.SchemaVersion)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class CompliantArticleItem : ArticleItem
    {
        [JsonProperty(PropertyName = "complianceStatus")]
        [XmlElement("complianceStatus")]
        [DataMember(Name = "ComplianceStatus")]
        public ComplianceStatus ComplianceStatus { get; set; }
    }
}
