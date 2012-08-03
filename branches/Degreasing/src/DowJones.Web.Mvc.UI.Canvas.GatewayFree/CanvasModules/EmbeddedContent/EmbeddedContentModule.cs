using DowJones.Mapping;

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.EmbeddedContent
{
    public class EmbeddedContentModule : DowJones.Web.Mvc.UI.Canvas.Module
    {
        public const int DefaultHeight = 300;
        public const int DefaultWidth = 300;

        public int Height { get; set; }
        public int Width { get; set; }
        public string Url { get; set; }


        public class EmbeddedContentModuleMapper : TypeMapper<Web.Mvc.UI.Canvas.GatewayFree.Modules.EmbeddedContentModule, DowJones.Web.Mvc.UI.Canvas.IModule>
        {
            public override Web.Mvc.UI.Canvas.IModule Map(Web.Mvc.UI.Canvas.GatewayFree.Modules.EmbeddedContentModule source)
            {
                return new EmbeddedContentModule
                {
                    ModuleId = source.Id,
                    Title = source.Title,
                    Description = source.Description,
                    Position = source.Position,
                    Width = source.Width.GetValueOrDefault(DefaultWidth),
                    Height = source.Height.GetValueOrDefault(DefaultHeight),
                    Url = source.Url,
                };
            }
        }

	}
}