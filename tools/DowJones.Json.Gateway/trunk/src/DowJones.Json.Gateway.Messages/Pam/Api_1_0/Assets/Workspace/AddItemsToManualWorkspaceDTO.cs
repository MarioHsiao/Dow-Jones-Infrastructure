using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "AddItemsToManualWorkspaceDTO")]
    [XmlType(TypeName = "AddItemsToManualWorkspaceDTO", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class AddItemsToManualWorkspaceDTO : AddItemsToWorkspaceDTO
    {
        [JsonProperty(PropertyName = "section")]
        [XmlElement(Type = typeof(Section), ElementName = "section", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "section")]
        public SectionCollection __sectionCollection;

        [JsonIgnore]
        [IgnoreDataMember]
        public SectionCollection SectionCollection
        {
            get
            {
                if (__sectionCollection == null) __sectionCollection = new SectionCollection();
                return __sectionCollection;
            }
            set { __sectionCollection = value; }
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
