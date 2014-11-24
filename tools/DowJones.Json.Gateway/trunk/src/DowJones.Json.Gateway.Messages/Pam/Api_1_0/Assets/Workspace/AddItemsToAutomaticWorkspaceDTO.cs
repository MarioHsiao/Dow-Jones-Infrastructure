using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "AddItemsToAutomaticWorkspaceDTO")]
    [XmlType(TypeName = "AddItemsToAutomaticWorkspaceDTO", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class AddItemsToAutomaticWorkspaceDTO : AddItemsToWorkspaceDTO
    {

        [JsonProperty(PropertyName = "item")]
        [XmlElement(Type = typeof(ContentItem), ElementName = "item", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "item")]
        public ContentItemCollection __itemCollection;

        [JsonIgnore]
        [IgnoreDataMember]
        public ContentItemCollection ItemCollection
        {
            get
            {
                if (__itemCollection == null) __itemCollection = new ContentItemCollection();
                return __itemCollection;
            }
            set { __itemCollection = value; }
        }

        [JsonProperty(PropertyName = "addAtPositionMode")]
        [XmlElement(ElementName = "addAtPositionMode", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "addAtPositionMode")]
        public AddAtPositionMode __addAtPositionMode;

        [JsonIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __addAtPositionModeSpecified;

        [JsonIgnore]
        [IgnoreDataMember]
        public AddAtPositionMode AddAtPositionMode
        {
            get { return __addAtPositionMode; }
            set { __addAtPositionMode = value; __addAtPositionModeSpecified = true; }
        }
    }
}
