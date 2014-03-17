using System.Runtime.Serialization;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "Permission", Namespace = "")]
    public class Permission
    {
        [DataMember(Name = "scope", IsRequired = true)]
        public ShareScope Scope { get; set; }

        [DataMember(Name = "shareRoleCollection", IsRequired = true, EmitDefaultValue = false)]
        public ShareRoleCollection ShareRoleCollection { get; set; }

        [DataMember(Name = "groups", IsRequired = true, EmitDefaultValue = false)]
        public GroupList Groups { get; set; }
    }
}