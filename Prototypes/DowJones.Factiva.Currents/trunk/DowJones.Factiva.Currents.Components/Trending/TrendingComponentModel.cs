using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Extensions;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using DowJones.Infrastructure;
using DowJones.Models.Common;

namespace DowJones.Factiva.Currents.Components.Trending
{
    public class TrendingComponentModel : DowJones.Web.Mvc.UI.ViewComponentModel
    {
        public string Code { get; set; }
        public string CurrentTimeFrameNewsVolume { get; set; }
        public string CurrentTimeFrameRoundedNewsVolume { get; set; }
        public string Descriptor { get; set; }
        public string SearchContextRef { get; set; }
        public string Type { get; set; }
        public string TypeDescriptor { get; set; }

        public IList<NewsEntity> trendingTopEntitiesPackageModel;

        public bool HasData
        {
            get { return trendingTopEntitiesPackageModel.Any(); }
        }

        public TrendingComponentModel()
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

        public bool ShouldShowSource(NewsEntity newsEntity)
        {
            return ShowSource
                    && !newsEntity.TypeDescriptor.IsEmpty(); //TODO write the correct logic
        }

        public bool ShouldShowPublicationDateTime(NewsEntity newsEntity)
        {
            return ShowPublicationDateTime
                    && !newsEntity.CurrentTimeFrameNewsVolume.RawText.ToString().IsEmpty();  //TODO write the correct logic
        }

        public TrendingComponentModel(IList<NewsEntity> newsEntityList)
        {
            this.trendingTopEntitiesPackageModel = newsEntityList;
        }

        public string GetTrendingUrl(NewsEntity entity, UrlHelper urlHelper)
        {
            if (!entity.Descriptor.Equals("external"))
                return urlHelper.Content("~/Trending/{0}?an={1}".FormatWith(
                    entity
                    .Descriptor
                    .ToLowerInvariant()
                    .Replace(' ', '-'), entity.Code));

            return string.Empty;
        }
    }
}