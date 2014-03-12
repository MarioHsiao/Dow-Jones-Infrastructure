using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "RegionList", Namespace = "")]
    public class RegionList : List
    {
        [DataMember(Name="properties", IsRequired = true, EmitDefaultValue = false)]
        public RegionListProperties Properties { get; set; }
    }
}