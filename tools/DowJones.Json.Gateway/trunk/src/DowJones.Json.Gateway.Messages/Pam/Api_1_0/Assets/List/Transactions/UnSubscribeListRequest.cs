using System.Runtime.Serialization;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [DataContract(Name = "UnSubscribeList", Namespace = "")]
    public class UnSubscribeListRequest : PostJsonRestRequest
    {
        [DataMember(Name = "listId", IsRequired = true)]
        public long ListId { get; set; }
    }
}