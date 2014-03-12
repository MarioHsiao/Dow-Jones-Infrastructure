using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [XmlInclude(typeof (SyndicationItemEx))]
    [DataContract(Name = "SyndicationItem")]
    public class SyndicationItem : Item
    {
        public SyndicationItem()
        {
            Properties = new SyndicationItemProperties();
        }

        [DataMember(Name = "properties")]
        public SyndicationItemProperties Properties { get; set; }

        [DataMember(Name = "status")]
        public ItemStatus Status { get; set; }
    }
}