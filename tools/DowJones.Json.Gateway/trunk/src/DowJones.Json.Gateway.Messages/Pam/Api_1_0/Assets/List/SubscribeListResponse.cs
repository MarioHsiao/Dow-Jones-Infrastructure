using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "SubscribeListResponse", Namespace = "")]
    public class SubscribeListResponse
    {
        [DataMember(Name = "subscribedListId", IsRequired = true)]
        public long SubscribedListId { get; set; }
    }
}