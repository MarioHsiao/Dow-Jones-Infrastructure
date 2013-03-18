using System;
using DowJones.Ajax.HeadlineList;
using DowJones.Models.Search;
using DowJones.Search;
using DowJones.Search.Navigation;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Track.V1_0;

namespace DowJones.Managers.Search
{
    [Serializable]
    public class SearchResponse
    {
        public uint ContentServerAddress { get; set; }

        public string ContextId { get; set; }

        public Histogram Histogram { get; set; }

        public ResultNavigator Navigators { get; set; }

        public HeadlineListDataResult Results { get; set; }

        public IPerformContentSearchResponse Response { get; set; }

        public Folder AlertInfo { get; set; }

        public AbstractBaseSearchQuery Query { get; set; }

        public RecognizedEntities RecognizedEntities { get; set; }

        public SearchResponse()
        {
            Histogram = new Histogram();
            Navigators = new ResultNavigator();
            Results = new HeadlineListDataResult();
            RecognizedEntities = new RecognizedEntities();
        }
    }
}