using System.Runtime.Serialization;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [DataContract(Name = "GetListsPropertiesListResponse", Namespace = "")]
    public class GetListsPropertiesListResponse : IJsonRestResponse
    {
        [DataMember(Name = "listPropertiesItemCollection", IsRequired = true, EmitDefaultValue = false)]
        public ListPropertiesItemCollection ListPropertiesItemCollection { get; set; }
    }
}