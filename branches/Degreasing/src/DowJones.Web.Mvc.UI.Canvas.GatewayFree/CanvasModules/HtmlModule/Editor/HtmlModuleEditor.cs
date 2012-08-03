namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.HtmlModule.Editor
{
    public class HtmlModuleEditor : DowJones.Web.Mvc.UI.ViewComponentModel
    {
        private readonly HtmlModule _module;

        public HtmlModuleEditor(HtmlModule module)
        {
            _module = module;
        }
    }
}