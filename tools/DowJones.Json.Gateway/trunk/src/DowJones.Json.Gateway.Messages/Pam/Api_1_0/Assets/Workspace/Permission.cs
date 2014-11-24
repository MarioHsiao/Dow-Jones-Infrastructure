using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "Permission")]
    [XmlType(TypeName = "Permission", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class Permission
    {
        [JsonProperty(PropertyName = "scope")]
        [XmlElement(ElementName = "scope", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "scope")]
        public ShareScope __scope;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __scopeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ShareScope Scope
        {
            get { return __scope; }
            set { __scope = value; __scopeSpecified = true; }
        }

        [JsonProperty(PropertyName = "roles")]
        [XmlElement(Type = typeof(ShareRole), ElementName = "roles", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "roles")]
        public RolesCollection __rolesCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public RolesCollection RolesCollection
        {
            get
            {
                if (__rolesCollection == null) __rolesCollection = new RolesCollection();
                return __rolesCollection;
            }
            set { __rolesCollection = value; }
        }

        [JsonProperty(PropertyName = "groups")]
        [XmlElement(Type = typeof(GroupList), ElementName = "groups", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "groups")]
        public GroupList __groups;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public GroupList Groups
        {
            get
            {
                if (__groups == null) __groups = new GroupList();
                return __groups;
            }
            set { __groups = value; }
        }

        public Permission()
        {
        }
    }
}
