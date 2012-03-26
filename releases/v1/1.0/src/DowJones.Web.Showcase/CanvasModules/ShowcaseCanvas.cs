using DowJones.Web.Mvc.UI.Canvas;

namespace DowJones.Web.Showcase.CanvasModules
{
    /// <summary>
    /// Just a simple implementation of AbstractCanvas to
    /// render out CanvasModel models used in the demo actions
    /// </summary>
    public class ShowcaseCanvas : AbstractCanvas<Canvas>
    {
        public override string ClientPluginName
        {
            get { return "dj_showcaseCanvas"; }
        }


        public ShowcaseCanvas()
        {
            CssClass += "container-9";
        }


        protected override void InitializeClientPlugin(System.IO.TextWriter writer)
        {
            writer.WriteLine("DJ.UI.ShowcaseCanvas = DJ.UI.AbstractCanvas.extend({});");
            writer.WriteLine("$.plugin('dj_showcaseCanvas', DJ.UI.ShowcaseCanvas);");

            base.InitializeClientPlugin(writer);
        }
    }
}