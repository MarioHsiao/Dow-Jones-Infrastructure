using System.Runtime.Serialization;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [DataContract(Name = "UnSubscribeList", Namespace = "")]
    public class UnSubscribeListRequest : IPostJsonRestRequest
    {
        [DataMember(Name = "listId", IsRequired = true)]
        public long ListId { get; set; }

        public virtual string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}