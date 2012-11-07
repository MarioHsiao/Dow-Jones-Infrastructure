// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPortalHeadlines.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Ajax.PortalHeadlineList;

namespace DowJones.Factiva.Currents.Website.Models.PageService
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
}