using System.Runtime.Serialization;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [DataContract(Name = "GetListsDetailsListResponse", Namespace = "")]
    public class GetListsDetailsListResponse : JsonRestResponse
    {
        [DataMember(Name = "listDetailsItemCollection", IsRequired = true, EmitDefaultValue = false)]
        public ListDetailsItemCollection ListDetailsItemCollection { get; set; }
    }
}