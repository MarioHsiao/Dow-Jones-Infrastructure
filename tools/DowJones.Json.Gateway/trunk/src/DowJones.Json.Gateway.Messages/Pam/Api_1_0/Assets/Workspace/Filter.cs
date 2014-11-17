using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonConverter(typeof(Filter.MyCustomConverter))]
    [JsonObject(Title = "ContentItem")]
    [XmlType(TypeName = "Filter", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [XmlInclude(typeof(NameSearchFilter))]
    [XmlInclude(typeof(DataContentFilter))]
    [XmlInclude(typeof(DateFilter))]
    [XmlInclude(typeof(IdSearchFilter))]
    [XmlInclude(typeof(ComplianceBinScopeFilter))]
    [XmlInclude(typeof(ProductFilter))]
    [XmlInclude(typeof(SegmentFilter))]
    [XmlInclude(typeof(CollectionMetadataSearchFilter))]
    [KnownType(typeof(NameSearchFilter))]
    [KnownType(typeof(DataContentFilter))]
    [KnownType(typeof(DateFilter))]
    [KnownType(typeof(IdSearchFilter))]
    [KnownType(typeof(ComplianceBinScopeFilter))]
    [KnownType(typeof(ProductFilter))]
    [KnownType(typeof(SegmentFilter))]
    [KnownType(typeof(CollectionMetadataSearchFilter))]
    public abstract class Filter
    {
        private class MyCustomConverter : JsonCreationConverter<Filter>
        {
            protected override Filter Create(Type objectType,
                Newtonsoft.Json.Linq.JObject jObject)
            {
                if ("NameSearchFilter".Equals(jObject.Value<string>("$type")))
                    return new NameSearchFilter();
                else if ("DataContentFilter".Equals(jObject.Value<string>("$type")))
                    return new DataContentFilter();
                else if ("DateFilter".Equals(jObject.Value<string>("$type")))
                    return new DateFilter();
                else if ("IdSearchFilter".Equals(jObject.Value<string>("$type")))
                    return new IdSearchFilter();
                else if ("ComplianceBinScopeFilter".Equals(jObject.Value<string>("$type")))
                    return new ComplianceBinScopeFilter();
                else if ("ProductFilter".Equals(jObject.Value<string>("$type")))
                    return new ProductFilter();
                else if ("SegmentFilter".Equals(jObject.Value<string>("$type")))
                    return new SegmentFilter();
                else if ("CollectionMetadataSearchFilter".Equals(jObject.Value<string>("$type")))
                    return new CollectionMetadataSearchFilter();
                else
                    return null;
            }
        }
    }
}
