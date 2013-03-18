using System.Runtime.Serialization;

namespace DowJones.Pages.Modules
{
    [DataContract(Name = "moduleProperties", Namespace = "")]
    public class ModuleProperties
    {
        [DataMember(Name = "categoryInfo")]
        public PublishStatusScope CategoryInfo { get; set; }

        [DataMember(Name = "publishStatusScope")]
        public PublishStatusScope PublishStatusScope { get; set; }

        [DataMember(Name = "moduleMetaData")]
        public MetaData ModuleMetaData { get; set; }
    }
}