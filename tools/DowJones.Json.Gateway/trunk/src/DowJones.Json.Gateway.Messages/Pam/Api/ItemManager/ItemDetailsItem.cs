using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "ItemDetailsItem")]
    public class ItemDetailsItem
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "errorCode")]
        public int ErrorCode { get; set; }

        [DataMember(Name = "item")]
        public Item Item { get; set; }
    }
}