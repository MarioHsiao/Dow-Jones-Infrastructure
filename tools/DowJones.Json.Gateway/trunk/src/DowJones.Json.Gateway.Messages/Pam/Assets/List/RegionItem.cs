using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "RegionItem", Namespace = "")]
    public class RegionItem : Item
    {
        [DataMember(Name = "properties", IsRequired = true, EmitDefaultValue = false)]
        public RegionItemProperties properties { get; set; }
    }
}