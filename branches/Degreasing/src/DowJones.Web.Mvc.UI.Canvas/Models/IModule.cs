using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public interface IModule : IViewComponentModel
    {
        bool CanEdit { get; set; }
        bool CanRefresh { get; set; }
        Canvas Canvas { get; set; }
        string CanvasId { get; set; }
        string Description { get; set; }
        IViewComponentModel Editor { get; set; }
        int ModuleId { get; set; }
        ModuleState ModuleState { get; set; }
        string ModuleType { get; }
        int Position { get; set; }
        IList<string> TagCollection { get; set; }
        string Title { get; set; }
    }
}