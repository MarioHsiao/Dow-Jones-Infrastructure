using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "Receipient")]
    [XmlType(TypeName = "Receipient", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class Receipient
    {
        [JsonProperty(PropertyName = "dateFormat")]
        [XmlElement(ElementName = "dateFormat", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "dateFormat")]
        public String __dateFormat;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public String DateFormat
        {
            get { return __dateFormat; }
            set { __dateFormat = value; }
        }

        [JsonProperty(PropertyName = "dateLanguage")]
        [XmlElement(ElementName = "dateLanguage", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "dateLanguage")]
        public String __dateLanguage;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public String DateLanguage
        {
            get { return __dateLanguage; }
            set { __dateLanguage = value; }
        }

        [JsonProperty(PropertyName = "to")]
        [XmlElement(Type = typeof(Email), ElementName = "to", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "to")]
        public EmailCollection __toCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public EmailCollection ToCollection
        {
            get
            {
                if (__toCollection == null) __toCollection = new EmailCollection();
                return __toCollection;
            }
            set { __toCollection = value; }
        }

        [JsonProperty(PropertyName = "cc")]
        [XmlElement(Type = typeof(Email), ElementName = "cc", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "cc")]
        public EmailCollection __ccCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public EmailCollection CcCollection
        {
            get
            {
                if (__ccCollection == null) __ccCollection = new EmailCollection();
                return __ccCollection;
            }
            set { __ccCollection = value; }
        }

        [JsonProperty(PropertyName = "bcc")]
        [XmlElement(Type = typeof(Email), ElementName = "bcc", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "bcc")]
        public EmailCollection __bccCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public EmailCollection BccCollection
        {
            get
            {
                if (__bccCollection == null) __bccCollection = new EmailCollection();
                return __bccCollection;
            }
            set { __bccCollection = value; }
        }

        [JsonProperty(PropertyName = "replyTo")]
        [XmlElement(Type = typeof(Email), ElementName = "replyTo", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "replyTo")]
        public Email __replyTo;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Email ReplyTo
        {
            get
            {
                if (__replyTo == null) __replyTo = new Email();
                return __replyTo;
            }
            set { __replyTo = value; }
        }

        [JsonProperty(PropertyName = "emailFormat")]
        [XmlElement(ElementName = "emailFormat", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "emailFormat")]
        public String __emailFormat;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public String EmailFormat
        {
            get { return __emailFormat; }
            set { __emailFormat = value; }
        }

        public Receipient()
        {
            DateFormat = String.Empty;
            DateLanguage = String.Empty;
            EmailFormat = String.Empty;
        }
    }
}
