using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.Pages;
using DowJones.Pages.Modules;
using DowJones.Web.Mvc.UI.Canvas.Controllers;
using DowJones.Web.Mvc.UI.Canvas.GatewayFree.Modules;

namespace DowJones.DegreasedDashboards.Website.Controllers
{
    public class ModuleGalleryController : DashboardControllerBase
    {
        private readonly IPageManager _pageManager;
        private readonly IModuleTemplateManager _moduleTemplateManager;

        public ModuleGalleryController(IPageManager pageManager, IModuleTemplateManager moduleTemplateManager)
        {
            _pageManager = pageManager;
            _moduleTemplateManager = moduleTemplateManager;
        }

        public ActionResult Create(string pageId, int templateId, string title, IEnumerable<ModuleOption> options)
        {
            var moduleTemplate = _moduleTemplateManager.GetTemplate(templateId);

            if(moduleTemplate == null)
                return new HttpStatusCodeResult(500, "Invalid Template ID: " + templateId);

            var module = new HtmlModule {
                    Title = title,
                    Html = (string)moduleTemplate.MetaData.FirstOrDefault(x => x.Name == "Html"),
                    Script = (string)moduleTemplate.MetaData.FirstOrDefault(x => x.Name == "Script"),
                    Options = options,
                };

            var moduleId = _pageManager.AddModuleToPage(pageId, module);

            return Json(new { moduleId });
        }
    }
}