using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [XmlInclude(typeof (SubjectListItemProperties))]
    [XmlInclude(typeof (ExecutiveListItemProperties))]
    [XmlInclude(typeof (IndustryListItemProperties))]
    [XmlInclude(typeof (RegionListItemProperties))]
    [XmlInclude(typeof (AuthorListItemProperties))]
    [XmlInclude(typeof (ChartItemProperties))]
    [XmlInclude(typeof (BookmarkItemProperties))]
    [XmlInclude(typeof (SyndicationItemProperties))]
    [XmlInclude(typeof (UserFileProperties))]
    [XmlInclude(typeof (ImageProperties))]
    [DataContract(Name = "ItemProperties")]
    public abstract class ItemProperties
    {
        protected ItemProperties()
        {
            Tag = new List<object>();
        }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "createdBy")]
        public string CreatedBy { get; set; }

        [DataMember(Name = "creationDate")]
        public DateTime CreationDate { get; set; }

        [DataMember(Name = "lastModifiedBy")]
        public string LastModifiedBy { get; set; }

        [DataMember(Name = "lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [DataMember(Name = "lastAccessedDate")]
        public DateTime LastAccessedDate { get; set; }

        [DataMember(Name = "tag")]
        public List<object> Tag { get; set; }

        [DataMember(Name = "createdByNamespace")]
        public string CreatedByNamespace { get; set; }

        [DataMember(Name = "createdByAccountId")]
        public string CreatedByAccountId { get; set; }
    }
}