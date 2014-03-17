using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [DataContract(Name = "GetListsDetailsListResponse", Namespace = "")]
    public class GetListsDetailsListResponse
    {
        [DataMember(Name = "listDetailsItemCollection", IsRequired = true, EmitDefaultValue = false)]
        public ListDetailsItemCollection listDetailsItemCollection { get; set; }
    }
}