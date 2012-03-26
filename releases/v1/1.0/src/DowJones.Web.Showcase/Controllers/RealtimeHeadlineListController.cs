using System.Web.Mvc;
using DowJones.Utilities.Ajax.Converters;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.RealtimeHeadlineList;
using Factiva.Gateway.Managers;
using Factiva.Gateway.Messages.RTQueue.V1_0;
using Factiva.Gateway.Services.V1_0;

namespace DowJones.Web.Showcase.Controllers
{
    public class RealtimeHeadlineListController : DowJonesControllerBase
    {

        public ActionResult Index()
        {
            return Components("Index", GetRealtimeHeadlines());
        }

        /// <summary>
        /// Returne Headlines
        /// </summary>
        /// <returns></returns>
        private static ContentContainerModel GetRealtimeHeadlines()
        {
            var realtimeHeadlineListManager = new RealtimeHeadlinelistConversionManager("en", "on,-05:00|1,on", ClockType.TwelveHours);
            var realtimeHeadlineListModel = new RealtimeHeadlineListModel
                                                {
                                                    ClockType = ClockType.TwelveHours,
                                                    DateTimeFormatingPreference = "on,-05:00|1,on",
                                                    MaxHeadlinesToReturn = 10,
                                                    Tokens =
                                                        {
                                                            controlTitleTkn = "Dow Jones | Latest News",
                                                            queueTkn = "Queue",
                                                            viewAllTkn = "View All"
                                                        },
                                                       OnHeadlineClick = "HeadlineClick",
                                                    ContainerWidth = 374
                                                };
            var sharedAlertResponse = CreateSharedAlert("IB_LN_ALL_IND");
            if (sharedAlertResponse != null)
            {
                realtimeHeadlineListModel.AlertContext = sharedAlertResponse.AlertContext;
                realtimeHeadlineListModel.Result = realtimeHeadlineListManager.Process(sharedAlertResponse, null, null);
            }
            return new ContentContainerModel(new IViewComponentModel[] { realtimeHeadlineListModel });
        }

        /// <summary>
        /// Returns Alert Context
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        private static CreateSharedAlertResponse CreateSharedAlert(string topic)
        {
            var request = new CreateSharedAlertRequest {SharedTopic = topic, MaxHeadlinesToReturn = 10};
            //var cData = ControlDataManager.Clone(ControlData);
            var cData = ControlDataManager.GetLightWeightUserControlData("joyful", "joyful", "16");
            var serviceResponse = RTQueueAggregationService.CreateSharedAlert(ControlDataManager.Clone(cData), request);
            return serviceResponse.rc == 0 ? serviceResponse : null;
        }

    }
}
