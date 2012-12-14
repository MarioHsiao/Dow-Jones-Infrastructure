using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Trending.Packages;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Trending.Results;
using DowJones.Formatters;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI;
using System.Linq;
using System.Collections.Generic;
using DowJones.Models.Common;
using System.Web.Mvc;

namespace DowJones.Factiva.Currents.Models
{
    public class CurrentTrendingModel : CompositeComponentModel
	{

        public IEnumerable<TrendingEntities> TrendingTopEntities;

        public IEnumerable<TrendingEntities> TrendingUp;

        public IEnumerable<TrendingEntities> TrendingDown;

	    public CurrentTrendingModel()
	    {
		    TrendingTopEntities = TrendingUp = TrendingDown = Enumerable.Empty<TrendingEntities>();
	    }
        
	}

    public class TrendingEntities 
    {
	    public string Descriptor { get; set; }
        public string SearchContextRef { get; set; }
		public WholeNumber CurrentTimeFrameNewsVolume { get; set; }

		public Percent PercentVolumeChange { get; set; }

		public WholeNumber PreviousTimeFrameNewsVolume { get; set; }

		public bool NewEntrant { get; set; }

	    public string GetTrendingUrl(NewsEntity entity, UrlHelper urlHelper)
        {
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
		/// A CurrentTrendingModel object.   
        /// </returns>
        public override CurrentTrendingModel Map(TrendingNewsPageModuleServiceResult trendingSource)
        {
			var currentTrendingModel = new CurrentTrendingModel();
	        var topEntities = trendingSource
		        .PartResults
		        .FirstOrDefault(c => c.Package is TrendingTopEntitiesPackage);

			if (topEntities != null)
				currentTrendingModel.TrendingTopEntities = topEntities.Package
					.TopNewsVolumeEntities
					.Select(p => new TrendingEntities
						{
							Descriptor = p.Descriptor,
							SearchContextRef = p.SearchContextRef,
							CurrentTimeFrameNewsVolume = p.CurrentTimeFrameNewsVolume,
							
						});

	        var trendingUp = trendingSource
		        .PartResults
		        .FirstOrDefault(c => c.Package is TrendingUpPackage);

			if (trendingUp != null)
				currentTrendingModel.TrendingUp = trendingUp.Package
					.TrendingEntities
					.Select(p => new TrendingEntities
					{
						Descriptor = p.Descriptor,
						SearchContextRef = p.SearchContextRef,
						CurrentTimeFrameNewsVolume = p.CurrentTimeFrameNewsVolume,
						PercentVolumeChange = p.PercentVolumeChange,
						PreviousTimeFrameNewsVolume = p.PreviousTimeFrameNewsVolume,
						NewEntrant = p.NewEntrant,
					});


			var trendingDown = trendingSource
			   .PartResults
			   .FirstOrDefault(c => c.Package is TrendingDownPackage);

			if (trendingDown != null)
				currentTrendingModel.TrendingDown = trendingDown.Package
					.TrendingEntities
					.Select(p => new TrendingEntities
					{
						Descriptor = p.Descriptor,
						SearchContextRef = p.SearchContextRef,
						CurrentTimeFrameNewsVolume = p.CurrentTimeFrameNewsVolume,
						PercentVolumeChange = p.PercentVolumeChange,
						PreviousTimeFrameNewsVolume = p.PreviousTimeFrameNewsVolume,
						NewEntrant = p.NewEntrant,
					});


	        return currentTrendingModel;
        }

        #endregion
    }
}