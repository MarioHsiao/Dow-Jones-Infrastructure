using System;
using System.Xml.Serialization;

namespace DowJones.Pages.Modules.Templates
{
    [Serializable]
    [XmlRoot(ElementName = "Option", Namespace = "")]
    public class ScriptModuleTemplateOption
    {
        public string DefaultValue { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public ScriptModuleTemplateOption()
        {
            Type = "string";
        }

        public ScriptModuleTemplateOption(string name, string displayName = null, string type = null, string defaultValue = null)
        {
            DefaultValue = defaultValue;
            DisplayName = displayName ?? name;
            Name = name;
            Type = type ?? Type;
        }

        public ScriptModuleTemplateOption(ScriptModuleTemplateOption option)
        {
            if (option == null)
                return;
            
            DefaultValue = option.DefaultValue;
            DisplayName = option.DisplayName;
            Name = option.Name;
            Type = option.Type ?? Type;
        }
    }
}