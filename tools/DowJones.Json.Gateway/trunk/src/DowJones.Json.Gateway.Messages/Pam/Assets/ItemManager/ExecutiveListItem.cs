using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "ExecutiveListItem")]
    public class ExecutiveListItem : Item
    {
        public ExecutiveListItem()
        {
            Properties = new ExecutiveListItemProperties();
        }

        [DataMember(Name = "properties")]
        public ExecutiveListItemProperties Properties { get; set; }

        [DataMember(Name = "status")]
        public ItemStatus Status { get; set; }
    }
}