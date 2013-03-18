using System.Runtime.Serialization;

namespace DowJones.Models.Common
{
    [DataContract(Name = "parentDateNewsEntity", Namespace = "")]
    public class ParentDateNewsEntity : NewsEntity
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "position")]
        public int Position { get; set; }

        [DataMember(Name = "isExpanded")]
        public bool IsExpanded { get; set; }

        [DataMember(Name = "newsEntities", EmitDefaultValue = false)]
        public DateNewsEntities NewsEntities { get; set; }
    }
}
