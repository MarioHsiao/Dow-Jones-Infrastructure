using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public class CanvasModuleCollection : List<ICanvasModule>
    {
        public IViewComponent Owner { get; set; }

        public CanvasModuleCollection(IViewComponent owner)
        {
            Owner = owner;
        }
    }
}
