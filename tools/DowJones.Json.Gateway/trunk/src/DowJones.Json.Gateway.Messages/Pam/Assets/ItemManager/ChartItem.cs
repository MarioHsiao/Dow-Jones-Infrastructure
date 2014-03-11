using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "ChartItem")]
    public class ChartItem : Item
    {
        public ChartItem()
        {
            Properties = new ChartItemProperties();
        }

        [DataMember(Name = "properties")]
        public ChartItemProperties Properties { get; set; }

        [DataMember(Name = "status")]
        public ItemStatus Status { get; set; }
    }
}