using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "deliveryInfo")]
    [XmlType(TypeName = "deliveryInfo", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class DeliveryInfo
    {
        [JsonProperty(PropertyName = "id")]
        [XmlElement(ElementName = "id", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "long", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "id")]
        public long __id;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __idSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public long Id
        {
            get { return __id; }
            set { __id = value; __idSpecified = true; }
        }

        [JsonProperty(PropertyName = "subject")]
        [XmlElement(ElementName = "subject", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "subject")]
        public string __subject;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Subject
        {
            get { return __subject; }
            set { __subject = value; }
        }

        [JsonProperty(PropertyName = "message")]
        [XmlElement(ElementName = "message", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "message")]
        public string __message;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string Message
        {
            get { return __message; }
            set { __message = value; }
        }

        [JsonProperty(PropertyName = "receipient")]
        [XmlElement(Type = typeof(Receipient), ElementName = "receipient", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "receipient")]
        public Receipient __receipient;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Receipient Receipient
        {
            get
            {
                if (__receipient == null) __receipient = new Receipient();
                return __receipient;
            }
            set { __receipient = value; }
        }

        [JsonProperty(PropertyName = "outputFormat")]
        [XmlElement(ElementName = "outputFormat", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "outputFormat")]
        public OutputFormat __outputFormat;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __outputFormatSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public OutputFormat OutputFormat
        {
            get { return __outputFormat; }
            set { __outputFormat = value; __outputFormatSpecified = true; }
        }

        [JsonProperty(PropertyName = "attachmentType")]
        [XmlElement(ElementName = "attachmentType", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "attachmentType")]
        public AttachmentType __attachmentType;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __attachmentTypeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public AttachmentType AttachmentType
        {
            get { return __attachmentType; }
            set { __attachmentType = value; __attachmentTypeSpecified = true; }
        }

        [JsonProperty(PropertyName = "attachmentTemplateId")]
        [XmlElement(ElementName = "attachmentTemplateId", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "long", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "attachmentTemplateId")]
        public long __attachmentTemplateId;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __attachmentTemplateIdSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public long AttachmentTemplateId
        {
            get { return __attachmentTemplateId; }
            set { __attachmentTemplateId = value; __attachmentTemplateIdSpecified = true; }
        }

        [JsonProperty(PropertyName = "addNewsletterAttachment")]
        [XmlElement(ElementName = "addNewsletterAttachment", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "addNewsletterAttachment")]
        public bool __addNewsletterAttachment;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __addNewsletterAttachmentSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool AddNewsletterAttachment
        {
            get { return __addNewsletterAttachment; }
            set { __addNewsletterAttachment = value; __addNewsletterAttachmentSpecified = true; }
        }

        [JsonProperty(PropertyName = "appendDateToSubject")]
        [XmlElement(ElementName = "appendDateToSubject", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "appendDateToSubject")]
        public bool __appendDateToSubject;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __appendDateToSubjectSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool AppendDateToSubject
        {
            get { return __appendDateToSubject; }
            set { __appendDateToSubject = value; __appendDateToSubjectSpecified = true; }
        }

        public DeliveryInfo()
        {

        }
    }
}
