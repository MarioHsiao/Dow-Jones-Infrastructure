using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "CreateListResponse", Namespace = "")]
    public class CreateListResponse
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
    }
}