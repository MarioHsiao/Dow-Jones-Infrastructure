using System.Runtime.Serialization;

namespace DowJones.Models.Common
{
    [DataContract(Name = "sourceNewsEntity", Namespace = "")]
    public class SourceNewsEntity : NewsEntity
    {
        [DataMember(Name = "isGroup")]
        public bool IsGroup { get; set; }
    }
}