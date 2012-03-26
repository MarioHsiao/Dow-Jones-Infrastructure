// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrendingItemSearchContext.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using DowJones.Utilities.Managers.Search;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext
{
    public class TrendingItemSearchContext : AbstractSearchContext<TrendingItemSearchContext>
    {
        [JsonProperty(PropertyName = SearchContextConstants.TimeFrame)]
        public TimeFrame TimeFrame;

        [JsonProperty(PropertyName = SearchContextConstants.EntityType)]
        public EntityType EntityType;

        [JsonProperty(PropertyName = SearchContextConstants.TrendType)]
        public TrendType TrendType;

        [JsonProperty(PropertyName = SearchContextConstants.Code)]
        public string Code;
    }
}
