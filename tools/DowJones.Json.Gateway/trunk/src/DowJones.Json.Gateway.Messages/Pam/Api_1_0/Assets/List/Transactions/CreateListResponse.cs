using System.Runtime.Serialization;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [DataContract(Name = "CreateListResponse", Namespace = "")]
    public class CreateListResponse: JsonRestResponse
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
    }
}