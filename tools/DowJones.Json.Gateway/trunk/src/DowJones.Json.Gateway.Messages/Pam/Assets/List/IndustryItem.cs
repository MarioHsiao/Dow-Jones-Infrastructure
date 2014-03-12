using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "IndustryItem", Namespace = "")]
    public class IndustryItem : Item
    {
        [DataMember(Name = "properties", IsRequired = true, EmitDefaultValue = false)]
        public IndustryItemProperties Properties { get; set; }
    }
}