using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "IndustryItemProperties", Namespace = "")]
    public class IndustryItemProperties : ItemProperties
    {
        [DataMember(Name = "executiveCode", IsRequired = true, EmitDefaultValue = false)]
        public string ExecutiveCode { get; set; }
    }
}