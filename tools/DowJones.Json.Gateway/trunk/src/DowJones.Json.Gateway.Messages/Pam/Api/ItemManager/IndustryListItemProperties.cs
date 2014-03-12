using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "IndustryListItemProperties")]
    public class IndustryListItemProperties : ItemProperties
    {
        [DataMember(Name = "operator")]
        public ItemOperator Operator { get; set; }
    }
}