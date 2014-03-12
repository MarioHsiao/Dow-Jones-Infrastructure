using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "Permission")]
    public class Permission
    {
        public Permission()
        {
            Groups = new List<GroupIdList>();
            Roles = new List<ShareRole>();
        }

        [DataMember(Name = "scope")]
        public ShareScope Scope { get; set; }

        [DataMember(Name = "roles")]
        public List<ShareRole> Roles { get; set; }

        [DataMember(Name = "groupIdLists")]
        public List<GroupIdList> Groups { get; set; }
    }
}