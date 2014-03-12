using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "RegionListItem")]
    public class RegionListItem : Item
    {
        public RegionListItem()
        {
            Properties = new RegionListItemProperties();
        }

        [DataMember(Name = "properties")]
        public RegionListItemProperties Properties { get; set; }
    }
}