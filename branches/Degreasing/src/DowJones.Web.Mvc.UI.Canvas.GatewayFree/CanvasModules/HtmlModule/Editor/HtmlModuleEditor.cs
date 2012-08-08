using DowJones.Web.Mvc.UI.Canvas.GatewayFree.Properties;

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.HtmlModule.Editor
{
    public class HtmlModuleEditor : CompositeComponentModel
    {
        private readonly HtmlModule _module;

        [ClientProperty("moduleId")]
        public int? ModuleId
        {
            get { return _module.ModuleId == default(int) ? null : (int?)_module.ModuleId; }
        }

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
            DataServiceUrl = CanvasSettings.Default.GetDataServiceUrl(typeof(HtmlModule), Settings.Default);
        }
    }
}