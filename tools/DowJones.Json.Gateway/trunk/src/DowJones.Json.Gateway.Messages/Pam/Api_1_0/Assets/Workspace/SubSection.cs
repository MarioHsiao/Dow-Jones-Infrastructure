using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "SubSection")]
    [XmlType(TypeName = "SubSection", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class SubSection : Section
    {
        public SubSection()
        {
        }
    }
}
