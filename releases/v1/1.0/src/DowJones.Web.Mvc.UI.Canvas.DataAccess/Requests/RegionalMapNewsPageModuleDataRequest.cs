// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegionalMapNewsPageModuleDataRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using DowJones.Utilities.Managers.Search;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests
{
    public class RegionalMapNewsPageModuleDataRequest : AbstractModuleGetRequest
    {
        public RegionalMapNewsPageModuleDataRequest()
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

        public override bool IsValid()
        {
            return PageId.IsNotEmpty() && ModuleId.IsNotEmpty() && (TimeFrame == TimeFrame.LastWeek || TimeFrame == TimeFrame.LastMonth);
        }

        #endregion
    }
}
