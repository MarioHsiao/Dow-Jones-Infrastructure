using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [CollectionDataContract(Name = "GroupList", Namespace = "", ItemName = "groupIdList")]
    public class GroupList : System.Collections.Generic.List<GroupIdList>
    {
    }
}