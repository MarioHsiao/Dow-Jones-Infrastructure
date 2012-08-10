using DowJones.Web.Mvc.UI.Canvas.GatewayFree.Properties;

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.EmbeddedContent.Editor
{
    public class EmbeddedContentEditor : CompositeComponentModel
    {
        private readonly EmbeddedContentModule _module;

        [ClientProperty("moduleId")]
        public int? ModuleId
        {
            get { return _module.ModuleId == default(int) ? null : (int?)_module.ModuleId; }
        }

        public int Width
        {
            get { return _module.Width; }
            set { _module.Height = value; }
        }
        
        public int Height
        {
            get { return _module.Height; }
            set { _module.Height = value; }
        }
        
        public string Url
        {
            get { return _module.Url; }
            set { _module.Url = value; }
        }

        public EmbeddedContentEditor(EmbeddedContentModule module = null)
        {
            _module = module ?? new EmbeddedContentModule();
            DataServiceUrl = CanvasSettings.Default.GetDataServiceUrl(typeof(EmbeddedContentModule), Settings.Default);
        }
    }
}