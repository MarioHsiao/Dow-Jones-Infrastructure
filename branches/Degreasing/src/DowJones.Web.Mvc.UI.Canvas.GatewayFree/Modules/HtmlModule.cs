using System.Collections.Generic;
using System.Linq;
using DowJones.Pages.Modules;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.Modules
{
    public class ModuleOption
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public ModuleOption()
        {
        }

        public ModuleOption(ModuleTemplateOption option)
        {
            Name = option.Name;
            Type = option.Type;
            Value = option.DefaultValue;
        }
    }

    public class HtmlModule : Pages.Modules.Module
    {
        public string Html { get; set; }
        public string Script { get; set; }

        public IEnumerable<ModuleOption> Options { get; set; }

        public int TemplateId { get; set; }

        public HtmlModule()
        {
            Options = Enumerable.Empty<ModuleOption>();
        }
    }
}
