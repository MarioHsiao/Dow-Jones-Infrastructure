using System.Collections.Generic;
using Factiva.Gateway.Messages.MarketData.V1_0;
using Factiva.Gateway.Messages.Symbology.Company.V1_0;

namespace DowJones.Managers.Sparkline
{
    public class SparklineServiceRequest
    {
        public SparklineServiceRequest()
        {
            DataPoints = 5;
        }

        public IEnumerable<string> CompanyCodes { get; set; }

        public uint DataPoints { get; set; }
    }

    public class SparklineServiceResponse
    {
        public IEnumerable<SparklineDataSet> SparklineData;
    }

    public class SparklineDataSet
    {
        public HistoricalDataResult HistoricalDataResult { get; set; }

        public Company Company { get; set; }
    }
}