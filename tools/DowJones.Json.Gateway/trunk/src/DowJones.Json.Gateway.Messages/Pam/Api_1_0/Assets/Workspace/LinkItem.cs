using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "LinkItem")]
    [XmlType(TypeName = "LinkItem", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class LinkItem : ContentItem
    {
        [JsonProperty(PropertyName = "publicationDate")]
        [XmlElement(ElementName = "publicationDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "publicationDate")]
        public DateTime __publicationDate;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __publicationDateSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime PublicationDate
        {
            get { return __publicationDate; }
            set { __publicationDate = value; __publicationDateSpecified = true; }
        }

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime PublicationDateUtc
        {
            get { return __publicationDate.ToUniversalTime(); }
            set { __publicationDate = value.ToLocalTime(); __publicationDateSpecified = true; }
        }

        [JsonProperty(PropertyName = "author")]
        [XmlElement(ElementName = "author", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "author")]
        public string __author;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Author
        {
            get { return __author; }
            set { __author = value; }
        }

        [JsonProperty(PropertyName = "type")]
        [XmlElement(ElementName = "type", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "type")]
        public LinkType __type;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __typeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public LinkType Type
        {
            get { return __type; }
            set { __type = value; __typeSpecified = true; }
        }

        [JsonProperty(PropertyName = "uri")]
        [XmlElement(ElementName = "uri", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "uri")]
        public string __uri;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Uri
        {
            get { return __uri; }
            set { __uri = value; }
        }

        [JsonProperty(PropertyName = "description")]
        [XmlElement(ElementName = "description", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "description")]
        public string __description;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Description
        {
            get { return __description; }
            set { __description = value; }
        }

        [JsonProperty(PropertyName = "title")]
        [XmlElement(ElementName = "title", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "title")]
        public string __title;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Title
        {
            get { return __title; }
            set { __title = value; }
        }

        [JsonProperty(PropertyName = "sourceName")]
        [XmlElement(ElementName = "sourceName", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "sourceName")]
        public string __sourceName;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string sourceName
        {
            get { return __sourceName; }
            set { __sourceName = value; }
        }

        [JsonProperty(PropertyName = "language")]
        [XmlElement(ElementName = "language", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "language")]
        public string __language;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string language
        {
            get { return __language; }
            set { __language = value; }
        }


        public LinkItem()
        {
            __publicationDate = DateTime.MinValue;
        }

    }
}
