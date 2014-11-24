using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "FeedAsset")]
    [XmlType(TypeName = "FeedAsset", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class FeedAsset
    {
        [JsonProperty(PropertyName = "assetId")]
        [XmlElement(Type = typeof(long), ElementName = "assetId", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "long", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "assetId")]
        public AssetIdCollection __assetIdCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public AssetIdCollection AssetIdCollection
        {
            get
            {
                if (__assetIdCollection == null) __assetIdCollection = new AssetIdCollection();
                return __assetIdCollection;
            }
            set { __assetIdCollection = value; }
        }

        [JsonProperty(PropertyName = "type")]
        [XmlElement(ElementName = "type", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "type")]
        public FeedAssetType __type;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __typeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public FeedAssetType Type
        {
            get { return __type; }
            set { __type = value; __typeSpecified = true; }
        }

        public FeedAsset()
        {
        }
    }
}
