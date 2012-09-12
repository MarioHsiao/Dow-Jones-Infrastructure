using DowJones.DependencyInjection;
using DowJones.Pages;

namespace DowJones.Web.Mvc.UI.Canvas.Controllers
{
    public class PagesControllerBase : DashboardControllerBase
    {
        [Inject("Avoiding base class controller injection")]
        public IPageRepository PageRepository { get; set; }

        [Inject("Avoiding base class controller injection")]
        public IPageSubscriptionManager SubscriptionManager { get; set; }

        protected override CanvasModuleViewResult AddModuleInternal(int id, string pageId, string callback)
        {
            PageRepository.AddModuleToPage(pageId, id);
            return ModuleInternal(id, pageId, callback);
        }

        protected override Pages.Modules.Module GetModule(int id, string pageId)
        {
            return PageRepository.GetModule(pageId, id);
        }

        protected override Page GetPage(string id)
        {
            return PageRepository.GetPage(id);
        }

        protected override string SubscribeToPage(string id, int positionNumber)
        {
            return SubscriptionManager.SubscribeToPage(id, positionNumber);
        }
    }
}