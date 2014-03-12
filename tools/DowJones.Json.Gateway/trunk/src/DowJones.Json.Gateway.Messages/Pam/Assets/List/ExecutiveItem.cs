using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "ExecutiveItem", Namespace = "")]
    public class ExecutiveItem : Item
    {
        [DataMember(Name = "properties", IsRequired = true, EmitDefaultValue = false)]
        public ExecutiveItemProperties Properties { get; set; }
    }
}