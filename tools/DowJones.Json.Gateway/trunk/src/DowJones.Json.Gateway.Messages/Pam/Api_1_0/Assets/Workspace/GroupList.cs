using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "GroupList")]
    [XmlType(TypeName = "GroupList", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class GroupList
    {
        [JsonProperty(PropertyName = "groupIdList")]
        [XmlElement(Type = typeof(GroupIdList), ElementName = "groupIdList", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "groupIdList")]
        public GroupIdListCollection __groupIdListCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public GroupIdListCollection GroupIdListCollection
        {
            get
            {
                if (__groupIdListCollection == null) __groupIdListCollection = new GroupIdListCollection();
                return __groupIdListCollection;
            }
            set { __groupIdListCollection = value; }
        }

        public GroupList()
        {
        }
    }
}
