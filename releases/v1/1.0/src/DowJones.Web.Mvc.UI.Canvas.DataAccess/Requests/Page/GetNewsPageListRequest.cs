// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetNewsPageListRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page
{
    public class NewsPagesListRequest : IPageRequest, ICacheState
    {
        #region IPagesListRequest Members

        /// <summary>
        /// Gets or sets the start index of the headlines. Note: that this is "0" based.
        /// </summary>
        /// <value>
        /// The start index of the headlines.
        /// </value>
        public string PageId { get; set; }

        public bool IsValid()
        {
            return true;
        }
        
        #endregion

        public CacheState CacheState { get; set; }
    }
}
