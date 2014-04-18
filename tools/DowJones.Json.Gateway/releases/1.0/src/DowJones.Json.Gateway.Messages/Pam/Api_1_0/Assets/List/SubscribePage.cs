using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "SubscribePage", Namespace = "")]
    public class SubscribePage
    {
        [DataMember(Name = "listId", IsRequired = true)]
        public long ListId { get; set; }
    }
}