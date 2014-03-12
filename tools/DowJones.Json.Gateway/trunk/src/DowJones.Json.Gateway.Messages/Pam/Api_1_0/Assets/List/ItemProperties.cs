using System;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "ItemProperties", Namespace = "")]
    [KnownType(typeof (IndustryItemProperties))]
    [KnownType(typeof (RegionItemProperties))]
    [KnownType(typeof (SubjectItemProperties))]
    [KnownType(typeof (ExecutiveItemProperties))]
    [KnownType(typeof (AuthorItemProperties))]
    public class ItemProperties
    {
        private DateTime creationDateField;
        private DateTime lastAccessedDateField;
        private DateTime lastModifiedDateField;

        [DataMember(Name = "creationDate", IsRequired = true)]
        public DateTime CreationDate
        {
            get { return creationDateField; }
            set { creationDateField = value; }
        }

        [DataMember(Name = "createdBy", IsRequired = true, EmitDefaultValue = false)]
        public string CreatedBy { get; set; }

        [DataMember(Name = "lastModifiedDate", IsRequired = true)]
        public DateTime LastModifiedDate
        {
            get { return lastModifiedDateField; }
            set { lastModifiedDateField = value; }
        }

        [DataMember(Name = "lastModifiedBy", IsRequired = true, EmitDefaultValue = false)]
        public string LastModifiedBy { get; set; }

        [DataMember(Name = "lastAccessedDate", IsRequired = true, Order = 4)]
        public DateTime LastAccessedDate
        {
            get { return lastAccessedDateField; }
            set { lastAccessedDateField = value; }
        }
    }
}