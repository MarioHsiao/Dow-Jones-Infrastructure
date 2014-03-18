using System.Runtime.Serialization;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [DataContract(Name = "DeleteListResponse", Namespace = "")]
    public class DeleteListResponse : JsonRestResponse
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
    }
}
