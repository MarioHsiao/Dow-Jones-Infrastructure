using System.Net;
using System.Web.Mvc;
using DowJones.Web.Showcase.Extensions;
using DowJones.Web.Showcase.Models;
using DowJones.Tools.Ajax.Converters;

namespace DowJones.Web.Showcase.Controllers
{
    [HandleError(ExceptionType = typeof(WebException), View = "RssError")]
    public class HeadlineListController : Controller
    {
        private readonly HeadlineListConversionManager _headlineListManager;

        public HeadlineListController(HeadlineListConversionManager headlineListManager)
        {
            _headlineListManager = headlineListManager;
        }

        public ActionResult Index(string q = "dow jones", ContentSearchMode? mode = null)
        {
            return RedirectToAction("Single", new {q});
        }

        public ActionResult Single(string q = "asp.net", ContentSearchMode? mode = null)
        {
            HeadlineListModel headlineListModel = _headlineListManager.PerformSearch(q, ContentSearchMode.ContentServer);
            var model = new DowJones.Web.Mvc.UI.Components.Models.HeadlineList.HeadlineListModel(headlineListModel.Result);
            model.ShowDuplicates = true;
            return View("Single", model);
        }

    }
}
