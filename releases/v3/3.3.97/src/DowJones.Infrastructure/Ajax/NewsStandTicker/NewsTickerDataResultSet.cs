using System.Collections.Generic;
using DowJones.Formatters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DowJones.Ajax.NewsStandTicker
{
    [DataContract(Name = "newsTickerDataResultSet", Namespace = "")]
    public class NewsTickerDataResultSet
    {
        private WholeNumber _count;

        [DataMember(Name = "count")]
        [JsonProperty("count")]
        public WholeNumber Count
        {
            get
            {
                return _count ?? (_count = new WholeNumber(Headlines.Count));
            }
            set { _count = value; }
        }

        private WholeNumber _first;

        [DataMember(Name = "first")]
        [JsonProperty("first")]
        public WholeNumber First
        {
            get
            {
                return _first ?? (_first = new WholeNumber(0));
            }
            set { _first = value; }
        }

        private WholeNumber _duplicateCount;

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        [DataMember(Name = "duplicateCount")]
        [JsonProperty("duplicateCount")]
        public WholeNumber DuplicateCount
        {
            get { return _duplicateCount ?? (_duplicateCount = new WholeNumber(0)); }
            set { _duplicateCount = value; }
        }


        private List<NewsTickerInfo> _headlines;

        [DataMember(Name = "headlines")]
        [JsonProperty("headlines")]
        public List<NewsTickerInfo> Headlines
        {
            get
            {
                return _headlines ?? (_headlines = new List<NewsTickerInfo>());
            }
            set { _headlines = value; }
        }
    }
}
