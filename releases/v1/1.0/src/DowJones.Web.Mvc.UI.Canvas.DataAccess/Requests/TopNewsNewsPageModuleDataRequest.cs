// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TopNewsNewsPageModuleDataRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class TopNewsNewsPageModuleDataRequest : AbstractModuleGetRequest
    {
        private List<TopNewsModulePart> parts;

        public TopNewsNewsPageModuleDataRequest()
        {
            MaxResultsToReturn = 5;
            FirstResultToReturn = 0;

            Parts = new List<TopNewsModulePart>
                       {
                           TopNewsModulePart.EditorsChoice,
                           TopNewsModulePart.OpinionAndAnalysis,
                           TopNewsModulePart.VideoAndAudio,
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

        /// <summary>
        /// Gets or sets the entity code. This is basically the region code that the user is trying to filter on.
        /// </summary>
        /// <value>The entity/Factiva code.</value>
        /// <remarks></remarks>
        public List<TopNewsModulePart> Parts
        {
            get { return parts ?? (parts = new List<TopNewsModulePart>()); }
            set { parts = value; }
        }
        
        #region IModuleRequest Members
        
        public override bool IsValid()
        {
            return PageId.IsNotEmpty() && ModuleId.IsNotEmpty() && parts.Count > 0 && FirstResultToReturn >= 0 && MaxResultsToReturn >= 0;
        }

        #endregion
    }
}
