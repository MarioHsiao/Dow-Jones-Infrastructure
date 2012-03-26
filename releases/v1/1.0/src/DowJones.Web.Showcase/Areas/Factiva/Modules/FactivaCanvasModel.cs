using DowJones.Web.Mvc.UI.Canvas;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Showcase.Areas.Factiva.Modules
{
    public class FactivaCanvasModel : Canvas
    {
        [ClientProperty("swapModuleServiceUrl")]
        public string SwapModuleServiceUrl { get; set; }

        public FactivaCanvasModel()
        {
            SwapModuleServiceUrl = Settings.Default.GetDataServiceUrl("SwapModuleServiceUrl");
        }
    }
}
