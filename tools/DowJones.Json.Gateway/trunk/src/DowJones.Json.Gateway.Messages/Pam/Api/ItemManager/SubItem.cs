using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [XmlInclude(typeof (SubjectListSubItem))]
    [XmlInclude(typeof (ExecutiveListSubItem))]
    [XmlInclude(typeof (IndustryListSubItem))]
    [XmlInclude(typeof (RegionListSubItem))]
    [XmlInclude(typeof (AuthorListSubItem))]
    [DataContract(Name = "SubItem")]
    public class SubItem
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "position")]
        public long Position { get; set; }

        [DataMember(Name = "createdDate")]
        public DateTime CreatedDate { get; set; }

        [DataMember(Name = "lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [DataMember(Name = "lastModifiedBy")]
        public string LastModifiedBy { get; set; }
    }
}