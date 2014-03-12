using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "SubjectItem", Namespace = "")]
    public class SubjectItem : Item
    {
        [DataMember(Name = "properties", IsRequired = true, EmitDefaultValue = false)]
        public SubjectItemProperties Properties { get; set; }
    }
}