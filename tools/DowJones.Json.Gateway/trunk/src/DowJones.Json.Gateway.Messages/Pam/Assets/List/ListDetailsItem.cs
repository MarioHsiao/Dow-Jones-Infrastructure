using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "ListDetailsItem", Namespace = "")]
    public class ListDetailsItem
    {
        [DataMember(Name = "id", IsRequired = true)]
        public long Id { get; set; }

        [DataMember(Name = "ErrorCode", IsRequired = true, EmitDefaultValue = false)]
        public string ErrorCode { get; set; }

        [DataMember(Name = "list", IsRequired = true, EmitDefaultValue = false)]
        public List List { get; set; }

    }
}