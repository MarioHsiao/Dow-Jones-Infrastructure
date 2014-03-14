using System.Runtime.Serialization;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "DeleteListResponse", Namespace = "")]
    public class DeleteListResponse : IJsonRestResponse
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
    }
}
