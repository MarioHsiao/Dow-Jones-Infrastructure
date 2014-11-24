using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "GroupIdList")]
    [XmlType(TypeName = "GroupIdList", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class GroupIdList
    {
        [JsonProperty(PropertyName = "type")]
        [XmlAttribute(AttributeName = "type", Form = XmlSchemaForm.Unqualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "type")]
        public GroupListType __type;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __typeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public GroupListType Type
        {
            get { return __type; }
            set { __type = value; __typeSpecified = true; }
        }

        [JsonProperty(PropertyName = "id")]
        [XmlElement(Type = typeof(string), ElementName = "id", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "id")]
        public IdCollection __idCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public IdCollection IdCollection
        {
            get
            {
                if (__idCollection == null) __idCollection = new IdCollection();
                return __idCollection;
            }
            set { __idCollection = value; }
        }

        public GroupIdList()
        {
        }
    }
}
