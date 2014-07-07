using System.Web.Mvc;
using DowJones.Assemblers.Headlines;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.Threading;
using DowJones.Web.Showcase.Extensions;
using DowJones.Web.Showcase.Models;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    [AspCompat]
    public class TransactionController : ControllerBase
    {
        private readonly HeadlineListConversionManager headlineListManager; 

        public TransactionController(HeadlineListConversionManager headlineListManager)
        {
            this.headlineListManager = headlineListManager;
        }

        public ActionResult Index(string q = "microsoft", ContentSearchMode? mode = null)
        {
            Log.Debug("Entering ActionResult ");
            headlineListManager.PerformSearch(q, mode);
            return View("Index");
        }


        public JsonResult Email()
        {
            return new JsonResult { Data = new { deliveryNumber = 4, email = "david.dacosta@dowjones.com" } };
        }
    }
}
