using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonConverter(typeof(WorkspaceProperties.MyCustomConverter))]
    [JsonObject(Title = "WorkspaceProperties")]
    [XmlType(TypeName = "WorkspaceProperties", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [XmlInclude(typeof(AutomaticWorkspaceProperties))]
    [XmlInclude(typeof(ManualWorkspaceProperties))]
    [XmlInclude(typeof(CollectionWorkspaceProperties))]
    [XmlInclude(typeof(NewsletterWorkspaceProperties))]
    [KnownType(typeof(AutomaticWorkspaceProperties))]
    [KnownType(typeof(ManualWorkspaceProperties))]
    [KnownType(typeof(CollectionWorkspaceProperties))]
    [KnownType(typeof(NewsletterWorkspaceProperties))]
    public abstract class WorkspaceProperties
    {
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

        [JsonProperty(PropertyName = "displayName")]
        [XmlElement(ElementName = "displayName", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "displayName")]
        public string __displayName;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string DisplayName
        {
            get { return __displayName; }
            set { __displayName = value; }
        }

        [JsonProperty(PropertyName = "description")]
        [XmlElement(ElementName = "description", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "description")]
        public string __description;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Description
        {
            get { return __description; }
            set { __description = value; }
        }

        [JsonProperty(PropertyName = "creationDate")]
        [XmlElement(ElementName = "creationDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "creationDate")]
        public DateTime __creationDate;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __creationDateSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime CreationDate
        {
            get { return __creationDate; }
            set { __creationDate = value; __creationDateSpecified = true; }
        }

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime CreationDateUtc
        {
            get { return __creationDate.ToUniversalTime(); }
            set { __creationDate = value.ToLocalTime(); __creationDateSpecified = true; }
        }

        [JsonProperty(PropertyName = "lastModifiedDate")]
        [XmlElement(ElementName = "lastModifiedDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "lastModifiedDate")]
        public DateTime __lastModifiedDate;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __lastModifiedDateSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime LastModifiedDate
        {
            get { return __lastModifiedDate; }
            set { __lastModifiedDate = value; __lastModifiedDateSpecified = true; }
        }

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime LastModifiedDateUtc
        {
            get { return __lastModifiedDate.ToUniversalTime(); }
            set { __lastModifiedDate = value.ToLocalTime(); __lastModifiedDateSpecified = true; }
        }

        [JsonProperty(PropertyName = "createdBy")]
        [XmlElement(ElementName = "createdBy", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "createdBy")]
        public string __createdBy;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string CreatedBy
        {
            get { return __createdBy; }
            set { __createdBy = value; }
        }

        [JsonProperty(PropertyName = "lastModifiedBy")]
        [XmlElement(ElementName = "lastModifiedBy", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "lastModifiedBy")]
        public string __lastModifiedBy;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string LastModifiedBy
        {
            get { return __lastModifiedBy; }
            set { __lastModifiedBy = value; }
        }

        [JsonProperty(PropertyName = "createdByNamespace")]
        [XmlElement(ElementName = "createdByNamespace", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "createdByNamespace")]
        public string __createdByNamespace;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string CreatedByNamespace
        {
            get { return __createdByNamespace; }
            set { __createdByNamespace = value; }
        }

        [JsonProperty(PropertyName = "lastModifiedByNamespace")]
        [XmlElement(ElementName = "lastModifiedByNamespace", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "lastModifiedByNamespace")]
        public string __lastModifiedByNamespace;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string LastModifiedByNamespace
        {
            get { return __lastModifiedByNamespace; }
            set { __lastModifiedByNamespace = value; }
        }

        [JsonProperty(PropertyName = "createdByAccountId")]
        [XmlElement(ElementName = "createdByAccountId", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "createdByAccountId")]
        public string __createdByAccountId;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string CreatedByAccountId
        {
            get { return __createdByAccountId; }
            set { __createdByAccountId = value; }
        }

        [JsonProperty(PropertyName = "lastModifiedByAccountId")]
        [XmlElement(ElementName = "lastModifiedByAccountId", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "lastModifiedByAccountId")]
        public string __lastModifiedByAccountId;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string LastModifiedByAccountId
        {
            get { return __lastModifiedByAccountId; }
            set { __lastModifiedByAccountId = value; }
        }

        [JsonProperty(PropertyName = "createdByIsActive")]
        [XmlElement(ElementName = "createdByIsActive", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "createdByIsActive")]
        public bool __createdByIsActive;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __createdByIsActiveSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool CreatedByIsActive
        {
            get { return __createdByIsActive; }
            set { __createdByIsActive = value; __createdByIsActiveSpecified = true; }
        }

        [JsonProperty(PropertyName = "lastModifiedByIsActive")]
        [XmlElement(ElementName = "lastModifiedByIsActive", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "lastModifiedByIsActive")]
        public bool __lastModifiedByIsActive;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __lastModifiedByIsActiveSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool LastModifiedByIsActive
        {
            get { return __lastModifiedByIsActive; }
            set { __lastModifiedByIsActive = value; __lastModifiedByIsActiveSpecified = true; }
        }

        [JsonProperty(PropertyName = "lastContentModifiedDate")]
        [XmlElement(ElementName = "lastContentModifiedDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "lastContentModifiedDate")]
        public DateTime __lastContentModifiedDate;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __lastContentModifiedDateSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime LastContentModifiedDate
        {
            get { return __lastContentModifiedDate; }
            set { __lastContentModifiedDate = value; __lastContentModifiedDateSpecified = true; }
        }

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime LastContentModifiedDateUtc
        {
            get { return __lastContentModifiedDate.ToUniversalTime(); }
            set { __lastContentModifiedDate = value.ToLocalTime(); __lastContentModifiedDateSpecified = true; }
        }

        [JsonProperty(PropertyName = "audience")]
        [XmlElement(Type = typeof(Audience), ElementName = "audience", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "audience")]
        public Audience __audience;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Audience Audience
        {
            get
            {
                if (__audience == null) __audience = new Audience();
                return __audience;
            }
            set { __audience = value; }
        }

        [JsonProperty(PropertyName = "emailDistributionId")]
        [XmlElement(ElementName = "emailDistributionId", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "emailDistributionId")]
        public int __emailDistributionId;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __emailDistributionIdSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public int EmailDistributionId
        {
            get { return __emailDistributionId; }
            set { __emailDistributionId = value; __emailDistributionIdSpecified = true; }
        }

        [JsonProperty(PropertyName = "areFeedsActive")]
        [XmlElement(ElementName = "areFeedsActive", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "areFeedsActive")]
        public bool __areFeedsActive;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __areFeedsActiveSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool AreFeedsActive
        {
            get { return __areFeedsActive; }
            set { __areFeedsActive = value; __areFeedsActiveSpecified = true; }
        }

        [JsonProperty(PropertyName = "hasRssFeed")]
        [XmlElement(ElementName = "hasRssFeed", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "hasRssFeed")]
        public bool __hasRssFeed;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __hasRssFeedSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool HasRssFeed
        {
            get { return __hasRssFeed; }
            set { __hasRssFeed = value; __hasRssFeedSpecified = true; }
        }

        [JsonProperty(PropertyName = "hasPodcastFeed")]
        [XmlElement(ElementName = "hasPodcastFeed", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "hasPodcastFeed")]
        public bool __hasPodcastFeed;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __hasPodcastFeedSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool HasPodcastFeed
        {
            get { return __hasPodcastFeed; }
            set { __hasPodcastFeed = value; __hasPodcastFeedSpecified = true; }
        }

        [JsonProperty(PropertyName = "hasWidget")]
        [XmlElement(ElementName = "hasWidget", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "hasWidget")]
        public bool __hasWidget;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __hasWidgetSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool HasWidget
        {
            get { return __hasWidget; }
            set { __hasWidget = value; __hasWidgetSpecified = true; }
        }

        [JsonProperty(PropertyName = "accessScope")]
        [XmlElement(ElementName = "accessScope", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "accessScope")]
        public AccessScope __accessScope;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __accessScopeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public AccessScope AccessScope
        {
            get { return __accessScope; }
            set { __accessScope = value; __accessScopeSpecified = true; }
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

        [JsonProperty(PropertyName = "scope")]
        [XmlElement(ElementName = "scope", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "scope")]
        public WorkspaceScopeType __scope;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __scopeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public WorkspaceScopeType Scope
        {
            get { return __scope; }
            set { __scope = value; __scopeSpecified = true; }
        }

        [JsonProperty(PropertyName = "widgetFeedsActive")]
        [XmlElement(ElementName = "widgetFeedsActive", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "widgetFeedsActive")]
        public bool __widgetFeedsActive;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __widgetFeedsActiveSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool WidgetFeedsActive
        {
            get { return __widgetFeedsActive; }
            set { __widgetFeedsActive = value; __widgetFeedsActiveSpecified = true; }
        }

        protected WorkspaceProperties()
        {
            __creationDate = DateTime.MinValue;
            __lastModifiedDate = DateTime.MinValue;
            __lastContentModifiedDate = DateTime.MinValue;
        }

        private class MyCustomConverter : JsonCreationConverter<WorkspaceProperties>
        {
            protected override WorkspaceProperties Create(Type objectType,
                Newtonsoft.Json.Linq.JObject jObject)
            {
                if ("AutomaticWorkspaceProperties".Equals(jObject.Value<string>("$type")))
                    return new AutomaticWorkspaceProperties();
               // else if ("ManualWorkspaceProperties".Equals(jObject.Value<string>("$type")))
                 //   return new ManualWorkspaceProperties();
                else if ("CollectionWorkspaceProperties".Equals(jObject.Value<string>("$type")))
                    return new CollectionWorkspaceProperties();
                else if ("NewsletterWorkspaceProperties".Equals(jObject.Value<string>("$type")))
                    return new NewsletterWorkspaceProperties();
                else
                    return null;
            }
        }
    }
}
