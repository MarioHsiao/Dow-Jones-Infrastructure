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
        [DataMember(Name = "creationDate")]
        public DateTime CreationDate { get; set; }

        [DataMember(Name = "createdBy")]
        public string CreatedBy { get; set; }

        [DataMember(Name = "lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [DataMember(Name = "lastModifiedBy")]
        public string LastModifiedBy { get; set; }

        [DataMember(Name = "lastAccessedDate")]
        public DateTime LastAccessedDate { get; set; }
    }
}