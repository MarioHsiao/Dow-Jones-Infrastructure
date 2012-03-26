using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using DowJones.Tools.Ajax;
using DowJones.Utilities.Ajax;
using DowJones.Utilities.Managers;
using DowJones.Utilities.Ajax.Converters;
using DowJones.Utilities.Ajax.TagCloud;
using DowJones.Infrastructure;
using DowJones.Utilities.Managers.Search;

using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Showcase.Extensions;
using DowJones.Web.Showcase.Models;
using DowJones.Web.Mvc.UI.Components.TagCloud;

using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Utils.V1_0;
using PerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using PerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;
using DowJones.Web.Mvc.UI.Components.Models;

namespace DowJones.Web.Showcase.Controllers
{
    public class TagCloudDemoViewModel
    {
        public TagCloudModel TagCloud1 { get; set; }
        public TagCloudModel TagCloud2 { get; set; }
    }

    public class TagCloudController : DowJonesControllerBase
    {
        //
        // GET: /TagCloud/
        private readonly TagConversionManager tagCloudMgr;

        public TagCloudController(TagConversionManager tagconversionManager)
        {
            tagCloudMgr = tagconversionManager; 
        }

        public ActionResult Index()
        {
            var viewModel = new TagCloudDemoViewModel(){
                TagCloud1 = GetTagCloudModel("OnTagItemClick"),
                TagCloud2 = GetTagCloudModel("OnTagItemClick1"),
            };

            return View("Index", viewModel);
        }

        private TagCloudModel GetTagCloudModel(string clickHandler)
        {
            var tag = new TagCloudModel();
            tag.OnTagItemClientClick = clickHandler;
            tag.EnableEventFiring = true;
            return tag;
        }

    }
}
