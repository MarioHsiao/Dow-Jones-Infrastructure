using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "AutomaticWorkspace")]
    [XmlType(TypeName = "AutomaticWorkspace", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class AutomaticWorkspace : Workspace
    {
        [JsonProperty(PropertyName = "properties")]
        [XmlElement(Type = typeof(AutomaticWorkspaceProperties), ElementName = "properties", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "properties")]
        public AutomaticWorkspaceProperties __properties;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public AutomaticWorkspaceProperties Properties
        {
            get
            {
                if (__properties == null) __properties = new AutomaticWorkspaceProperties();
                return __properties;
            }
            set { __properties = value; }
        }

        [JsonProperty(PropertyName = "item")]
        [XmlElement(Type = typeof(ContentItem), ElementName = "item", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "item")]
        public ContentItemCollection __itemsCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ContentItemCollection ItemsCollection
        {
            get
            {
                if (__itemsCollection == null) __itemsCollection = new ContentItemCollection();
                return __itemsCollection;
            }
            set { __itemsCollection = value; }
        }
    }
}
