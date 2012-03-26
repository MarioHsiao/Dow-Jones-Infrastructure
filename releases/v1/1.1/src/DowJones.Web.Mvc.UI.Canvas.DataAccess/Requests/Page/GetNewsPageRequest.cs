// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetNewsPageRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests
{
    public class NewsPageRequest : IPageRequest, ICacheState
    {
        #region IModuleRequest Members
        
        /// <summary>
        /// Gets or sets the page ID.
        /// </summary>
        /// <value>The page ID.</value>
        public string PageId { get; set; }
        
        public bool IsValid()
        {
            return PageId.IsNotEmpty();
        }
        #endregion

        public CacheState CacheState { get; set; }
    }
}
