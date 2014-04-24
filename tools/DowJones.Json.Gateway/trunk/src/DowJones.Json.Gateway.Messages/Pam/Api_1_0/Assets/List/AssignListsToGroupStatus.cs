using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "AssignListsToGroupStatus", Namespace = "")]
    public class AssignListsToGroupStatus
    {
        [DataMember(Name="listId", IsRequired = true, EmitDefaultValue = false)]
        public long ListId { get; set; }

        [DataMember(Name="errorCode", IsRequired = true)]
        public string ErrorCode { get; set; }
    }
}