using System.Runtime.Serialization;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [DataContract(Name = "SubscribeList", Namespace = "")]
    public class SubscribeListRequest : IPostJsonRestRequest
    {
        [DataMember(Name = "subscribedListId", IsRequired = true)]
        public long SubscribedListId { get; set; }

        public virtual string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}