using System;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "ListProperties", Namespace = "")]
    [KnownType(typeof (IndustryListProperties))]
    [KnownType(typeof (RegionListProperties))]
    [KnownType(typeof (SubjectListProperties))]
    [KnownType(typeof (ExecutiveListProperties))]
    [KnownType(typeof (AuthorListProperties))]
    public class ListProperties
    {
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "creationDate", EmitDefaultValue = false)]
        public DateTime? CreationDate { get; set; }

        [DataMember(Name = "createdBy", EmitDefaultValue = false)]
        public string CreatedBy { get; set; }

        [DataMember(Name = "lastModifiedDate", EmitDefaultValue = false)]
        public DateTime? LastModifiedDate { get; set; }

        [DataMember(Name = "lastModifiedBy", EmitDefaultValue = false)]
        public string LastModifiedBy { get; set; }

        [DataMember(Name = "lastAccessedDate", EmitDefaultValue = false)]
        public DateTime? LastAccessedDate { get; set; }
    }
}