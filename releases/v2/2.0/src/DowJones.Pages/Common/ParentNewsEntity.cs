using System.Runtime.Serialization;

namespace DowJones.Pages
{
    [DataContract(Name = "parentNewsEntity", Namespace = "")]
    public class ParentNewsEntity : NewsEntity
    {
        [DataMember(Name = "newsEntities", EmitDefaultValue = false)]
        public NewsEntities NewsEntities { get; set; }
    }
}