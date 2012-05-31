using System.Net;
using System.Web.Mvc;
using DowJones.Ajax.HeadlineList;
using DowJones.Assemblers.Headlines;
using DowJones.Search;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.Common.Types;
using DowJones.Web.Mvc.UI.Components.CompositeHeadline;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using DowJones.Web.Mvc.UI.Components.Models;
using DowJones.Web.Showcase.Extensions;
using DowJones.Web.Showcase.Models;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    [HandleError(ExceptionType = typeof (WebException), View = "RssError")]
    public class HeadlineListCarouselController : ControllerBase
    {
        private readonly HeadlineListConversionManager _headlineListManager;

        public HeadlineListCarouselController(HeadlineListConversionManager headlineListManager)
        {
            _headlineListManager = headlineListManager;
        }

        public ActionResult Index(string q = "dow jones", ContentSearchMode? mode = null)
        {
            return RedirectToAction("Simple", new {q});
        }

        public ActionResult Simple(string q = "rst:DJFVW OR rst:DJCOMMDJFVW se:\"People\" pd:>03/19/2010", ContentSearchMode? mode = null)
        {
            var showcaseHeadlineListModel = _headlineListManager.PerformSearch(q, ContentSearchMode.ContentServer);
            var model = new HeadlineListModel(showcaseHeadlineListModel.Result)
                            {
                                ShowDuplicates = ShowDuplicates.On, 
                                ShowCheckboxes = true
                            };

            return View("Simple", model);
        }

        public ActionResult Carousel(string  accession="",string q = "rst:ctgfpe OR rst:ctgfvc se:\"VC Fund News\" OR se:\"Fund News\" OR se:\"Fund-raising Roundup\" OR se:\"fund raising roundup\" pd:05/31/2010 OR pd:>05/31/2010 pd:05/31/2012 OR pd:<05/31/2012", ContentSearchMode? mode = null)
        {
            var showcaseHeadlineListModel = _headlineListManager.PerformSearch(q, ContentSearchMode.ContentServer);
           // var model  
            var carousel = new HeadlineListCarouselModel(){
                SelectedAccessionNo = accession,
                NumberOfHeadlinesToScrollBy = 3,
                AutoScrollSpeed = "400",
                HeadlineList = new HeadlineListModel(showcaseHeadlineListModel.Result)
                {
                    ShowDuplicates = ShowDuplicates.On,
                    ShowCheckboxes = true,
                    ShowThumbnail = ThumbnailDisplayType.Inline,
                    DisplaySnippets = SnippetDisplayType.Inline
                }                
            };
            return View("Carousel", carousel);
        }


        public ActionResult Composite(string q = "an:DJFVW00020120119e81jliwkq", ContentSearchMode? mode = null)
        {
            var showcaseHeadlineListModel = _headlineListManager.PerformContentSearch(q, new HeadlineUtility(ControlData, Preferences).GetThumbNail);
            var model = new CompositeHeadlineModel
                            {
                                FirstResultIndex = (int) showcaseHeadlineListModel.resultSet.first.Value,
                                LastResultIndex = (int) showcaseHeadlineListModel.resultSet.count.Value,
                                TotalResultCount = (int) showcaseHeadlineListModel.hitCount.Value,
                                HeadlineList = new HeadlineListModel(showcaseHeadlineListModel)
                                                   {
                                                       ShowDuplicates = ShowDuplicates.On,
                                                       ShowCheckboxes = true,
                                                       ShowThumbnail = ThumbnailDisplayType.Hybrid,
                                                       PostProcessingOptions = new[]
                                                                                   {
                                                                                       PostProcessingOptions.Save,
                                                                                       PostProcessingOptions.Print,
                                                                                       PostProcessingOptions.Email,
                                                                                       PostProcessingOptions.PressClips
                                                                                   }
                                                   },
                                PostProcessing = new PostProcessing(new[]
                                                                        {
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