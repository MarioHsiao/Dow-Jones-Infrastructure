using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "ArticleItem")]
    [XmlType(TypeName = "ArticleItem", Namespace = Declarations.SchemaVersion), Serializable]
    [XmlInclude(typeof(CompliantArticleItem))]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class ArticleItem : ContentItem
    {
        [JsonProperty(PropertyName = "accessionNumber")]
        [XmlElement(ElementName = "accessionNumber", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "accessionNumber")]
        public string __accessionNumber;

        [JsonIgnore]
        [IgnoreDataMember]
        public string AccessionNumber
        {
            get { return __accessionNumber; }
            set { __accessionNumber = value; }
        }

        [JsonProperty(PropertyName = "contentCategory")]
        [XmlAttribute(AttributeName = "contentCategory", Form = XmlSchemaForm.Unqualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "contentCategory")]
        public ContentCategory __contentCategory;

        [JsonIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __contentCategorySpecified;

        [JsonIgnore]
        [IgnoreDataMember]
        public ContentCategory ContentCategory
        {
            get { return __contentCategory; }
            set { __contentCategory = value; __contentCategorySpecified = true; }
        }

        [JsonProperty(PropertyName = "providerId")]
        [XmlElement(ElementName = "providerId", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "providerId")]
        public string __providerId;

        [JsonIgnore]
        [IgnoreDataMember]
        public string ProviderId
        {
            get { return __providerId; }
            set { __providerId = value; }
        }

        [JsonProperty(PropertyName = "providerType")]
        [XmlElement(ElementName = "providerType", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "providerType")]
        public string __providerType;

        [JsonIgnore]
        [IgnoreDataMember]
        public string ProviderType
        {
            get { return __providerType; }
            set { __providerType = value; }
        }

    }
}
