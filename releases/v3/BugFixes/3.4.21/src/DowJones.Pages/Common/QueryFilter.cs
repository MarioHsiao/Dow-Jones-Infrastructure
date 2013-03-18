using System.Runtime.Serialization;

namespace DowJones.Pages.Common
{
    [DataContract(Namespace = "")]
    public class QueryFilter
    {
        [DataMember(Name = "type")]
        public FilterType Type { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "descriptor")]
        public string Descriptor { get; set; }
    }
}
