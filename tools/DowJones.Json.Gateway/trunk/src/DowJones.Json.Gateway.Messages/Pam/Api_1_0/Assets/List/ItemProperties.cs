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
        [DataMember(Name = "creationDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime CreationDate { get; set; }

        [DataMember(Name = "createdBy", IsRequired = false, EmitDefaultValue = false)]
        public string CreatedBy { get; set; }

        [DataMember(Name = "lastModifiedDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? LastModifiedDate { get; set; }

        [DataMember(Name = "lastModifiedBy", IsRequired = false, EmitDefaultValue = false)]
        public string LastModifiedBy { get; set; }

        [DataMember(Name = "lastAccessedDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? LastAccessedDate { get; set; }
    }
}