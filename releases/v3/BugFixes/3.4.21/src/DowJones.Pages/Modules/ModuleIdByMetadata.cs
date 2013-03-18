using System.Runtime.Serialization;

namespace DowJones.Pages.Modules
{
    public class ModuleIdByMetadata
    {
        [DataMember(Name = "moduleId")]
        public int ModuleId;

        [DataMember(Name = "metaData")]
        public MetaData MetaData;

        [DataMember(Name = "interfaceLanguage")]
        public string InterfaceLanguage;
    }
}