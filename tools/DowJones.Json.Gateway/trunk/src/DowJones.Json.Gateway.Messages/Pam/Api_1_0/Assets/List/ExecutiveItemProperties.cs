using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "ExecutiveItemProperties", Namespace = "")]
    public class ExecutiveItemProperties : ItemProperties
    {
        [DataMember(Name = "executiveCode", IsRequired = true, EmitDefaultValue = false)]
        public string ExecutiveCode { get; set; }
    }
}