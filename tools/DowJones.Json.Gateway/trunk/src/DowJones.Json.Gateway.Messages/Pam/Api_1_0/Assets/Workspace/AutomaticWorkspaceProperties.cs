using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "AutomaticWorkspaceProperties")]
    [XmlType(TypeName = "AutomaticWorkspaceProperties", Namespace = Declarations.SchemaVersion), Serializable]
    public class AutomaticWorkspaceProperties : WorkspaceProperties
    {
    }
}
