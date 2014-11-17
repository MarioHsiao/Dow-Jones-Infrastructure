using System;
using System.Xml.Serialization;
using DowJones.Json.Gateway.Interfaces;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace.Transactions
{
    [JsonObject(Title = "UpdateWorkspaceResponse")]
    [XmlRoot(ElementName = "UpdateWorkspaceResponse", Namespace = Declarations.SchemaVersion, IsNullable = false), Serializable]
    public class UpdateWorkspaceResponse : IJsonRestResponse
    {
        public UpdateWorkspaceResponse()
        {
        }
    }
}
