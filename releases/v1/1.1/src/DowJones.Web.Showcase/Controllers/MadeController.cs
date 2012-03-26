using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DowJones.Ajax.Converters;
using DowJones.Tools.Ajax.Converters;
using DowJones.Utilities.Ajax.Converters;
using DowJones.Web.Showcase.Extensions;
using DowJones.Web.Showcase.Models;
using ControlModels = DowJones.Web.Mvc.UI.Components.Models;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using System.Xml.Serialization;

namespace DowJones.Web.Showcase.Controllers
{
    public class MadeController : DowJones.Web.Mvc.DowJonesControllerBase
    {
        ArticleConversionManager _articleManager;
        HeadlineListConversionManager _headlineListManager;
        TagConversionManager _tagCloudManager;
        DowJones.Ajax.Converters.NavigatorConversionManager _navigatorManager;
        DowJones.Utilities.Formatters.Globalization.DateTimeFormatter _formatter = new DowJones.Utilities.Formatters.Globalization.DateTimeFormatter("en");
        private string _binaryHandlerUrl;

        public MadeController()
        {
            _articleManager = new ArticleConversionManager(_binaryHandlerUrl, _formatter);
            _headlineListManager = new HeadlineListConversionManager(_formatter);
            _tagCloudManager = new TagConversionManager();
            _navigatorManager = new Ajax.Converters.NavigatorConversionManager();
        }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            string baseUrl = requestContext.HttpContext.Request.Url.Scheme + "://" + requestContext.HttpContext.Request.Url.Authority + requestContext.HttpContext.Request.ApplicationPath.TrimEnd('/') + '/';
            _binaryHandlerUrl = baseUrl + "DowJones.Utilities.Handlers.ArticleControl.ContentHandler.ashx";
            base.Initialize(requestContext);
        }

        /// <summary>
        /// HeadlineList, Article, Discovery.  Simple Example.
        /// </summary>
        /// <param name="q">The q.</param>
        /// <returns></returns>
        public ActionResult Index(string q = "Dow Jones")
        {
            var m = GetModel(q);
            //SaveModel(m, q);
            return View(m);
        }

        public ActionResult FullView()
        {
            return View();
        }
        public ActionResult FramedView()
        {
            return View();        
        }
        public ActionResult RichView()
        {
            return View();
        }


        #region Locals


        Ajax.Navigator.Navigator GetNavigator(Factiva.Gateway.Messages.Search.V2_0.NavigatorCollection collection, string id)
        {
            return _navigatorManager.Process((from cn in collection where cn.Id == id select cn).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult ResultSet(List<SearchTerm> search)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach(var st in search)
            {
                sb.Append(st.ToString());
            }
            var m = GetModel(sb.ToString());
            //SaveModel(m, sb.ToString());
            return Json(m);
        }
        [HttpPost]
        public ActionResult Article(string accessionNumber)
        {
            var model = GetArticle(accessionNumber);
            //SaveArticle(model);
            return Json(model);
        }
        /*jsonp*/
        [HttpGet]
        public string ResultSet(string callback, string search)
        {
            var serializer = new JavaScriptSerializer();
            var searchList = serializer.Deserialize(search, typeof (IEnumerable<SearchTerm>)) as IEnumerable<SearchTerm>;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var st in searchList)
            {
                sb.Append(st.ToString());
            }
            var m = GetModel(sb.ToString());
            Response.ContentType = "application/json";
            return callback + "(" + serializer.Serialize(m) + ")";
        }
        [HttpGet]
        public string Article(string callback, string accessionNumber)
        {
            var m = GetArticle(accessionNumber);
            var serializer = new JavaScriptSerializer();
            Response.ContentType = "application/json";
            return callback + "(" + serializer.Serialize(m) + ")";
        }
        /*end jsonp*/


