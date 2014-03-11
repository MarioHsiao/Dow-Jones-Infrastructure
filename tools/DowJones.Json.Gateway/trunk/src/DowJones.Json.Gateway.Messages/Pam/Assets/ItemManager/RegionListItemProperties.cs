using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "RegionListItemProperties")]
    public class RegionListItemProperties : ItemProperties
    {
        [DataMember(Name = "operator")]
        public ItemOperator Operator { get; set; }
    }
}