using System.Runtime.Serialization;

namespace DowJones.Models.Charting.HistoricalNewsData
{
    [DataContract(Name = "historicalNewsDataResult", Namespace = "")]
    public class HistoricalNewsDataResult
    {
        public HistoricalNewsDataResult()
        {
            DataPoints = new NewsDataPointCollection();
        }
        [DataMember(Name = "dataPoints")]
        public NewsDataPointCollection DataPoints { get; set; }

        [DataMember(Name = "frequency")]
        public DataPointFrequency? Frequency { get; set; }
    }
}