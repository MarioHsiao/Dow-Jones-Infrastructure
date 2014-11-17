using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "ManualWorkspace")]
    [XmlType(TypeName = "ManualWorkspace", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [XmlInclude(typeof(CollectionWorkspaceProperties))]
    [XmlInclude(typeof(NewsletterWorkspaceProperties))]
    [DataContract(Name = "ManualWorkspace", Namespace = "")]
    public class ManualWorkspace : Workspace
    {
        [JsonProperty(PropertyName = "properties")]
        [XmlElement(Type = typeof(ManualWorkspaceProperties), ElementName = "properties", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "properties")]
        public ManualWorkspaceProperties __properties;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ManualWorkspaceProperties Properties
        {
            get { return __properties; }
            set { __properties = value; }
        }

        [JsonProperty(PropertyName = "section")]
        [XmlElement(Type = typeof(Section), ElementName = "section", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "section")]
        public SectionCollection __sectionCollection;

        [JsonIgnore]
        [XmlIgnore]
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

        [JsonProperty(PropertyName = "feedAssets")]
        [XmlElement(Type = typeof(FeedAssets), ElementName = "feedAssets", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "feedAssets")]
        public FeedAssets __feedAssets;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public FeedAssets FeedAssets
        {
            get
            {
                if (__feedAssets == null) __feedAssets = new FeedAssets();
                return __feedAssets;
            }
            set { __feedAssets = value; }
        }

        [JsonProperty(PropertyName = "deliveryInfo")]
        [XmlElement(Type = typeof(DeliveryInfo), ElementName = "deliveryInfo", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "deliveryInfo")]
        public DeliveryInfoCollection __deliveryInfoCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DeliveryInfoCollection DeliveryInfoCollection
        {
            get
            {
                if (__deliveryInfoCollection == null) __deliveryInfoCollection = new DeliveryInfoCollection();
                return __deliveryInfoCollection;
            }
            set { __deliveryInfoCollection = value; }
        }

    }
}
