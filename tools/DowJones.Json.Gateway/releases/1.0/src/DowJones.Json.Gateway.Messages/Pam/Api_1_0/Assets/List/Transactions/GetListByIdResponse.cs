using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [ServicePath("pamapi/1.0/Lists.svc")]
    [DataContract(Name = "GetListByIDResponse", Namespace = "")]
    public class GetListByIdResponse : IJsonRestResponse
    {
        [DataMember(Name = "list", IsRequired = true, EmitDefaultValue = false)]
        public List List { get; set; }
    }
}