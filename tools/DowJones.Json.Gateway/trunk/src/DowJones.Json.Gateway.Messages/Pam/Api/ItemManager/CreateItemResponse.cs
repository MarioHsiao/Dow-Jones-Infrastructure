using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "CreateItemResponse")]
    public class CreateItemResponse
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
    }
}