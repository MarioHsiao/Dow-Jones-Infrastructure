using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "CollectionMetadata")]
    [XmlType(TypeName = "CollectionMetadata", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class CollectionMetadata
    {
        [JsonProperty(PropertyName = "metaDataCode")]
        [XmlElement(Type = typeof(string), ElementName = "metaDataCode", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "metaDataCode")]
        public MetaDataCodeCollection __metaDataCodeCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public MetaDataCodeCollection MetaDataCodeCollection
        {
            get
            {
                if (__metaDataCodeCollection == null) __metaDataCodeCollection = new MetaDataCodeCollection();
                return __metaDataCodeCollection;
            }
            set { __metaDataCodeCollection = value; }
        }

        public CollectionMetadata()
        {
        }
    }
}
