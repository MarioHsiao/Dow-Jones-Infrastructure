using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "NewsletterWorkspaceProperties")]
    [DataContract(Name = "NewsletterWorkspaceProperties", Namespace = "")]
    [XmlType(TypeName = "NewsletterWorkspaceProperties", Namespace = Declarations.SchemaVersion), Serializable]
    public class NewsletterWorkspaceProperties : ManualWorkspaceProperties
    {
        [JsonProperty(PropertyName = "templateId")]
        [XmlElement(ElementName = "templateId", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "long", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "templateId")]
        public long __templateId;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __templateIdSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public long TemplateId
        {
            get { return __templateId; }
            set { __templateId = value; __templateIdSpecified = true; }
        }

        [JsonProperty(PropertyName = "dateline")]
        [XmlElement(ElementName = "dateline", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "dateline")]
        public string __dateline;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Dateline
        {
            get { return __dateline; }
            set { __dateline = value; }
        }

        [JsonProperty(PropertyName = "footer")]
        [XmlElement(ElementName = "footer", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "footer")]
        public string __footer;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Footer
        {
            get { return __footer; }
            set { __footer = value; }
        }

        [JsonProperty(PropertyName = "masthead")]
        [XmlElement(Type = typeof(MastHeadData), ElementName = "masthead", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "masthead")]
        public MastHeadData __masthead;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public MastHeadData Masthead
        {
            get
            {
                if (__masthead == null) __masthead = new MastHeadData();
                return __masthead;
            }
            set { __masthead = value; }
        }

        [JsonProperty(PropertyName = "showTableOfContents")]
        [XmlElement(ElementName = "showTableOfContents", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "showTableOfContents")]
        public bool __showTableOfContents;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __showTableOfContentsSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool ShowTableOfContents
        {
            get { return __showTableOfContents; }
            set { __showTableOfContents = value; __showTableOfContentsSpecified = true; }
        }

        [JsonProperty(PropertyName = "includeSnippet")]
        [XmlElement(ElementName = "includeSnippet", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "includeSnippet")]
        public bool __includeSnippet;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __includeSnippetSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool IncludeSnippet
        {
            get { return __includeSnippet; }
            set { __includeSnippet = value; __includeSnippetSpecified = true; }
        }

        [JsonProperty(PropertyName = "summary")]
        [XmlElement(ElementName = "summary", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string __summary;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Summary
        {
            get { return __summary; }
            set { __summary = value; }
        }

        [JsonProperty(PropertyName = "title")]
        [XmlElement(ElementName = "title", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "title")]
        public string __title;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Title
        {
            get { return __title; }
            set { __title = value; }
        }

        [JsonProperty(PropertyName = "documentDate")]
        [XmlElement(ElementName = "documentDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "documentDate")]
        public DateTime __documentDate;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __documentDateSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime DocumentDate
        {
            get { return __documentDate; }
            set { __documentDate = value; __documentDateSpecified = true; }
        }

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime DocumentDateUtc
        {
            get { return __documentDate.ToUniversalTime(); }
            set { __documentDate = value.ToLocalTime(); __documentDateSpecified = true; }
        }

        [JsonProperty(PropertyName = "emailSentDate")]
        [XmlElement(ElementName = "emailSentDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "emailSentDate")]
        public DateTime __emailSentDate;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __emailSentDateSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime EmailSentDate
        {
            get { return __emailSentDate; }
            set
            {
                __emailSentDate = value;
                __emailSentDateSpecified = true;
            }
        }

        [JsonProperty(PropertyName = "rssSentDate")]
        [XmlElement(ElementName = "rssSentDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "rssSentDate")]
        public DateTime __rssSentDate;


        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __rssSentDateSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime RssSentDate
        {
            get { return __rssSentDate; }
            set
            {
                __rssSentDate = value;
                __rssSentDateSpecified = true;
            }
        }


        [JsonProperty(PropertyName = "widgetSentDate")]
        [XmlElement(ElementName = "widgetSentDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "widgetSentDate")]
        public DateTime __widgetSentDate;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __widgetSentDateSpecified;


        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime WidgetSentDate
        {
            get { return __widgetSentDate; }
            set
            {
                __widgetSentDate = value;
                __widgetSentDateSpecified = true;
            }
        }

        [JsonProperty(PropertyName = "freeText")]
        [XmlElement(ElementName = "freeText", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "freeText")]
        public string __freeText;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string FreeText
        {
            get { return __freeText; }
            set { __freeText = value; }
        }


        public NewsletterWorkspaceProperties()
        {
            __documentDate = DateTime.MinValue;
        }
    }
}
