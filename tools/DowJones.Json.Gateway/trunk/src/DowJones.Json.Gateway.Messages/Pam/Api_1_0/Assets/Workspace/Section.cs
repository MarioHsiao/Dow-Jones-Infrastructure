using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "Section")]
    [XmlType(TypeName = "Section", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [XmlInclude(typeof(SeparatorItem))]
    [XmlInclude(typeof(ContentItem))]
    public class Section
    {
        [JsonProperty(PropertyName = "id")]
        [XmlElement(ElementName = "id", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "long", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "id")]
        public long __id;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __idSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public long Id
        {
            get { return __id; }
            set { __id = value; __idSpecified = true; }
        }

        [JsonProperty(PropertyName = "position")]
        [XmlElement(ElementName = "position", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "position")]
        public int __position;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __positionSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public int Position
        {
            get { return __position; }
            set { __position = value; __positionSpecified = true; }
        }

        [JsonProperty(PropertyName = "name")]
        [XmlElement(ElementName = "name", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "name")]
        public string __name;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Name
        {
            get { return __name; }
            set { __name = value; }
        }

        [JsonProperty(PropertyName = "Item")]
        [XmlElement(Type = typeof(Item), ElementName = "Item", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "Item")]
        public ItemCollection __itemCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ItemCollection ItemCollection
        {
            get
            {
                if (__itemCollection == null) __itemCollection = new ItemCollection();
                return __itemCollection;
            }
            set { __itemCollection = value; }
        }

        [JsonProperty(PropertyName = "tag")]
        [XmlElement(Type = typeof(string), ElementName = "tag", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "tag")]
        public TagCollection __tagCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public TagCollection TagCollection
        {
            get
            {
                if (__tagCollection == null) __tagCollection = new TagCollection();
                return __tagCollection;
            }
            set { __tagCollection = value; }
        }

        [JsonProperty(PropertyName = "element")]
        [XmlElement(Type = typeof(Element), ElementName = "element", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "element")]
        public ElementCollection __elementCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ElementCollection ElementCollection
        {
            get
            {
                if (__elementCollection == null) __elementCollection = new ElementCollection();
                return __elementCollection;
            }
            set { __elementCollection = value; }
        }

        [JsonProperty(PropertyName = "subSection")]
        [XmlElement(Type = typeof(SubSection), ElementName = "subSection", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "subSection")]
        public SubSectionCollection __subSectionCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public SubSectionCollection SubSectionCollection
        {
            get
            {
                if (__subSectionCollection == null) __subSectionCollection = new SubSectionCollection();
                return __subSectionCollection;
            }
            set { __subSectionCollection = value; }
        }

        public Section()
        {

        }
    }
}
