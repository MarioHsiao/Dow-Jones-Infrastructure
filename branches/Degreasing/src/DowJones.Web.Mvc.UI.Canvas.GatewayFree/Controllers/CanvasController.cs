﻿using System.Collections.Generic;
using System.Web.Mvc;
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

    public class CanvasController : DashboardControllerBase
    {
        [Route("canvas/modules/positions")]
        [Route("canvas/modules/positions/{method}")]
        public ActionResult UpdateModulePositions()
        {
            var request = ParsePostData<UpdateModulePositionsRequest>();

            PageManager.UpdateModulePositions(request.PageId, request.Columns);

            return new HttpStatusCodeResult(200);
        }

        [HttpDelete]
        [Route("canvas/modules/id")]
        [Route("canvas/modules/id/{method}")]
        public ActionResult RemoveModule(string pageId, int moduleId)
        {
            PageManager.RemoveModules(pageId, moduleId);

            return new HttpStatusCodeResult(200);
        }

        private T ParsePostData<T>()
        {
            return JsonConvert.DeserializeObject<T>(Request.Form[0]);
        }
    }
}
