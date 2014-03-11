using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "UpdateItemResponse")]
    public class UpdateItemResponse
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
    }
}