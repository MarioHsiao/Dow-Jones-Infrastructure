using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "AuthorItem", Namespace = "")]
    public class AuthorItem : Item
    {
        [DataMember(Name="properties", IsRequired = true, EmitDefaultValue = false)]
        public AuthorItemProperties Properties { get; set; }
    }
}