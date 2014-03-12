using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "GroupIdList", Namespace = "")]
    public class GroupIdList
    {
        [DataMember(Name = "idCollection", IsRequired = true, EmitDefaultValue = false)]
        public IdCollection IdCollection { get; set; }

        [DataMember(Name = "type", IsRequired = true)]
        public GroupListType Type { get; set; }
    }
}