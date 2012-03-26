using DowJones.Tools.Ajax;

namespace DowJones.Web.Mvc.UI.Canvas.Ajax.Core
{
    public class CloneCanvasModuleRequestDelegate : BaseCanvasAjaxRequestDelegate
    {
        public int moduleId;
    }

    public class CloneCanvasModuleResponseDelegate : AbstractAjaxResponseDelegate
    {
        public int clonedModuleId;
        public int newModuleId;
    }
}