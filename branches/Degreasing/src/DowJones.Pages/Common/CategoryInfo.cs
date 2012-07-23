using System.Runtime.Serialization;

namespace DowJones.Pages
{
    [DataContract(Name = "pageCategoryInfo", Namespace = "")]
    public class CategoryInfo
    {
        [DataMember(Name = "categoryDescriptor")]
        public string CategoryDescriptor { get; set; }

        [DataMember(Name = "categoryCode")]
        public string CategoryCode { get; set; }
    }
}