using System.Net;
using System.Web.Mvc;
using DowJones.Assemblers.Headlines;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using DowJones.Web.Showcase.Extensions;
using DowJones.Web.Showcase;
using DowJones.Web.Showcase.Models;
using DowJones.Web.Mvc.UI.Components.CompositeHeadline;
using DowJones.Web.Mvc.UI.Components.Models;
using System.Collections.Generic;
using HeadlineListModel = DowJones.Web.Showcase.Models.HeadlineListModel;

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
            return RedirectToAction("Simple", new { q });
        }

        public ActionResult Simple(string q = "asp.net", ContentSearchMode? mode = null)
        {
            HeadlineListModel showcaseHeadlineListModel = _headlineListManager.PerformSearch(q, ContentSearchMode.ContentServer);
            var model = new DowJones.Web.Mvc.UI.Components.HeadlineList.HeadlineListModel(showcaseHeadlineListModel.Result);
            model.ShowDuplicates = ShowDuplicates.On;
            model.ShowCheckboxes = true;
            return View("Simple", model);
        }

        public ActionResult Composite(string q = "asp.net", ContentSearchMode? mode = null)
        {

            HeadlineListModel showcaseHeadlineListModel = _headlineListManager.PerformSearch(q, ContentSearchMode.ContentServer);
            CompositeHeadlineModel model = new CompositeHeadlineModel()
            {
                FirstResultIndex = (int)showcaseHeadlineListModel.Result.resultSet.first.Value,
                LastResultIndex = (int)showcaseHeadlineListModel.Result.resultSet.count.Value,
                TotalResultCount = (int)showcaseHeadlineListModel.Result.hitCount.Value,
                HeadlineList = new DowJones.Web.Mvc.UI.Components.HeadlineList.HeadlineListModel(showcaseHeadlineListModel.Result)
                {
                    ShowDuplicates = ShowDuplicates.On,
                    ShowCheckboxes = true,
                },
                PostProcessing = new PostProcessing(new[] { 
                    PostProcessingOptions.Read, 
                    PostProcessingOptions.Save, 
                    PostProcessingOptions.PrintLabel, 
                    PostProcessingOptions.Email
                })
            };

            return View("Composite", model);
        }

    }
}
