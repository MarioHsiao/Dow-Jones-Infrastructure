using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "GroupList")]
    public class GroupList
    {
        public GroupList()
        {
            GroupIdList = new List<GroupIdList>();
        }

        [DataMember(Name = "groupIdList")]
        public List<GroupIdList> GroupIdList { get; set; }
    }
}