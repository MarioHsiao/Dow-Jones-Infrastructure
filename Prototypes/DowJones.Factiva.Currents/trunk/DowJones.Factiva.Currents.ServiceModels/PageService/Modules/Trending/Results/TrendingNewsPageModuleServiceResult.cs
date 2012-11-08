using System;
using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Trending.Packages;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Trending.Results
{
    /// <summary>
    /// The trending news page module service result.
    /// </summary>
    [DataContract(Name = "trendingNewsPageModuleServiceResult", Namespace = "")]
    public class TrendingNewsPageModuleServiceResult :
        AbstractModuleServiceResult<TrendingNewsPageServicePartResult<AbstractTrendingPackage>, AbstractTrendingPackage, TrendingNewsPageModule>
        
    {}


    internal class Change : IComparable<Change>
    {
        public string Code;
        public string Descriptor;
        private int previous;
        private int current;

        private double? rankScore;
        private decimal? changePercentage;

        public decimal ChangePercentage
        {
            get
            {
                if (!changePercentage.HasValue)
                {
                    changePercentage = previous == 0 ?
                                           decimal.MaxValue :
                                           decimal.Round((Convert.ToDecimal(current) - Convert.ToDecimal(previous)) / Convert.ToDecimal(previous) * 100, 2);
                }

                return changePercentage.Value;
            }
        }

        public double RankScore
        {
            get
            {
                if (!rankScore.HasValue)
                {
                    var log = Math.Log(current + 3);
                    rankScore = (current - previous) / (log * log);
                }

                return rankScore.Value;
            }
        }

        public int Previous
        {
            get { return previous; }
            set
            {
                rankScore = null;
                changePercentage = null;
                previous = value;
            }
        }

        public int Current
        {
            get { return current; }
            set
            {
                rankScore = null;
                changePercentage = null;
                current = value;
            }
        }

        public int CompareTo(Change other)
        {
            return other == null ? 1 : RankScore.CompareTo(other.RankScore);
        }
    }
}
