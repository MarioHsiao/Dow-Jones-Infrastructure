namespace DowJones.Pages.Modules
{
    public class ModuleTemplateOption
    {
        public string DefaultValue { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public ModuleTemplateOption()
        {
        }

        public ModuleTemplateOption(string name, string displayName = null, string type = null, string defaultValue = null)
        {
            DefaultValue = defaultValue;
            Name = name;
            DisplayName = displayName ?? name;
            Type = type ?? "string";
        }
    }
}