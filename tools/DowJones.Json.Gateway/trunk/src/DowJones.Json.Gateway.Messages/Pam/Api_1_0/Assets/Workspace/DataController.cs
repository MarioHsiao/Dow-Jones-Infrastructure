using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "DataController")]
    [XmlType(TypeName = "DataController", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class DataController
    {
        [JsonProperty(PropertyName = "maxSectionsToReturn")]
        [XmlElement(ElementName = "maxSectionsToReturn", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "maxSectionsToReturn")]
        public string __maxSectionsToReturn;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string MaxSectionsToReturn
        {
            get { return __maxSectionsToReturn; }
            set { __maxSectionsToReturn = value; }
        }

        [JsonProperty(PropertyName = "maxItemsToReturn")]
        [XmlElement(ElementName = "maxItemsToReturn", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string __maxItemsToReturn;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string MaxItemsToReturn
        {
            get { return __maxItemsToReturn; }
            set { __maxItemsToReturn = value; }
        }
    }
}
