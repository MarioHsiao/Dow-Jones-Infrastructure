using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonConverter(typeof(ContentItem.MyCustomConverter))]
    [JsonObject(Title = "ContentItem")]
    [XmlType(TypeName = "ContentItem", Namespace = Declarations.SchemaVersion), Serializable]
    [XmlInclude(typeof(LinkItem))]
    [XmlInclude(typeof(ImageItem))]
    [XmlInclude(typeof(ArticleItem))]
    [KnownType(typeof(LinkItem))]
    [KnownType(typeof(ImageItem))]
    [KnownType(typeof(ArticleItem))]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public abstract class ContentItem : Item
    {
        [JsonProperty(PropertyName = "importance")]
        [XmlElement(ElementName = "importance", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "importance")]
        public Importance __importance;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __importanceSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Importance Importance
        {
            get { return __importance; }
            set { __importance = value; __importanceSpecified = true; }
        }

        [JsonProperty(PropertyName = "comment")]
        [XmlElement(ElementName = "comment", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "comment")]
        public string __comment;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Comment
        {
            get { return __comment; }
            set { __comment = value; }
        }

        [JsonProperty(PropertyName = "element")]
        [XmlElement(Type = typeof(Element), ElementName = "element", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "element")]
        public ElementCollection __elementCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ElementCollection ElementCollection
        {
            get
            {
                if (__elementCollection == null) __elementCollection = new ElementCollection();
                return __elementCollection;
            }
            set { __elementCollection = value; }
        }

        private class MyCustomConverter : JsonCreationConverter<ContentItem>
        {
            protected override ContentItem Create(Type objectType,
                Newtonsoft.Json.Linq.JObject jObject)
            {
                if ("LinkItem".Equals(jObject.Value<string>("$type")))
                    return new LinkItem();
                else if ("ImageItem".Equals(jObject.Value<string>("$type")))
                    return new ImageItem();
                else if ("ArticleItem".Equals(jObject.Value<string>("$type")))
                    return new ArticleItem();
                else
                    return null;
            }
        }
    }
}
