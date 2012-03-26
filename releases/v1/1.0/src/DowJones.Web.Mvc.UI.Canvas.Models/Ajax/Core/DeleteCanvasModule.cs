using DowJones.Tools.Ajax;

namespace DowJones.Web.Mvc.UI.Canvas.Ajax.Core
{
    public class DeleteCanvasModuleFromCanvasRequestDelegate : BaseCanvasAjaxRequestDelegate
    {
        public string moduleId;
    }
    
    public class DeleteCanvasModuleFromCanvasResponseDelegate : AbstractAjaxResponseDelegate
    {
    }
}
