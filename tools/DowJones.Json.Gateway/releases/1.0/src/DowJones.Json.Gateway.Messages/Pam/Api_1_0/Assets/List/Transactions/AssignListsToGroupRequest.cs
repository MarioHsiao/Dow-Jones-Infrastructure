using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [ServicePath("pamapi/1.0/Lists.svc")]
    [DataContract(Name = "AssignListsToGroup", Namespace = "")]
    public class AssignListsToGroupRequest : IPostJsonRestRequest
    {
        [DataMember(Name="listIdCollection", IsRequired = true, EmitDefaultValue = false)]
        public ListIdCollection ListIdCollection { get; set; }

        [DataMember(Name="action", IsRequired = true)]
        public GroupAssignmentAction Action { get; set; }

        [DataMember(Name="groups", EmitDefaultValue = false)]
        public GroupList Groups { get; set; }

        public string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}