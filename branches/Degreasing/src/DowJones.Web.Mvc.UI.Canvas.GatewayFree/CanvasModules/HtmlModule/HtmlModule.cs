using DowJones.Mapping;

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.HtmlModule
{
    public class HtmlModule : DowJones.Web.Mvc.UI.Canvas.Module
    {
        public string Html { get; set; }
        public string Script { get; set; }

        public bool HasScript
        {
            get { return !string.IsNullOrWhiteSpace(Script); }
        }


        public class HtmlModuleMapper : TypeMapper<Web.Mvc.UI.Canvas.GatewayFree.Modules.HtmlModule, DowJones.Web.Mvc.UI.Canvas.IModule>
        {
            public override Web.Mvc.UI.Canvas.IModule Map(Web.Mvc.UI.Canvas.GatewayFree.Modules.HtmlModule source)
            {
                return new HtmlModule {
                        ModuleId = source.Id,
                        Title = source.Title,
                        Description = source.Description,
                        Position = source.Position,
                        Html = source.Html,
                        Script = source.Script,
                    };
            }
        }
    }
}
