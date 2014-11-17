using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "ItemDateFilter")]
    [XmlType(TypeName = "ItemDateFilter", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class ItemDateFilter : ItemFilter
    {
        [JsonProperty(PropertyName = "startDate")]
        [XmlElement(ElementName = "startDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "startDate")]
        public DateTime __startDate;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __startDateSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime StartDate
        {
            get { return __startDate; }
            set { __startDate = value; __startDateSpecified = true; }
        }

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime StartDateUtc
        {
            get { return __startDate.ToUniversalTime(); }
            set { __startDate = value.ToLocalTime(); __startDateSpecified = true; }
        }

        [JsonProperty(PropertyName = "endDate")]
        [XmlElement(ElementName = "endDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "endDate")]
        public DateTime __endDate;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __endDateSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime EndDate
        {
            get { return __endDate; }
            set { __endDate = value; __endDateSpecified = true; }
        }

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DateTime EndDateUtc
        {
            get { return __endDate.ToUniversalTime(); }
            set { __endDate = value.ToLocalTime(); __endDateSpecified = true; }
        }

        public ItemDateFilter()
        {
            __startDate = DateTime.MinValue;
            __endDate = DateTime.MaxValue;
        }
    }
}
