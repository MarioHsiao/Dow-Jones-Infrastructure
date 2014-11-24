using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "ImageItem")]
    [XmlType(TypeName = "ImageItem", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class ImageItem : ContentItem
    {
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

        [JsonProperty(PropertyName = "postbackUri")]
        [XmlElement(ElementName = "postbackUri", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "postbackUri")]
        public string __postbackUri;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string PostbackUri
        {
            get { return __postbackUri; }
            set { __postbackUri = value; }
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

    }
}
