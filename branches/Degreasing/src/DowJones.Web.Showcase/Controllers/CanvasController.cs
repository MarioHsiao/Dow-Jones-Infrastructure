using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using DowJones.Web.Showcase.Models;
using DowJones.Web.Showcase.Modules.Empty;

namespace DowJones.Web.Showcase.Controllers
{
    public class CanvasController : CanvasControllerBase
    {
        private static readonly IEnumerable<PortalHeadlineInfo> Headlines = new[] {
                new PortalHeadlineInfo { Title = "Google is awesome"}, 
                new PortalHeadlineInfo { Title = "Microsoft is awesome"}, 
                new PortalHeadlineInfo { Title = "Apple is awesome"}, 
                new PortalHeadlineInfo { Title = "RIM is bankrupt"}, 
            };


        public ActionResult Index( )
        {
            return Canvas(
                new EmptyModule { CanvasId = "CANVAS_1", ModuleId = 1234, Title = "Demo Module #1", ContentUrl = Url.Action("Content")},
                new EmptyModule { CanvasId = "CANVAS_1", ModuleId = 5678, Title = "Editable Module", CanEdit = true }
            );
        }

        public string Content(int sleep = 0)
        {
            Thread.Sleep(TimeSpan.FromSeconds(sleep));
            return string.Format("Current time: {0}", DateTime.Now);
        }

        public override ActionResult Module(string id, string pageId, string callback)
        {
            var headlines = new PortalHeadlineListResultSet(Headlines);
            var portalHeadlines = new PortalHeadlineListModel(new PortalHeadlineListDataResult(headlines));
            var module = new PortalHeadlineLists
                             {
                                 CanvasId = "CANVAS_1", 
                                 ModuleId = new Random().Next(99999), 
                                 Title = "Portal Headlines", 
                                 CanEdit = true,
                                 PortalHeadlines = new [] { portalHeadlines },
                             };

            return Module(module, callback);
        }

        public ActionResult JsonpModule(string id, string pageId, string jsonpCallback, string callback = null)
        {
            var module = (ViewComponentViewResult)Module(id, pageId, callback);
            return new JsonpViewComponentResult(module) { Callback = jsonpCallback };
        }

		public ActionResult ArticlePopup()
		{
			return PartialView("_PopupArticleContent");
		}
    }
}