using System.Diagnostics;

namespace DowJones.Pages.Modules
{
    [DebuggerDisplay("{Name}:{Value}")]
    public class ModuleTemplateMetaDatum
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public static explicit operator string(ModuleTemplateMetaDatum datum)
        {
            return datum != null ? datum.Value : null;
        }
    }
}