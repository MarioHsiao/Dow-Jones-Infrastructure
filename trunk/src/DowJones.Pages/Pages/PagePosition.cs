using System.Runtime.Serialization;

namespace DowJones.Pages
{
    [DataContract(Name = "pagePosition", Namespace = "")]
    public class PagePosition
    {
        [DataMember(Name = "pageId")]
        public string PageId { get; set; }

        [DataMember(Name = "position")]
        public int Position { get; set; }
    }
}