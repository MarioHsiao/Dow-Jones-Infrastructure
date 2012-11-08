using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.Pages.Modules;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.UI.Canvas.Controllers;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.Controllers.Requests
{
    public class UpdateModulePositionsRequest
    {
        public string PageId { get; set; }
        public IEnumerable<IEnumerable<int>> Columns { get; set; }
    }
}

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.Controllers
{
    using Requests;

    public class CanvasController : PagesControllerBase
    {
        [Route("canvas/modules/resize/{state}")]
        public ActionResult ResizeModule(string pageId, int moduleId, DowJones.Pages.Modules.ModuleState moduleState)
        {
            var module = PageRepository.GetModule(pageId, moduleId) as ScriptModule;
            module.ModuleState = moduleState;

            PageRepository.UpdateModule(module);

            return new HttpStatusCodeResult(200);
        }

        [Route("canvas/modules/positions")]
        [Route("canvas/modules/positions/{method}")]
        public ActionResult UpdateModulePositions()
        {
            var request = ParsePostData<UpdateModulePositionsRequest>();

            var groups = request.Columns.Select(x => new Pages.Layout.ZoneLayout.Zone(x));

            PageRepository.UpdatePageLayout(request.PageId, new Pages.Layout.ZoneLayout(groups));

            return new HttpStatusCodeResult(200);
        }

        [HttpDelete]
        [Route("canvas/modules/id")]
        [Route("canvas/modules/id/{method}")]
        public ActionResult RemoveModule(string pageId, int moduleId)
        {
            PageRepository.RemoveModulesFromPage(pageId, moduleId);

            return new HttpStatusCodeResult(200);
        }

        private T ParsePostData<T>()
        {
            return JsonConvert.DeserializeObject<T>(Request.Form[0]);
        }
    }
}
