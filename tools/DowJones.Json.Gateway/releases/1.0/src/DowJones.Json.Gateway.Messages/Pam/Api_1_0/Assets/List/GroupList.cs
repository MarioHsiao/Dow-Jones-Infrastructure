using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [CollectionDataContract(Name = "GroupList", Namespace = "", ItemName = "groupIdList")]
    public class GroupList : List<GroupIdList>
    {
        public GroupList()
        {
        }

        public GroupList(IEnumerable<GroupIdList> collection) : base(collection)
        {
        }
    }
}