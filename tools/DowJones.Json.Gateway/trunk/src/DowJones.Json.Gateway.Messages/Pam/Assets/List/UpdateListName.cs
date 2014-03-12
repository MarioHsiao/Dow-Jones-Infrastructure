using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "UpdateListName", Namespace = "")]
    public class UpdateListName
    {
        [DataMember(Name = "id", IsRequired = true)]
        public long Id { get; set; }

        [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = false)]
        public string Name { get; set; }
    }
}