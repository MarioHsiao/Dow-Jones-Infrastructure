// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrendingNewsPageModuleDataRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Utilities.Managers.Search;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests
{
    public enum EntityType
    {
        /// <summary>
        /// Companies
        /// </summary>
        [NavigationControlId("co")]
        Companies = 0,
        
        /// <summary>
        /// People
        /// </summary>
        [NavigationControlId("pe")]
        People = 1,
        
        /// <summary>
        /// 
        /// </summary>
        [NavigationControlId("ns")]
        Subjects = 2,
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class NavigationControlIdAttribute : Attribute
    {
        public NavigationControlIdAttribute(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }

    public class TrendingNewsPageModuleRequest : AbstractModuleGetRequest
    {
        public TrendingNewsPageModuleRequest()
        {
            TimeFrame = TimeFrame.LastWeek;
            Parts = new List<TrendType>
                        {
                            TrendType.TopEntities,
                            TrendType.TrendingDown,
                            TrendType.TrendingUp,
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
        /// Gets or sets the time frame.
        /// </summary>
        /// <value>The time frame is 1 week, 1 month, 3 months etc.,</value>
        /// <remarks></remarks>
        public TimeFrame TimeFrame { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        /// <remarks></remarks>
        public EntityType EntityType { get; set; }
        
        public List<TrendType> Parts { get; set; }

        #region IModuleRequest Members

        /// <summary>
        /// Determines whether this instance is valid.
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
