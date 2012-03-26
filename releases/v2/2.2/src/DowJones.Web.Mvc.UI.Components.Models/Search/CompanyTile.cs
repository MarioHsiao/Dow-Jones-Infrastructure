using DowJones.Web.Mvc.UI.Components.Common;

namespace DowJones.Web.Mvc.UI.Components.Search
{
    public class CompanyTile
    {

        public bool IsPrivateCompany { get; set; }

        public string CompanyName { get; set; }

        public double? CurrentPrice { get; set; }

        public double? PriceDifference { get; set; }

        public double? PercentChange { get; set; }

        public PriceTrend PriceTrend
        {
            get
            {
                if (PercentChange > 0)
                    return PriceTrend.Positive;
                if (PercentChange < 0)
                    return PriceTrend.Negative;
                
                return PriceTrend.Neutral;
            }
        }

        // TODO:  Spark Lines model
        public dynamic Past5DaysPriceHistory { get; set; }

        public string Ticker { get; set; }
    }
}