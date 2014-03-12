using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [XmlInclude(typeof(SubjectListItem))]
    [XmlInclude(typeof(ExecutiveListItem))]
    [XmlInclude(typeof(IndustryListItem))]
    [XmlInclude(typeof(RegionListItem))]
    [XmlInclude(typeof(AuthorListItem))]
    [XmlInclude(typeof(ChartItem))]
    [XmlInclude(typeof(BookmarkItem))]
    [XmlInclude(typeof(SyndicationItem))]
    [XmlInclude(typeof(SyndicationItemEx))]
    [XmlInclude(typeof(UserFile))]
    [XmlInclude(typeof(Image))]
    [DataContract(Name = "Item")]
    public abstract class Item
    {
        protected Item()
        {
            SubItem = new List<SubItem>();
            ShareProperties = new ShareProperties();
            Id = 0;
        }

        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "shareProperties")]
        public ShareProperties ShareProperties { get; set; }

        [DataMember(Name = "subItem")]
        public List<SubItem> SubItem { get; set; }
    }
}