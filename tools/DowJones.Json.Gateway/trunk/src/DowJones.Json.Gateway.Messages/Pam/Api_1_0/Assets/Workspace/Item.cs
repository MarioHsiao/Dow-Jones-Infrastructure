using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonConverter(typeof(Item.MyCustomConverter))]
    [JsonObject(Title = "Item")]
    [XmlType(TypeName = "Item", Namespace = Declarations.SchemaVersion), Serializable]
    [XmlInclude(typeof(ContentItem))]
    [XmlInclude(typeof(ArticleItem))]
    [XmlInclude(typeof(ImageItem))]
    [XmlInclude(typeof(LinkItem))]
    [XmlInclude(typeof(SeparatorItem))]
    [KnownType(typeof(ContentItem))]
    [KnownType(typeof(ArticleItem))]
    [KnownType(typeof(ImageItem))]
    [KnownType(typeof(LinkItem))]
    [KnownType(typeof(SeparatorItem))]
    public abstract class Item
    {
        [JsonProperty(PropertyName = "id")]
        [XmlElement(ElementName = "id", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "long", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "id")]
        public long __id;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __idSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public long Id
        {
            get { return __id; }
            set { __id = value; __idSpecified = true; }
        }

        [JsonProperty(PropertyName = "position")]
        [XmlElement(ElementName = "position", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "position")]
        public int __position;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __positionSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public int Position
        {
            get { return __position; }
            set { __position = value; __positionSpecified = true; }
        }

        [JsonProperty(PropertyName = "isPublished")]
        [XmlElement(ElementName = "isPublished", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "isPublished")]
        public bool __isPublished;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __isPublishedSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool IsPublished
        {
            get { return __isPublished; }
            set { __isPublished = value; __isPublishedSpecified = true; }
        }

        [JsonProperty(PropertyName = "status")]
        [XmlElement(ElementName = "status", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "status")]
        public int __status;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __statusSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public int Status
        {
            get { return __status; }
            set { __status = value; __statusSpecified = true; }
        }

        [JsonProperty(PropertyName = "creationDate")]
        [XmlElement(ElementName = "creationDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "creationDate")]
        public DateTime __creationDate;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __creationDateSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime CreationDate
        {
            get { return __creationDate; }
            set { __creationDate = value; __creationDateSpecified = true; }
        }

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime CreationDateUtc
        {
            get { return __creationDate.ToUniversalTime(); }
            set { __creationDate = value.ToLocalTime(); __creationDateSpecified = true; }
        }

        [JsonProperty(PropertyName = "createdBy")]
        [XmlElement(ElementName = "createdBy", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "createdBy")]
        public string __createdBy;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string CreatedBy
        {
            get { return __createdBy; }
            set { __createdBy = value; }
        }

        [JsonProperty(PropertyName = "createdByNamespace")]
        [XmlElement(ElementName = "createdByNamespace", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "createdByNamespace")]
        public string __createdByNamespace;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string CreatedByNamespace
        {
            get { return __createdByNamespace; }
            set { __createdByNamespace = value; }
        }

        [JsonProperty(PropertyName = "createdByAccountId")]
        [XmlElement(ElementName = "createdByAccountId", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "createdByAccountId")]
        public string __createdByAccountId;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string CreatedByAccountId
        {
            get { return __createdByAccountId; }
            set { __createdByAccountId = value; }
        }

        [JsonProperty(PropertyName = "createdByIsActive")]
        [XmlElement(ElementName = "createdByIsActive", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "createdByIsActive")]
        public bool __createdByIsActive;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __createdByIsActiveSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool CreatedByIsActive
        {
            get { return __createdByIsActive; }
            set { __createdByIsActive = value; __createdByIsActiveSpecified = true; }
        }

        [JsonProperty(PropertyName = "lastModifiedDate")]
        [XmlElement(ElementName = "lastModifiedDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "lastModifiedDate")]
        public DateTime __lastModifiedDate;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __lastModifiedDateSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime LastModifiedDate
        {
            get { return __lastModifiedDate; }
            set { __lastModifiedDate = value; __lastModifiedDateSpecified = true; }
        }

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime LastModifiedDateUtc
        {
            get { return __lastModifiedDate.ToUniversalTime(); }
            set { __lastModifiedDate = value.ToLocalTime(); __lastModifiedDateSpecified = true; }
        }

        [JsonProperty(PropertyName = "lastModifiedBy")]
        [XmlElement(ElementName = "lastModifiedBy", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "lastModifiedBy")]
        public string __lastModifiedBy;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string LastModifiedBy
        {
            get { return __lastModifiedBy; }
            set { __lastModifiedBy = value; }
        }

        [JsonProperty(PropertyName = "lastModifiedByNamespace")]
        [XmlElement(ElementName = "lastModifiedByNamespace", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "lastModifiedByNamespace")]
        public string __lastModifiedByNamespace;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string LastModifiedByNamespace
        {
            get { return __lastModifiedByNamespace; }
            set { __lastModifiedByNamespace = value; }
        }

        [JsonProperty(PropertyName = "lastModifiedByAccountId")]
        [XmlElement(ElementName = "lastModifiedByAccountId", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "lastModifiedByAccountId")]
        public string __lastModifiedByAccountId;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string LastModifiedByAccountId
        {
            get { return __lastModifiedByAccountId; }
            set { __lastModifiedByAccountId = value; }
        }

        [JsonProperty(PropertyName = "lastModifiedByIsActive")]
        [XmlElement(ElementName = "lastModifiedByIsActive", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "lastModifiedByIsActive")]
        public bool __lastModifiedByIsActive;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __lastModifiedByIsActiveSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool LastModifiedByIsActive
        {
            get { return __lastModifiedByIsActive; }
            set { __lastModifiedByIsActive = value; __lastModifiedByIsActiveSpecified = true; }
        }

        [JsonProperty(PropertyName = "tag")]
        [XmlElement(Type = typeof(string), ElementName = "tag", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "tag")]
        public TagCollection __tagCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public TagCollection TagCollection
        {
            get
            {
                if (__tagCollection == null) __tagCollection = new TagCollection();
                return __tagCollection;
            }
            set { __tagCollection = value; }
        }

        private class MyCustomConverter : JsonCreationConverter<Item>
        {
            protected override Item Create(Type objectType,
                Newtonsoft.Json.Linq.JObject jObject)
            {
                if ("ArticleItem".Equals(jObject.Value<string>("$type")))
                    return new ArticleItem();
                else if ("ImageItem".Equals(jObject.Value<string>("$type")))
                    return new ImageItem();
                else if ("LinkItem".Equals(jObject.Value<string>("$type")))
                    return new LinkItem();
                else if ("SeparatorItem".Equals(jObject.Value<string>("$type")))
                    return new SeparatorItem();
                else
                    return null;
            }
        }
    }
}
