using System.Collections.Generic;
using System.Web.Mvc;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.UI.Canvas.Controllers;
using Newtonsoft.Json;
using DowJones.Pages;

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
    using DowJones.Web.Mvc.UI.Canvas.GatewayFree.Controllers.Requests;

    public class CanvasController : PageControllerBase
    {
        private IPageManager _pageManager;

        public CanvasController(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        [Route("canvas/modules/positions")]
        [Route("canvas/modules/positions/{method}")]
        public ActionResult UpdateModulePositions()
        {
            UpdateModulePositionsRequest request =
                JsonConvert.DeserializeObject<UpdateModulePositionsRequest>(Request.Form[0]);

            _pageManager.UpdateModulePositions(request.PageId, request.Columns);

            return Json(request, JsonRequestBehavior.AllowGet);
        }
    }
}
