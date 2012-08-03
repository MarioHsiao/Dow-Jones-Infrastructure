namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.HtmlModule.Editor
{
    public class HtmlModuleEditor : DowJones.Web.Mvc.UI.ViewComponentModel
    {
        private readonly HtmlModule _module;

        public string Html
        {
            get { return _module.Html; }
            set { _module.Html = value; }
        }

        public string Script
        {
            get { return _module.Script; }
            set { _module.Script = value; }
        }

        public HtmlModuleEditor(HtmlModule module)
        {
            _module = module;
        }
    }
}