using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "UpdateItem")]
    public class UpdateItem
    {
        [DataMember(Name = "item")]
        public Item Item { get; set; }
    }
}