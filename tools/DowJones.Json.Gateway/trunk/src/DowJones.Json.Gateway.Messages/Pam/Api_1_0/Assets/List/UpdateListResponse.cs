

using System.Runtime.Serialization;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "UpdateListResponse", Namespace = "")]
    public class UpdateListResponse : IJsonRestResponse
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
    }
}
