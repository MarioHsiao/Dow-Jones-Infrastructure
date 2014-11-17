using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "SeparatorItem")]
    [XmlType(TypeName = "SeparatorItem", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class SeparatorItem : Item
    {

    }
}
