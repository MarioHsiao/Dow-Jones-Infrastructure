using DowJones.Tools.Ajax;
using State = Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleState;

namespace DowJones.Web.Mvc.UI.Canvas.Ajax.Core
{
    public class UpdateCanvasModuleStateRequestDelegate : BaseCanvasAjaxRequestDelegate
    {
        public string moduleId;
        public State state;
    }

    public class UpdateCanvasModuleStateResponseDelegate : AbstractAjaxResponseDelegate
    {
    }
}