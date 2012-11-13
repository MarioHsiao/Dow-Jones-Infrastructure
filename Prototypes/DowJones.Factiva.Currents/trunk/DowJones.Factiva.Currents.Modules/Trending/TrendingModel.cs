using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Trending.Results;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Canvas;
using DowJones.Web.Mvc.UI.Canvas.Editors;
using DowJones.Web.Mvc.UI.Canvas.Models;
using System.Linq;
using System.Collections.Generic;
using DowJones.Models.Common;
using DowJones.Factiva.Currents.Components.Trending;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Trending.Packages;

namespace DowJones.Factiva.Currents.Models
{
	public class TrendingModel : Module
	{
        //public IEnumerable<IList<NewsEntityNewsVolumeVariation>> NewsEntity;

        public IEnumerable<TrendingComponentModel> trendingTopEntitiesPackageModel;
        
	}

    /// <summary>
    /// The TopNewsModule Mapper class
    /// </summary>
    public class TrendingModuleMapper : TypeMapper<TrendingNewsPageModuleServiceResult, TrendingModel>
    {
        #region Public Methods

        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// A IModule object.   
        /// </returns>
        public override TrendingModel Map(TrendingNewsPageModuleServiceResult trendingSource)
        {
            var x = trendingSource.PartResults.Last().Package.TopNewsVolumeEntities;
            return new TrendingModel()
            {
                trendingTopEntitiesPackageModel = trendingSource.PartResults.Where(c => c.PackageType == "TrendingTopEntitiesPackage").Select(p => new TrendingComponentModel(p.Package.TopNewsVolumeEntities))
            };
        }

        #endregion
    }
}