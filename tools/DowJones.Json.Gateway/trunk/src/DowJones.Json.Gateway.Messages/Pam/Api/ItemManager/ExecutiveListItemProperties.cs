using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "ExecutiveListItemProperties")]
    public class ExecutiveListItemProperties : ItemProperties
    {
        [DataMember(Name = "operator")]
        public ItemOperator Operator { get; set; }
    }
}