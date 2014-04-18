using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "SubjectList", Namespace = "")]
    public class SubjectList : List
    {
        [DataMember(Name = "properties", IsRequired = true, EmitDefaultValue = false)]
        public SubjectListProperties Properties { get; set; }
    }
}