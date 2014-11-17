using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonConverter(typeof(Workspace.MyCustomConverter))]
    [JsonObject(Title = "Workspace")]
    [XmlType(TypeName = "Workspace", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [XmlInclude(typeof(AutomaticWorkspace))]
    [XmlInclude(typeof(ManualWorkspace))]
    [KnownType("AutomaticWorkspace")]
    [KnownType("ManualWorkspace")]
    public abstract class Workspace
    {
        [JsonProperty(PropertyName = "id")]
        [XmlElement(ElementName = "id", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "long", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "id")]
        public long __id;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __idSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public long Id
        {
            get { return __id; }
            set { __id = value; __idSpecified = true; }
        }

        [JsonProperty(PropertyName = "segment")]
        [XmlElement(ElementName = "segment", IsNullable = false, Form = XmlSchemaForm.Qualified, Type = typeof(Segment), Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "segment")]
        public Segment __segment;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Segment Segment
        {
            get { return __segment; }
            set { __segment = value; }
        }

        [JsonProperty(PropertyName = "product")]
        [XmlElement(ElementName = "product", IsNullable = false, Form = XmlSchemaForm.Qualified, Type = typeof(Product), Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "product")]
        public Product __product;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Product Product
        {
            get { return __product; }
            set { __product = value; }
        }

        [JsonProperty(PropertyName = "code")]
        [XmlElement(ElementName = "code", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "code")]
        public string __code;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Code
        {
            get { return __code; }
            set { __code = value; }
        }

        [JsonProperty(PropertyName = "shareProperties")]
        [XmlElement(Type = typeof(ShareProperties), ElementName = "shareProperties", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "shareProperties")]
        public ShareProperties __shareProperties;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ShareProperties ShareProperties
        {
            get
            {
                if (__shareProperties == null) __shareProperties = new ShareProperties();
                return __shareProperties;
            }
            set { __shareProperties = value; }
        }

        [JsonProperty(PropertyName = "publishedDate")]
        [XmlElement(ElementName = "publishedDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "publishedDate")]
        public DateTime __publishedDate;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __publishedDateSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime PublishedDate
        {
            get { return __publishedDate; }
            set { __publishedDate = value; __publishedDateSpecified = true; }
        }

        protected Workspace()
        {
            __segment = Segment.Unspecified;
            __product = Product.Unspecified;
        }

        private class MyCustomConverter : JsonCreationConverter<Workspace>
        {
            protected override Workspace Create(Type objectType,
                Newtonsoft.Json.Linq.JObject jObject)
            {
                if ("AutomaticWorkspace".Equals(jObject.Value<string>("$type")))
                    return new AutomaticWorkspace();
                else if ("ManualWorkspace".Equals(jObject.Value<string>("$type")))
                    return new ManualWorkspace();
                else
                    return null;
            }
        }
    }
}
