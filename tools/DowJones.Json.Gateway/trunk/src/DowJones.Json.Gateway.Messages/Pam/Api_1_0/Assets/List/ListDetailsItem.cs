using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "ListDetailsItem", Namespace = "")]
    public class ListDetailsItem
    {
        [DataMember(Name = "id", IsRequired = true)]
        public long Id { get; set; }

        [DataMember(Name = "ErrorCode", EmitDefaultValue = false)]
        public string ErrorCode { get; set; }

        [DataMember(Name = "list", EmitDefaultValue = false)]
        public List List { get; set; }
    }
}