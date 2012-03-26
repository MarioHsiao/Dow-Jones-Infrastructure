using System.Net;
using System.Web.Mvc;
using DowJones.Assemblers.Headlines;
using DowJones.Search;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.CompositeHeadline;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using DowJones.Web.Mvc.UI.Components.Models;
using DowJones.Web.Showcase.Extensions;
using DowJones.Web.Showcase.Models;
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
            return RedirectToAction("Single", new {q});
        }

        public ActionResult Single(string q = "asp.net", ContentSearchMode? mode = null)
        {
            HeadlineListModel showcaseHeadlineListModel = _headlineListManager.PerformSearch(q, ContentSearchMode.ContentServer);
            var model = new DowJones.Web.Mvc.UI.Components.HeadlineList.HeadlineListModel(showcaseHeadlineListModel.Result);
            model.ShowDuplicates = ShowDuplicates.On;
            model.ShowCheckboxes = true;
            return View("Simple", model);
        }

        public ActionResult Composite( string q = "an:DJFVW00020120119e81jliwkq", ContentSearchMode? mode = null )
        {
            var showcaseHeadlineListModel = _headlineListManager.PerformContentSearch(q, new HeadlineUtility(ControlData, Preferences).GetThumbNail);
            var model = new CompositeHeadlineModel()
            {
                FirstResultIndex = (int)showcaseHeadlineListModel.resultSet.first.Value,
                LastResultIndex = (int)showcaseHeadlineListModel.resultSet.count.Value,
                TotalResultCount = (int)showcaseHeadlineListModel.hitCount.Value,
                HeadlineList = new DowJones.Web.Mvc.UI.Components.HeadlineList.HeadlineListModel(showcaseHeadlineListModel)
                {
                    ShowDuplicates = ShowDuplicates.On,
                    ShowCheckboxes = true,
                    ShowThumbnail = true,
                    PostProcessingOptions = new[] { 
                        PostProcessingOptions.Save, 
                        PostProcessingOptions.Print, 
                        PostProcessingOptions.Email,
                        PostProcessingOptions.PressClips
                    }
                },
                PostProcessing = new PostProcessing(new[] { 
                    PostProcessingOptions.Read, 
                    PostProcessingOptions.Save, 
                    PostProcessingOptions.Print, 
                    PostProcessingOptions.Email,
                    PostProcessingOptions.Export,
                    PostProcessingOptions.PressClips
                })
                                     {
                                         EnableDuplicateOption = true,
                                     },
                HeadlineSortOptions = new EnumSelectListWithTranslatedText<SortOrder>(SortOrder.Relevance)
            };

            return View("Composite", model);
        }

    }
}
