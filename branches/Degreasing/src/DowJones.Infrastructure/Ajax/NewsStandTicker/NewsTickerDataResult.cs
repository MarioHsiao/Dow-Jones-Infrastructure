using DowJones.Formatters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DowJones.Ajax.NewsStandTicker
{
    [DataContract(Name = "newsTickerDataResult", Namespace = "")]
    public class NewsTickerDataResult
    {
        [DataMember(Name = "hitCount")]
        [JsonProperty("hitCount")]
        public WholeNumber HitCount { get; set; }

        private NewsTickerDataResultSet _resultSet;

        [DataMember(Name = "resultSet")]
        [JsonProperty("resultSet")]
        public NewsTickerDataResultSet ResultSet
        {
            get
            {
                return _resultSet ?? (_resultSet = new NewsTickerDataResultSet());
            }
            set { _resultSet = value; }
        }
    }
}
