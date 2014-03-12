using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "FilterGroup", Namespace = "")]
    public class FilterGroup
    {
        [DataMember(Name = "filterCollection", IsRequired = true, EmitDefaultValue = false)]
        public FilterCollection FilterCollection { get; set; }
    }
}