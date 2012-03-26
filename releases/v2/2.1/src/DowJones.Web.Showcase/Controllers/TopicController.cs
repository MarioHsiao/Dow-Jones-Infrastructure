using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using DowJones.Assemblers.Search;
using DowJones.Managers.Search;
using DowJones.Search;
using DowJones.Web.Mvc.Search.Requests;
using DowJones.Web.Mvc.Search.Requests.Freetext;
using DowJones.Web.Mvc.Search.UI.Components.Builders;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class TopicController : ControllerBase
    {
        private SourceListService listService;
        public TopicController(SourceListService listService)
        {
            this.listService = listService;
        }

        public ActionResult Index()
        {
//            var request = new FreeTextSearchRequest
//                              {
//                                  FreeText = "AJS", 
//                                  FreeTextIn = SearchFreeTextArea.HeadlineAndLeadParagraph, 
//                                  DateRange = SearchDateRange.LastTwoYears
//                              };
//
//           
//            var list = listService.GetSourceList();
          

            return new EmptyResult();
        }

       
    }
}