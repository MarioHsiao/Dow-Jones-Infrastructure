using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "DataItemsCountController")]
    [XmlType(TypeName = "DataItemsCountController", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class DataItemsCountController
    {
        [JsonProperty(PropertyName = "maxSectionsToReturn")]
        [XmlElement(ElementName = "maxSectionsToReturn", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "maxSectionsToReturn")]
        public int __maxSectionsToReturn;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __maxSectionsToReturnSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public int MaxSectionsToReturn
        {
            get { return __maxSectionsToReturn; }
            set { __maxSectionsToReturn = value; __maxSectionsToReturnSpecified = true; }
        }

        [JsonProperty(PropertyName = "maxItemsToReturn")]
        [XmlElement(ElementName = "maxItemsToReturn", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "maxItemsToReturn")]
        public int __maxItemsToReturn;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __maxItemsToReturnSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public int MaxItemsToReturn
        {
            get { return __maxItemsToReturn; }
            set { __maxItemsToReturn = value; __maxItemsToReturnSpecified = true; }
        }
    }
}
