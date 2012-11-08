using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Trending.Packages;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Trending.Results
{
    [DataContract(Name = "trendingNewsPageModuleServicePartResult", Namespace = "")]
    public class TrendingNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage>
        where TPackage : AbstractTrendingPackage
    {
    }
}