using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "CreateList", Namespace = "")]
    public class CreateList
    {
        [DataMember(Name="list", IsRequired = true, EmitDefaultValue = false)]
        public List List { get; set; }
    }
}