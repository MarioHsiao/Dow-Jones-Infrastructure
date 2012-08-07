using System.Collections.Generic;
using System.Web.Mvc;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.UI.Canvas.Controllers;

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.Controllers
{
    public class CanvasController : PageControllerBase
    {
        [JsonModelBinder]
        public class UpdateModulePositionsRequest
        {
            public string PageId { get; set; }
            public List<int[]> Columns { get; set; }
        }

        [Route("canvas/modules/positions")]
        [Route("canvas/modules/positions/{method}")]
        public ActionResult UpdateModulePositions(UpdateModulePositionsRequest request)
        {
            return Json(request, JsonRequestBehavior.AllowGet);
        }
    }
}
