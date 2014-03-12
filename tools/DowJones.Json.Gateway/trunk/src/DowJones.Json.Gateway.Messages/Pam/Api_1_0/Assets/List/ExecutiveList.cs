using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "ExecutiveList", Namespace = "")]
    public class ExecutiveList : List
    {
        [DataMember(Name = "properties", IsRequired = true, EmitDefaultValue = false)]
        public ExecutiveListProperties Properties { get; set; }
    }
}