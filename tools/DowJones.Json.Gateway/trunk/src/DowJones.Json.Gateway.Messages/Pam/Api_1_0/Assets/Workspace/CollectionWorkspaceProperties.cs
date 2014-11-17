using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "CollectionWorkspaceProperties")]
    [DataContract(Name = "CollectionWorkspaceProperties", Namespace = "")]
    [XmlType(TypeName = "CollectionWorkspaceProperties", Namespace = Declarations.SchemaVersion), Serializable]
    public class CollectionWorkspaceProperties : ManualWorkspaceProperties
    {
        [JsonProperty(PropertyName = "collectionMetadata")]
        [XmlElement(Type = typeof(CollectionMetadata), ElementName = "collectionMetadata", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "collectionMetadata")]
        public List<string> __collectionMetadata;

        [JsonIgnore]
        [IgnoreDataMember]
        public List<string> CollectionMetadata
        {
            get
            {
                if (__collectionMetadata == null) __collectionMetadata = new List<string>();
                return __collectionMetadata;
            }
            set { __collectionMetadata = value; }
        }

        [JsonProperty(PropertyName = "maxItemsCount")]
        [XmlElement(ElementName = "maxItemsCount", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "maxItemsCount")]
        public int __maxItemsCount;

        [JsonIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __maxItemsCountSpecified;

        [JsonIgnore]
        [IgnoreDataMember]
        public int MaxItemsCount
        {
            get { return __maxItemsCount; }
            set { __maxItemsCount = value; __maxItemsCountSpecified = true; }
        }
    }
}
