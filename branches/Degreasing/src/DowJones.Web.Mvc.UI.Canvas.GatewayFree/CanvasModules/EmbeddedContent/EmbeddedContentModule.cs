using DowJones.Mapping;
using DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.EmbeddedContent.Editor;

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.EmbeddedContent
{
    public class EmbeddedContentModule : DowJones.Web.Mvc.UI.Canvas.Module
    {
        public const int DefaultHeight = 400;
        public const int DefaultWidth = 400;

        public int Height { get; set; }
        public int Width { get; set; }
        public string Url { get; set; }

        public EmbeddedContentModule()
        {
            Editor = new EmbeddedContentEditor(this);
            Width = DefaultWidth;
            Height = DefaultHeight;
        }


        public class EmbeddedContentModuleMapper : TypeMapper<Web.Mvc.UI.Canvas.GatewayFree.Modules.EmbeddedContentModule, DowJones.Web.Mvc.UI.Canvas.IModule>
        {
            public override Web.Mvc.UI.Canvas.IModule Map(Web.Mvc.UI.Canvas.GatewayFree.Modules.EmbeddedContentModule source)
            {
                return new EmbeddedContentModule
                {
                    CanEdit = true,
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