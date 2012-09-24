using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.Dash.DataGenerators;
using DowJones.Pages.Common;
using DowJones.Pages.Modules;
using DowJones.Pages.Modules.Templates;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.UI.Canvas;
using DowJones.Web.Mvc.UI.Canvas.Controllers;
using Newtonsoft.Json;
using ModuleState = DowJones.Pages.Modules.ModuleState;
using ZoneLayout = DowJones.Pages.Layout.ZoneLayout;

namespace DowJones.Dash.Website.Controllers
{
    public class DashboardController : PagesControllerBase
    {
        private readonly PageGenerator _pageGenerator;
        private readonly IScriptModuleTemplateManager _templateManager;

        public DashboardController(PageGenerator pageGenerator, IScriptModuleTemplateManager templateManager)
        {
            _pageGenerator = pageGenerator;
            _templateManager = templateManager;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Initialize SignalR
            ScriptRegistry.OnDocumentReady(@"
                $.connection.dashboard.publish = DJ.publish;
                $.connection.hub.start();");
        }

        [Authorize]
        public ActionResult Index()
        {
            var username = User.Identity.Name;

            var page =
                PageRepository.GetPages(SortBy.Position, SortOrder.Ascending)
                    .FirstOrDefault(x => x.OwnerUserId == username);
            
            if(page == null)
            {
                page = _pageGenerator.GeneratePage(username);
                page.ID = PageRepository.CreatePage(page);
            }

            return Canvas(new Canvas { WebServiceBaseUrl = Url.Content("~/dashboard") }, page);
        }

        [Authorize]
        public ActionResult Reset()
        {
            var username = User.Identity.Name;

            var page =
                PageRepository.GetPages(SortBy.Position, SortOrder.Ascending)
                    .FirstOrDefault(x => x.OwnerUserId == username);

            if (page != null)
            {
                PageRepository.DeletePage(page.ID);
                TempData["SuccessMessage"] = "Page reset";
            }

            return RedirectToAction("Index");
        }

//        [OutputCache(Duration = 36000)]
        public ActionResult Gallery(string pageId)
        {
            var templates = _templateManager.GetTemplates()
                        .OrderBy(x => x.Title)
                        .ToArray();

            return PartialView("_Gallery", templates);
        }


        #region Canvas "Services"

        public class UpdateModulePositionsRequest
        {
            public string PageId { get; set; }
            public IEnumerable<IEnumerable<int>> Columns { get; set; }
        }

        [Route("dashboard/modules/resize/{state}")]
        public ActionResult ResizeModule(string pageId, int moduleId, ModuleState state)
        {
            var module = PageRepository.GetModule(pageId, moduleId) as ScriptModule;
            module.ModuleState = state;

            PageRepository.UpdateModule(module);

            return new HttpStatusCodeResult(200);
        }

        [Route("dashboard/modules/positions")]
        [Route("dashboard/modules/positions/{method}")]
        public ActionResult UpdateModulePositions()
        {
            var request = JsonConvert.DeserializeObject<UpdateModulePositionsRequest>(Request.Form[0]);

            var groups = request.Columns.Select(x => new ZoneLayout.Zone(x));

            PageRepository.UpdatePageLayout(request.PageId, new ZoneLayout(groups));

            return new HttpStatusCodeResult(200);
        }

        [HttpDelete]
        [Route("dashboard/modules/id")]
        [Route("dashboard/modules/id/{method}")]
        public ActionResult RemoveModule(string pageId, int moduleId)
        {
            return ResizeModule(pageId, moduleId, ModuleState.Hidden);
        }

        #endregion
    }
}