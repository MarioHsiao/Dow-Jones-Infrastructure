using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Trending.Results;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI;
using System.Linq;
using System.Collections.Generic;
using DowJones.Models.Common;
using System.Web.Mvc;
using DowJones.Formatters;

namespace DowJones.Factiva.Currents.Models
{
    public class CurrentTrendingModel : CompositeComponentModel
	{

        public IEnumerable<TrendingEntities> TrendingTopEntitiesPackageModel;

        public IEnumerable<TrendingEntities> TrendingUpModel;

        public IEnumerable<TrendingEntities> TrendingDownModel;
        
	}

    public class TrendingEntities 
    {
        public string Code { get; set; }
        public WholeNumber CurrentTimeFrameNewsVolume { get; set; }
        public WholeNumber CurrentTimeFrameRoundedNewsVolume { get; set; }
        public string Descriptor { get; set; }
        public string SearchContextRef { get; set; }
        public string Type { get; set; }
        public string TypeDescriptor { get; set; }

        public IList<NewsEntity> TrendingTopEntitiesPackageModel;

        public IList<NewsEntityNewsVolumeVariation> TrendingUpDownPackageModel;


        public bool HasTrendingTopEntitiesData
        {
            get
            {
                return (TrendingTopEntitiesPackageModel != null && TrendingTopEntitiesPackageModel.Any());
            }
        }

        public bool HasTrendingUpDownData
        {
            get
            {
                return (TrendingUpDownPackageModel != null && TrendingUpDownPackageModel.Any());
            }
        }

        public TrendingEntities()
        {
        }

        public string SelectedGuid { get; set; }
        public bool ShowSource { get; set; }
        public bool ShowPublicationDateTime { get; set; }

        // view helpers
        public string GetSelectionStatus(NewsEntity newsEntity)
        {
            return SelectedGuid == newsEntity.Code ? "dj_entry_selected" : string.Empty;
        }

        public TrendingEntities(IList<NewsEntity> newsEntityList)
        {
            this.TrendingTopEntitiesPackageModel = newsEntityList;
        }

        public TrendingEntities(IList<NewsEntityNewsVolumeVariation> newsEntityNewsVolumeVariationList)
        {
            this.TrendingUpDownPackageModel = newsEntityNewsVolumeVariationList;
        }

        public string GetTrendingUrl(NewsEntity entity, UrlHelper urlHelper)
        {
            //if (!entity.Descriptor.Equals("external"))
            //    return urlHelper.Content("~/Trending/{0}?an={1}".Format(
            //        entity.Descriptor,
            //        entity.ToLowerInvariant()
            //        .Replace(' ', '-'), entity.Code));

            return string.Empty;
        }
    }

    /// <summary>
    /// The TopNewsModule Mapper class
    /// </summary>
    public class TrendingModuleMapper : TypeMapper<TrendingNewsPageModuleServiceResult, CurrentTrendingModel>
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
        public override CurrentTrendingModel Map(TrendingNewsPageModuleServiceResult trendingSource)
        {
            return new CurrentTrendingModel()
            {
                TrendingTopEntitiesPackageModel = trendingSource
                                                    .PartResults
                                                    .Where(c => c.PackageType == "TrendingTopEntitiesPackage")
                                                    .Select(p => new TrendingEntities(p.Package.TopNewsVolumeEntities)),

                TrendingUpModel = trendingSource
                                    .PartResults
                                    .Where(c => c.PackageType == "TrendingUpPackage")
                                    .Select(p => new TrendingEntities(p.Package.TrendingEntities)),

                TrendingDownModel = trendingSource
                                        .PartResults
                                        .Where(c => c.PackageType == "TrendingDownPackage")
                                        .Select(p => new TrendingEntities(p.Package.TrendingEntities))
            };
        }

        #endregion
    }
}