using DowJones.Web.Mvc.UI.Canvas.Editors;
using DowJones.Web.Mvc.UI.Canvas;

namespace DowJones.Web.Showcase.Areas.Factiva.Models
{
    public class ModuleGardenModel : Canvas
    {
        public SwapModuleEditor SwapModuleEditor { get; set; }
        public SyndicationModuleEditor SyndicationEditor { get; set; }
        

        public ModuleGardenModel()
        {
            SwapModuleEditor = new SwapModuleEditor { 
                ModuleType = string.Empty // needed to initialize accessors on client side
            };


            SyndicationEditor = new SyndicationModuleEditor();

        }
    }
}