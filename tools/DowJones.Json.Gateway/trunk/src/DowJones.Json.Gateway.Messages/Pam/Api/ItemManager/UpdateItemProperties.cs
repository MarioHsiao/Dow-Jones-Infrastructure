using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "UpdateItemProperties")]
    public class UpdateItemProperties
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "itemProperties")]
        public ItemProperties ItemProperties { get; set; }
    }
}