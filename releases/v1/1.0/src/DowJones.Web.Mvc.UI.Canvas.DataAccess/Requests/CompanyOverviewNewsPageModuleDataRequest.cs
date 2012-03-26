// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyOverviewNewsPageModuleDataRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests
{
    public enum CompanyOverviewParts
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
        /// SnapShot part.
        /// </summary>
        SnapShot,

        /// <summary>
        /// Trending part.
        /// </summary>
        Trending,
    }

    public class CompanyOverviewNewsPageModuleDataRequest : AbstractModuleGetRequest
    {
        private List<CompanyOverviewParts> parts = new List<CompanyOverviewParts>();

        public CompanyOverviewNewsPageModuleDataRequest()
        {
            FirstResultToReturn = 0;
            MaxResultsToReturn = 5;
            Parts = new List<CompanyOverviewParts> 
                        {
                            CompanyOverviewParts.Chart, 
                            CompanyOverviewParts.RecentArticles, 
                            CompanyOverviewParts.SnapShot, 
                            CompanyOverviewParts.Trending 
                        };
            TruncationType = AbstractServiceResult.DefaultTruncationType;
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

        public List<CompanyOverviewParts> Parts
        {
            get { return parts ?? (parts = new List<CompanyOverviewParts>()); }
            set { parts = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use custom date range].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use custom date range]; otherwise, <c>false</c>.
        /// </value>
        public bool UseCustomDateRange { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date in format DDMMCCYY.
        /// </value>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date in format DDMMCCYY.
        /// </value>
        public string EndDate { get; set; }

        public override bool IsValid()
        {
            if (UseCustomDateRange)
            {
                if (StartDate.IsNullOrEmpty() || EndDate.IsNullOrEmpty())
                {
                    return false;
                }
            }

            return PageId.IsNotEmpty() && ModuleId.IsNotEmpty() && parts.Count > 0 && FirstResultToReturn >= 0 && MaxResultsToReturn >= 0;
        }
    }
}
