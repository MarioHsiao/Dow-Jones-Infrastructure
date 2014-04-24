using System.Runtime.Serialization;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [DataContract(Name = "AssignListsToGroupResponse", Namespace = "")]
    public class AssignListsToGroupResponse : IJsonRestResponse
    {
        [DataMember(Name = "assignListsToGroupStatusCollection", IsRequired = true, EmitDefaultValue = false)]
        public AssignListsToGroupStatusCollection AssignListsToGroupStatusCollection { get; set; }
    }
}