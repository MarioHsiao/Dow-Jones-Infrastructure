// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RadarNewsPageModuleDataRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Managers.Search;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests
{
    /// <summary>
    /// The radar news page module data request.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class RadarNewsPageModuleDataRequest : AbstractModuleGetRequest
    {
        public RadarNewsPageModuleDataRequest()
        {
            TimeFrame = TimeFrame.LastWeek;
        }

        /// <summary>
        /// Gets or sets the time frame.
        /// </summary>
        /// <value>The time frame is 1 week, 1 month, 3 months etc.,</value>
        /// <remarks></remarks>
        public TimeFrame TimeFrame { get; set; }

        #region IModuleRequest Members
        
        /// <summary>
        /// The is valid.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid()
        {
            return PageId.IsNotEmpty() && ModuleId.IsNotEmpty() && (TimeFrame == TimeFrame.LastWeek || TimeFrame == TimeFrame.LastMonth);
        }

        #endregion
    }
}
