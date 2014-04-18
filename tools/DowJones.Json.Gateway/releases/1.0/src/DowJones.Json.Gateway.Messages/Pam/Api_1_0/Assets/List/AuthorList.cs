using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "AuthorList", Namespace = "")]
    public class AuthorList : List
    {
        [DataMember(Name = "properties", IsRequired = true, EmitDefaultValue = false)]
        public AuthorListProperties Properties { get; set; }
    }
}