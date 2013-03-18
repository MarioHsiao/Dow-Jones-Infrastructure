using System;
using System.Collections.Generic;
using DowJones.Models.Common;

namespace DowJones.Web.Mvc.UI.Components.TrendingNews
{
    public class TrendingNewsModel :ViewComponentModel 
    {
        //The type of the package. -- TrendingUpPackage, TrendingDownPackage, TrendingTopEntitiesPackage
        [ClientProperty] 
        public string packageType { get; set; }

        //Specific type of entity to return trending on - People, Companies, Subjects.
        [ClientProperty]
        public string entityType { get; set; }

        // Trending entities depending on the type.
        [ClientData]
        public IList<NewsEntityNewsVolumeVariation> trendingEntities { get; set; } 
    }
}
