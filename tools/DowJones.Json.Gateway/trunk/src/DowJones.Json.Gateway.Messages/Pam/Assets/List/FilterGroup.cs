using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "FilterGroup", Namespace = "")]
    public class FilterGroup
    {
        [DataMember(Name = "filterCollection", IsRequired = true, EmitDefaultValue = false)]
        public FilterCollection FilterCollection { get; set; }
    }
}