using Newtonsoft.Json;
using DowJones.Utilities.Formatters;
using System.Runtime.Serialization;

namespace DowJones.Tools.Ajax.NewsStandTicker
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
