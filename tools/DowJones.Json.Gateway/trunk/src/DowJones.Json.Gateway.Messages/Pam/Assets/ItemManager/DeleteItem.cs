using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "DeleteItem")]
    public class DeleteItem
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
    }
}