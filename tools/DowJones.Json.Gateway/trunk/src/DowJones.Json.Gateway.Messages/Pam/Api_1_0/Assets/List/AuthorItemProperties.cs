using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "AuthorItemProperties", Namespace = "")]
    public class AuthorItemProperties : ItemProperties
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }
    }
}