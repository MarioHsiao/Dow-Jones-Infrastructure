using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "IdSearchFilter", Namespace = "")]
    public class IdSearchFilter : Filter
    {
        [DataMember(Name = "listIdCollection", IsRequired = true, EmitDefaultValue = false)]
        public ListIdCollection ListIdCollection { get; set; }
    }
}