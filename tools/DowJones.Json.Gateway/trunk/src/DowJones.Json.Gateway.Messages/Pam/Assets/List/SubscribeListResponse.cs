using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "SubscribeListResponse", Namespace = "")]
    public class SubscribeListResponse
    {
        [DataMember(Name = "subscribedListId", IsRequired = true)]
        public long SubscribedListId { get; set; }
    }
}