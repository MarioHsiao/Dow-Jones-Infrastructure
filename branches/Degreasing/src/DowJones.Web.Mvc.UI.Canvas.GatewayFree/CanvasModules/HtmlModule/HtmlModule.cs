using System.Collections.Generic;
using System.Linq;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.HtmlModule.Editor;
using DowJones.Web.Mvc.UI.Canvas.GatewayFree.Properties;

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.HtmlModule
{
    public class HtmlModule : Module
    {
        public string Html { get; set; }
        public string Script { get; set; }

        // TODO: Introduce abstraction via new ModuleOption (i.e. CanvasModuleOption)
        [ClientProperty("metadata", Nested = false)]
        public IEnumerable<KeyValuePair<string, object>> Options { get; set; }

        public bool HasScript
        {
            get { return !string.IsNullOrWhiteSpace(Script); }
        }

        public HtmlModule()
        {
            Editor = new HtmlModuleEditor(this);
            DataServiceUrl = CanvasSettings.Default.GetDataServiceUrl(GetType(), Settings.Default);
            Options = Enumerable.Empty<KeyValuePair<string, object>>();
        }


        public class HtmlModuleMapper : TypeMapper<Modules.HtmlModule, IModule>
        {
            public override IModule Map(Modules.HtmlModule source)
            {
                return new HtmlModule {
                    CanEdit = true,
                    ModuleId = source.Id,
                    Title = source.Title,
                    Description = source.Description,
                    Position = source.Position,
                    Html = source.Html,
                    Script = source.Script,
                    Options = source.Options.Select(x => new KeyValuePair<string, object>(x.Name, x.Value)),
                };
            }
        }
    }
}
