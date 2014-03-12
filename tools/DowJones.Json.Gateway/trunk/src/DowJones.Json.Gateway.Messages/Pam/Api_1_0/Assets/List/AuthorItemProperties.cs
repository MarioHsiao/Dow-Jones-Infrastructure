using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "AuthorItemProperties", Namespace = "")]
    public class AuthorItemProperties : ItemProperties
    {
        [DataMember(Name = "executiveCode", IsRequired = true, EmitDefaultValue = false)]
        public string ExecutiveCode { get; set; }
    }
}