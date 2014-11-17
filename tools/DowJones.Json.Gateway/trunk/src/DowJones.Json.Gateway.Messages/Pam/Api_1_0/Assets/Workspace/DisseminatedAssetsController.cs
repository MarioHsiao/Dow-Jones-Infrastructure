using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [XmlType(TypeName = "DisseminatedAssetsController", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class DisseminatedAssetsController
    {
        [JsonProperty(PropertyName = "maxAssetsToReturn")]
        [XmlElement(ElementName = "maxAssetsToReturn", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "maxAssetsToReturn")]
        public int __maxAssetsToReturn;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __maxAssetsToReturnSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public int MaxAssetsToReturn
        {
            get { return __maxAssetsToReturn; }
            set { __maxAssetsToReturn = value; __maxAssetsToReturnSpecified = true; }
        }

        [JsonProperty(PropertyName = "type")]
        [XmlElement(ElementName = "type", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "type")]
        public DisseminatedAssetType __type;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __typeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DisseminatedAssetType Type
        {
            get { return __type; }
            set { __type = value; __typeSpecified = true; }
        }

        [JsonProperty(PropertyName = "sortBy")]
        [XmlElement(ElementName = "sortBy", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "sortBy")]
        public DisseminatedAssetSortBy __sortBy;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __sortBySpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DisseminatedAssetSortBy SortBy
        {
            get { return __sortBy; }
            set { __sortBy = value; __sortBySpecified = true; }
        }

        [JsonProperty(PropertyName = "sortOrder")]
        [XmlElement(ElementName = "sortOrder", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "sortOrder")]
        public SortOrder __sortOrder;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __sortOrderSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public SortOrder sortOrder
        {
            get { return __sortOrder; }
            set { __sortOrder = value; __sortOrderSpecified = true; }
        }
    }
}
