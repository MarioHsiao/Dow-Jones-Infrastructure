using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "DisseminatedAsset")]
    [XmlType(TypeName = "DisseminatedAsset", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DisseminatedAsset
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

        [JsonProperty(PropertyName = "type")]
        [XmlElement(ElementName = "type", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "type")]
        public DisseminatedAssetType __type;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __typeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DisseminatedAssetType Type
        {
            get { return __type; }
            set { __type = value; __typeSpecified = true; }
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

        [JsonProperty(PropertyName = "emailSentDate")]
        [XmlElement(ElementName = "emailSentDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
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
            set { __emailSentDate = value; __emailSentDateSpecified = true; }
        }

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime EmailSentDateUtc
        {
            get { return __emailSentDate.ToUniversalTime(); }
            set { __emailSentDate = value.ToLocalTime(); __emailSentDateSpecified = true; }
        }

        [JsonProperty(PropertyName = "emailJobId")]
        [XmlElement(ElementName = "emailJobId", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "emailJobId")]
        public string __emailJobId;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string EmailJobId
        {
            get { return __emailJobId; }
            set { __emailJobId = value; }
        }

        public DisseminatedAsset()
        {
            __lastModifiedDate = DateTime.Now;
        }
    }
}
