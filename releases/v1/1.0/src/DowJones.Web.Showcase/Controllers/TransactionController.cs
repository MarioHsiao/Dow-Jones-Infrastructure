using System.Web.Mvc;
using DowJones.Tools.Ajax.Converters;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.Threading;
using DowJones.Web.Showcase.Extensions;
using DowJones.Web.Showcase.Models;

namespace DowJones.Web.Showcase.Controllers
{
    [AspCompat]
    public class TransactionController : DowJonesControllerBase
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
    }
}
