using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Pages.Modules
{
    [DataContract(Name = "scriptModule", Namespace = "")]
    public class ScriptModule : Module
    {
        [DataMember(Name = "templateId")]
        public string TemplateId { get; set; }

        [DataMember(Name = "options")]
        public ModuleOptions Options { get; set; }

        [DataMember(Name = "moduleState")]
        public ModuleState ModuleState { get; set; }

        [DataMember(Name = "columnSpan")]
        public int ColumnSpan { get; set; }

        public ScriptModule()
        {
            Options = new ModuleOptions();
            ModuleState = ModuleState.Maximized;
        }
    }

    [CollectionDataContract(Name = "options", Namespace = "", ItemName = "option")]
    public class ModuleOptions : List<NameValuePair>
    {
    }
}