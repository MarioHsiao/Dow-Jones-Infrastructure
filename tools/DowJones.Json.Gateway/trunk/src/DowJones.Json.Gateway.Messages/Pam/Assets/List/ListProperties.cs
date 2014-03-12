using System;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "ListProperties", Namespace = "")]
    [KnownType(typeof (IndustryListProperties))]
    [KnownType(typeof (RegionListProperties))]
    [KnownType(typeof (SubjectListProperties))]
    [KnownType(typeof (ExecutiveListProperties))]
    [KnownType(typeof (AuthorListProperties))]
    public class ListProperties
    {
        [DataMember(Name = "description", IsRequired = true)]
        public string Description { get; set; }

        [DataMember(Name = "creationDate", IsRequired = true)]
        public DateTime CreationDate { get; set; }

        [DataMember(Name = "createdBy", IsRequired = true, EmitDefaultValue = false)]
        public string CreatedBy { get; set; }

        [DataMember(Name = "lastModifiedDate", IsRequired = true)]
        public DateTime LastModifiedDate { get; set; }

        [DataMember(Name = "lastModifiedBy", IsRequired = true, EmitDefaultValue = false)]
        public string LastModifiedBy { get; set; }

        [DataMember(Name = "lastAccessedDate", IsRequired = true)]
        public DateTime LastAccessedDate { get; set; }
    }
}