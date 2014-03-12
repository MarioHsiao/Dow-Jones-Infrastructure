using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "IndustryList", Namespace = "")]
    public class IndustryList : List
    {
        [DataMember(Name="properties", IsRequired = true, EmitDefaultValue = false)]
        public IndustryListProperties Properties { get; set; }
    }
}