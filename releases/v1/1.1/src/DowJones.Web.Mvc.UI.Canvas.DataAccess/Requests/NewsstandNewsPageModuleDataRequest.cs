// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsstandNewsPageModuleDataRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests
{
    public enum NewsstandPart
    {
        /// <summary>
        /// </summary>
        Headlines,

        /// <summary>
        /// </summary>
        Counts,

        /// <summary>
        /// </summary>
        DiscoveredEntities,
    }

    /// <summary>
    /// </summary>
    /// <remarks></remarks>
    public class NewsstandNewsPageModuleDataRequest : AbstractModuleGetRequest
    {
        private List<NewsstandPart> parts;

        public NewsstandNewsPageModuleDataRequest()
        {
            MaxResultsToReturn = 5;
            FirstResultToReturn = 0;
            Parts = new List<NewsstandPart>
                        {
                            NewsstandPart.Counts,
                            NewsstandPart.DiscoveredEntities,
                            NewsstandPart.Headlines
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

        public List<NewsstandPart> Parts
        {
            get { return parts ?? (parts = new List<NewsstandPart>()); }
            set { parts = value; }
        }

        /// <summary>
        /// Gets or sets the first result to return.
        /// </summary>
        /// <value>
        /// The first result to return.
        /// </value>
        public int FirstResultToReturn { get; set; }

        /// <summary>
        /// Gets or sets the number of results to return.
        /// </summary>
        /// <value>The number of results to return.</value>
        /// <remarks></remarks>
        public int MaxResultsToReturn { get; set; }

        #region IModuleRequest Members
        
        public override bool IsValid()
        {
            return PageId.IsNotEmpty() && ModuleId.IsNotEmpty() && parts.Count > 0;
        } 
        
        #endregion
    }
}
