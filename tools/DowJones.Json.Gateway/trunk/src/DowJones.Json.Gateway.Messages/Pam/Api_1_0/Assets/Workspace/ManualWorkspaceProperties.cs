using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonConverter(typeof(ManualWorkspaceProperties.MyCustomConverter))]
    [JsonObject(Title = "ManualWorkspaceProperties")]
    [XmlType(TypeName = "ManualWorkspaceProperties", Namespace = Declarations.SchemaVersion), Serializable]
    //[XmlInclude(typeof(NewsletterWorkspaceProperties))]
    [XmlInclude(typeof(CollectionWorkspaceProperties))]
    //[KnownType(typeof(NewsletterWorkspaceProperties))]
    [KnownType(typeof(CollectionWorkspaceProperties))]
    public abstract class ManualWorkspaceProperties : WorkspaceProperties
    {
        private class MyCustomConverter : JsonCreationConverter<ManualWorkspaceProperties>
        {
            protected override ManualWorkspaceProperties Create(Type objectType,
                Newtonsoft.Json.Linq.JObject jObject)
            {
                if ("NewsletterWorkspaceProperties".Equals(jObject.Value<string>("$type")))
                    return new NewsletterWorkspaceProperties();
                else if ("CollectionWorkspaceProperties".Equals(jObject.Value<string>("$type")))
                    return new CollectionWorkspaceProperties();
                else
                    return null;
            }
        }
    }
}
