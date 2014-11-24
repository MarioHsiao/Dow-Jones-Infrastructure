using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Json.Gateway.Interfaces;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace.Transactions
{
    [JsonObject(Title = "GetWorkspacesDetailsListResponse")]
    [XmlRoot(ElementName = "GetWorkspacesDetailsListResponse", Namespace = Declarations.SchemaVersion, IsNullable = false), Serializable]
    [DataContract(Name = "GetWorkspacesDetailsListResponse", Namespace = "")]
    public class GetWorkspacesDetailsListResponse : IJsonRestResponse
    {
        [JsonProperty(PropertyName = "workspace")]
        [XmlElement(Type = typeof(Workspace), ElementName = "workspace", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "workspace")]
        public WorkspaceCollection __workspaceCollection;

        [JsonIgnore]
        [XmlIgnore]
        public WorkspaceCollection WorkspaceCollection
        {
            get
            {
                if (__workspaceCollection == null) __workspaceCollection = new WorkspaceCollection();
                return __workspaceCollection;
            }
            set { __workspaceCollection = value; }
        }
    }
}
