using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "FeedAssets")]
    [XmlType(TypeName = "FeedAssets", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class FeedAssets
    {
        [JsonProperty(PropertyName = "feedAsset")]
        [XmlElement(Type = typeof(FeedAsset), ElementName = "feedAsset", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "feedAsset")]
        public FeedAssetCollection __feedAssetCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public FeedAssetCollection FeedAssetCollection
        {
            get
            {
                if (__feedAssetCollection == null) __feedAssetCollection = new FeedAssetCollection();
                return __feedAssetCollection;
            }
            set { __feedAssetCollection = value; }
        }

        public FeedAssets()
        {
        }
    }
}
