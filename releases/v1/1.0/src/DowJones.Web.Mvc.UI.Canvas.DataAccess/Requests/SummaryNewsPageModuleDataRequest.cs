// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SummaryNewsPageModuleDataRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests
{
    public enum SummaryParts
    {
        /// <summary>
        /// Chart part.
        /// </summary>
        Chart,

        /// <summary>
        /// Recent Articles part.
        /// </summary>
        RecentArticles,

        /// <summary>
        /// Recent Articles part.
        /// </summary>
        RecentVideos,

        /// <summary>
        /// SnapShot part.
        /// </summary>
        RegionalMap,

        /// <summary>
        /// Trending part.
        /// </summary>
        Trending,
    }

    public class SummaryNewsPageModuleDataRequest : AbstractModuleGetRequest
    {
        private List<SummaryParts> parts = new List<SummaryParts>();

        public SummaryNewsPageModuleDataRequest()
        {
            FirstResultToReturn = 0;
            MaxResultsToReturn = 5;
            MaxEntitiesToReturn = 10;
            TruncationType = AbstractServiceResult.DefaultTruncationType;
            Parts = new List<SummaryParts>
                        {
                            //SummaryParts.Chart,
                            SummaryParts.RecentArticles,
                            SummaryParts.RecentVideos,
                            SummaryParts.RegionalMap,
                            SummaryParts.Trending
                        };
        }

        /// <summary>
        /// Gets or sets the type of the truncation.
        /// </summary>
        /// <value>
        /// The type of the truncation.
        /// </value>
        public TruncationType TruncationType { get; set; }
       
        /// <summary>
        /// Gets or sets the number of headlines.
        /// </summary>
        /// <value>
        /// The number of headlines.
        /// </value>
        public int MaxResultsToReturn { get; set; }

        /// <summary>
        /// Gets or sets the start index of the headlines. Note: that this is "0" based.
        /// </summary>
        /// <value>
        /// The start index of the headlines.
        /// </value>
        public int FirstResultToReturn { get; set; }

        /// <summary>
        /// Gets or sets the max entities to return.
        /// </summary>
        /// <value>
        /// The max entities to return.
        /// </value>
        public int MaxEntitiesToReturn { get; set; }

        public List<SummaryParts> Parts
        {
            get { return parts ?? (parts = new List<SummaryParts>()); }
            set { parts = value; }
        }

        public override bool IsValid()
        {
            return PageId.IsNotEmpty() && ModuleId.IsNotEmpty() && parts.Count > 0 && FirstResultToReturn >= 0 && MaxResultsToReturn >= 0 && MaxEntitiesToReturn > 0;
        }
    }
}
