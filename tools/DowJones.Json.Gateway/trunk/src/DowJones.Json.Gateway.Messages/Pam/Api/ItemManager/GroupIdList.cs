using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "GroupIdList")]
    public class GroupIdList
    {
        public GroupIdList()
        {
            Id = new List<string>();
        }

        [DataMember(Name = "id")]
        public List<string> Id { get; set; }

        [DataMember(Name = "type")]
        public GroupListType Type { get; set; }
    }
}