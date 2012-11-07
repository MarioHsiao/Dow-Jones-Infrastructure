using System.Runtime.Serialization;
using DowJones.Factiva.Currents.Website.Models.PageService.Modules.Trending.Packages;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Trending.Results
{
    [DataContract(Name = "trendingNewsPageModuleServicePartResult", Namespace = "")]
    public class TrendingNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage>
        where TPackage : AbstractTrendingPackage
    {
    }
}