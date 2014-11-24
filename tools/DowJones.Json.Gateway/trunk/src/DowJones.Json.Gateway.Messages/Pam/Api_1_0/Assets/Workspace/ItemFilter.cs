using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonConverter(typeof(ItemFilter.MyCustomConverter))]
    [JsonObject(Title = "ItemFilter")]
    [XmlType(TypeName = "ItemFilter", Namespace = Declarations.SchemaVersion), Serializable]
    [XmlInclude(typeof(ItemDateFilter))]
    [KnownType(typeof(ItemDateFilter))]
    public abstract class ItemFilter
    {
        private class MyCustomConverter : JsonCreationConverter<ItemFilter>
        {
            protected override ItemFilter Create(Type objectType,
                Newtonsoft.Json.Linq.JObject jObject)
            {
                if ("ItemDateFilter".Equals(jObject.Value<string>("$type")))
                    return new ItemDateFilter();
                else
                    return null;
            }
        }
    }
}
