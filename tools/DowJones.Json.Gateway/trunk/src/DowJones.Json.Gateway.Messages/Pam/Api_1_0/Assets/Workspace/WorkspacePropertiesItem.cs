using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "WorkspacePropertiesItem")]
    [XmlType(TypeName = "WorkspacePropertiesItem", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [XmlInclude(typeof(ManualWorkspaceProperties))]
    [XmlInclude(typeof(AutomaticWorkspaceProperties))]
    [XmlInclude(typeof(CompliantWorkspacePropertiesItem))]
    public class WorkspacePropertiesItem
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

        [JsonProperty(PropertyName = "properties")]
        [XmlElement(Type = typeof(WorkspaceProperties), ElementName = "properties", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "properties")]
        public WorkspaceProperties __properties;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public WorkspaceProperties Properties
        {
            get { return __properties; }
            set { __properties = value; }
        }

        [JsonProperty(PropertyName = "totalNumberOfDisseminatedAssets")]
        [XmlElement(ElementName = "totalNumberOfDisseminatedAssets", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "totalNumberOfDisseminatedAssets")]
        public int __totalNumberOfDisseminatedAssets;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __totalNumberOfDisseminatedAssetsSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public int TotalNumberOfDisseminatedAssets
        {
            get { return __totalNumberOfDisseminatedAssets; }
            set { __totalNumberOfDisseminatedAssets = value; __totalNumberOfDisseminatedAssetsSpecified = true; }
        }

        [JsonProperty(PropertyName = "numberOfItems")]
        [XmlElement(ElementName = "numberOfItems", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "numberOfItems")]
        public int __numberOfItems;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __numberOfItemsSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public int NumberOfItems
        {
            get { return __numberOfItems; }
            set { __numberOfItems = value; __numberOfItemsSpecified = true; }
        }

        [JsonProperty(PropertyName = "DisseminatedAsset")]
        [XmlElement(Type = typeof(DisseminatedAsset), ElementName = "DisseminatedAsset", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "DisseminatedAsset")]
        public DisseminatedAssetCollection __DisseminatedAssetCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DisseminatedAssetCollection DisseminatedAssetCollection
        {
            get
            {
                if (__DisseminatedAssetCollection == null) __DisseminatedAssetCollection = new DisseminatedAssetCollection();
                return __DisseminatedAssetCollection;
            }
            set { __DisseminatedAssetCollection = value; }
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
    }
}
