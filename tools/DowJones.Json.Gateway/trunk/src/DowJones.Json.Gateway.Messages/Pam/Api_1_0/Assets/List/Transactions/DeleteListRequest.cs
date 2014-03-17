using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [ServicePath("pamapi/1.0/List.svc")]
    [DataContract(Name = "DeleteList", Namespace = "")]
    public class DeleteListRequest : IPostJsonRestRequest
    {
        [DataMember(Name = "id", IsRequired = true)]
        public long ID { get; set; }
    }
}