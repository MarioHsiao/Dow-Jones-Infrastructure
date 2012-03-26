// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPortalHeadlines.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Session;
using DowJones.Tools.Ajax.PortalHeadlineList;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPortalHeadlines
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        PortalHeadlineListDataResult Result { get; set; }
    }

    public interface IViewAllSearchContextRef
    {
        string ViewAllSearchContextRef { get; set; }
    }
}