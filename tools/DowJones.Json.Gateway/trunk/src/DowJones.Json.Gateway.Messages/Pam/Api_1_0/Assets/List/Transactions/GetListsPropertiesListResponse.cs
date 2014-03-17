using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [DataContract(Name = "GetListsPropertiesListResponse", Namespace = "")]
    public class GetListsPropertiesListResponse
    {
        [DataMember(Name = "listPropertiesItemCollection", IsRequired = true, EmitDefaultValue = false)]
        public ListPropertiesItemCollection ListPropertiesItemCollection { get; set; }
    }
}