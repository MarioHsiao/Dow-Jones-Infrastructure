using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "Element")]
    [XmlType(TypeName = "Element", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class Element
    {
        [JsonProperty(PropertyName = "id")]
        [XmlElement(ElementName = "id", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "id")]
        public int __id;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __idSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public int Id
        {
            get { return __id; }
            set { __id = value; __idSpecified = true; }
        }

        [JsonProperty(PropertyName = "type")]
        [XmlElement(ElementName = "type", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "type")]
        public ElementType __type;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __typeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ElementType Type
        {
            get { return __type; }
            set { __type = value; __typeSpecified = true; }
        }

        [JsonProperty(PropertyName = "name")]
        [XmlElement(ElementName = "name", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "name")]
        public string __name;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Name
        {
            get { return __name; }
            set { __name = value; }
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

        [JsonProperty(PropertyName = "text")]
        [XmlElement(ElementName = "text", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "text")]
        public string __text;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Text
        {
            get { return __text; }
            set { __text = value; }
        }

        [JsonProperty(PropertyName = "imageType")]
        [XmlElement(ElementName = "imageType", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "imageType")]
        public ElementImageType __imageType;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __imageTypeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ElementImageType ImageType
        {
            get { return __imageType; }
            set { __imageType = value; __imageTypeSpecified = true; }
        }

        [JsonProperty(PropertyName = "url")]
        [XmlElement(ElementName = "url", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "url")]
        public string __url;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Url
        {
            get { return __url; }
            set { __url = value; }
        }

        [JsonProperty(PropertyName = "navigateUrl")]
        [XmlElement(ElementName = "navigateUrl", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "navigateUrl")]
        public string __navigateUrl;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string NavigateUrl
        {
            get { return __navigateUrl; }
            set { __navigateUrl = value; }
        }

        [JsonProperty(PropertyName = "imageID")]
        [XmlElement(ElementName = "imageID", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "imageID")]
        public int __imageID;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __imageIDSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public int ImageID
        {
            get { return __imageID; }
            set { __imageID = value; __imageIDSpecified = true; }
        }

        [JsonProperty(PropertyName = "styleName")]
        [XmlElement(ElementName = "styleName", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "styleName")]
        public string __styleName;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string StyleName
        {
            get { return __styleName; }
            set { __styleName = value; }
        }

        [JsonProperty(PropertyName = "imageSize")]
        [XmlElement(ElementName = "imageSize", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "imageSize")]
        public int __imageSize;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __imageSizeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public int ImageSize
        {
            get { return __imageSize; }
            set { __imageSize = value; __imageSizeSpecified = true; }
        }


        public Element()
        {
        }
    }
}
