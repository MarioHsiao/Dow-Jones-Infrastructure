// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsPageHeadlineModuleDataRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests
{
    public class NewsPageHeadlineModuleDataRequest : AbstractModuleGetRequest
    {
        public NewsPageHeadlineModuleDataRequest()
        {
            MaxPartsToReturn = 3;
            MaxResultsToReturn = 5;
            FirstPartToReturn = 0;
            FirstResultToReturn = 0;
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
        /// Gets or sets the size of the result.
        /// </summary>
        /// <value>The size of the result.</value>
        public int MaxPartsToReturn { get; set; }

        /// <summary>
        /// Gets or sets the index of the result page.
        /// </summary>
        /// <value>The index of the result page.</value>
        public int FirstPartToReturn { get; set; }

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

        #region IModuleRequest Members
        
        public override bool IsValid()
        {
            return PageId.IsNotEmpty() && ModuleId.IsNotEmpty() && FirstResultToReturn >= 0 && MaxResultsToReturn >= 0 && MaxResultsToReturn <= 100 && MaxPartsToReturn > 0 && FirstPartToReturn >= 0;
        }
        
        #endregion
    }
}