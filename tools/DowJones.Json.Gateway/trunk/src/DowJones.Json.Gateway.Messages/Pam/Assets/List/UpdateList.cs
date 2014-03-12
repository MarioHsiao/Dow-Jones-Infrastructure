using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "UpdateList", Namespace = "")]
    public class UpdateList
    {
        [DataMember(Name = "list", IsRequired = true, EmitDefaultValue = false)]
        public List List { get; set; }
    }
}