        /// <summary>
        /// Saves the model.  This is to get some local data to play with when the gateway is down.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="searchTerm">The search term.</param>
        void SaveModel(MadeViewModel model, string searchTerm)
        {
            return;
/*
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MadeViewModel));

                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(@"C:\Dev\DowJones\Assembla\branches\mn-dev\static\_data\mvm-" + searchTerm + ".xml"))
                {
                    serializer.Serialize(writer, model);
                }
                
                System.Web.Script.Serialization.JavaScriptSerializer jsserializer
                    = new System.Web.Script.Serialization.JavaScriptSerializer();
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(@"C:\Dev\DowJones\Assembla\branches\mn-dev\static\_data\mvm-" + searchTerm + ".json"))
                {                    
                    writer.Write(jsserializer.Serialize(model));
                }
                //SaveArticle(model.FirstArticle);

            }
            catch
            {
                throw;
            }            
*/
        }
        void SaveArticle(DowJones.Web.Mvc.UI.Components.Models.Article.ArticleModel model)
        {
            return;
/*
            XmlSerializer serializer = new XmlSerializer(typeof(DowJones.Web.Mvc.UI.Components.Models.Article.ArticleModel));
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(@"C:\Dev\DowJones\Assembla\branches\mn-dev\static\_data\article-" + model.AccessionNumber + ".xml"))
            {
                serializer.Serialize(writer, model);
            }
            System.Web.Script.Serialization.JavaScriptSerializer jsserializer
                    = new System.Web.Script.Serialization.JavaScriptSerializer();


            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(@"C:\Dev\DowJones\Assembla\branches\mn-dev\static\_data\article-" + model.AccessionNumber + ".json"))
            {
                writer.Write(jsserializer.Serialize(model));
            }
*/

        }
        MadeViewModel GetModel(string q)
        {
            ControlModels.HeadlineList.HeadlineListModel headlineModel = null;
            ControlModels.Article.ArticleModel articleModel = null;
            ControlModels.Discovery.DiscoveryModel discoveryModel = null;

            var controlData = base.ControlData;
            Factiva.Gateway.Messages.Search.V2_0.ContentSearchResult contentResult = null;
            var headlineResult = _headlineListManager.PerformContentSearch(q, controlData, out contentResult);
            headlineModel = new ControlModels.HeadlineList.HeadlineListModel(headlineResult) { ShowCheckboxes = true, ShowDuplicates = true };

            if (headlineModel.Headlines != null)
            {
                discoveryModel = new ControlModels.Discovery.DiscoveryModel();
                var keywordlist = discoveryModel.AddDiscoveryList("Keywords", ControlModels.Discovery.DiscoveryChartTypes.TagCloud, _tagCloudManager.Process(contentResult));
                keywordlist.DiscoveryItems.Sort((a, b) => a.Value.CompareTo(b.Value));
                var dateNavigator = _navigatorManager.Process(contentResult.TimeNavigatorSet.NavigatorCollection.FirstOrDefault());
                foreach (var b in dateNavigator.BucketCollection)
                {
                    b.Id = DateTime.Parse(b.Id.Substring(4, 2) + "/1/" + b.Id.Substring(0, 4)).ToString("MMM").ToLower();
                }
                discoveryModel.AddDiscoveryList("Dates", ControlModels.Discovery.DiscoveryChartTypes.DateNavigator, dateNavigator);
                var codeNavCollection = contentResult.CodeNavigatorSet.NavigatorCollection;
                discoveryModel.AddDiscoveryList("Companies", ControlModels.Discovery.DiscoveryChartTypes.HitCount, GetNavigator(codeNavCollection, "co"), "FDS");
                discoveryModel.AddDiscoveryList("Subjects", ControlModels.Discovery.DiscoveryChartTypes.HitCount, GetNavigator(codeNavCollection, "ns"), "NS");
                discoveryModel.AddDiscoveryList("People", ControlModels.Discovery.DiscoveryChartTypes.HitCount, GetNavigator(codeNavCollection, "pe"), "PE");
                discoveryModel.AddDiscoveryList("Regions", ControlModels.Discovery.DiscoveryChartTypes.HitCount, GetNavigator(codeNavCollection, "re"), "RE");

                var firstHeadline = headlineModel.Headlines.FirstOrDefault();
                if (firstHeadline != null)
                {
                    articleModel = GetArticle(firstHeadline.AccessionNumber);
                }
            }
            ViewBag.SearchTerm = q;
            ViewBag.Query = q;
            return new MadeViewModel { HeadlineList = headlineModel, Discovery = discoveryModel ?? new ControlModels.Discovery.DiscoveryModel(), FirstArticle = (articleModel ?? new ControlModels.Article.ArticleModel()) };
        }

        ControlModels.Article.ArticleModel GetArticle(string accessionNumber)
        {
            ControlModels.Article.ArticleModel articleModel = _articleManager.GetArticle(accessionNumber); 
            return articleModel;
        }

        #endregion

    }
}
