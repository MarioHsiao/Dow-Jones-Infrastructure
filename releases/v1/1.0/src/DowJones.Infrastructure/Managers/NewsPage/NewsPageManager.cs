using System.Collections.Generic;
using System.Linq;
using Factiva.Gateway.Messages.Assets.PNP.V1_0;
using NewsPages = Factiva.Gateway.Messages.Assets.PNP.V1_0;
using Factiva.Gateway.Utils.V1_0;
using log4net;

namespace DowJones.Utilities.Managers.NewsPage
{
    public class NewsPageManager : AbstractAggregationManager
    {
        private readonly ILog log = LogManager.GetLogger(typeof (NewsPageManager));

        public NewsPageManager(ControlData controlData, string interfaceLanguage) : base(controlData)
        {
        }

        public NewsPageManager(string sessionID, string clientTypeCode, string accessPointCode, string interfaceLangugage) : base(sessionID, clientTypeCode, accessPointCode)
        {
        }

        public void DeleteNewsPage(string pageId)
        {
            var request = new DeleteNewsPageRequest {pageID = pageId};
            Invoke<DeleteNewsPageResponse>(request);
        }

        public void UpdatePositions(ICollection<string> pages)
        {
            if (pages == null || pages.Count == 0)
                return;

            var request = new UpdateNewsPagePositionsRequest();

            var i = 1;

            request.pagePositions = pages.Select(s => new NewsPagePosition
                                                          {
                                                              position = i++, pageID = s
                                                          }).ToArray();
            Invoke<UpdateNewsPagePositionsResponse>(request);
        }

        public void UpdateNewsPageName(string tabId, string tabName)
        {
            var request = new UpdateNewsPageRequest();
            var newsPage = new NewsPages.NewsPage {ID = tabId, name = tabName};

            request.newsPage = newsPage;

            Invoke<UpdateNewsPageResponse>(request);
        }
        
        protected override ILog Log
        {
            get { return log; }
        }
    }
}
