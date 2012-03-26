// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseModuleRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Core
{
    [DataContract(Name = "baseModuleRequest", Namespace = "")]
    public class BaseModuleRequest : IModuleRequest
    {
        /// <summary>
        /// Gets or sets the module ID.
        /// </summary>
        /// <value>The module ID.</value>.
        [DataMember(Name = "moduleId")]
        public string ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the page ID.
        /// </summary>
        /// <value>The page ID.</value>
        [DataMember(Name = "pageId")]
        public string PageId { get; set; }

        /// <summary>
        /// Gets or sets the cache key.
        /// </summary>
        /// <value>
        /// The cache key.
        /// </value>
        [DataMember(Name = "cacheKey")]
        public string CacheKey { get; set; }
        
        /// <summary>
        /// The is valid.
        /// </summary>
        /// <returns>
        /// The is valid.
        /// </returns>
        public virtual bool IsValid()
        {
            return ModuleId.IsNotEmpty() && PageId.IsNotEmpty();
        }
    }
}
