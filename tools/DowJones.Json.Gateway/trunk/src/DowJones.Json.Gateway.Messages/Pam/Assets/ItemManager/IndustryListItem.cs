using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "IndustryListItem")]
    public class IndustryListItem : Item
    {
        public IndustryListItem()
        {
            Properties = new IndustryListItemProperties();
        }

        [DataMember(Name = "properties")]
        public IndustryListItemProperties Properties { get; set; }
    }
}