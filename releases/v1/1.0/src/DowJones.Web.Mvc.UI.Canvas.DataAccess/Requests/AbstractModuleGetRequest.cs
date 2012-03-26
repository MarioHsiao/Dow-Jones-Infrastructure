using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests
{
    public abstract class AbstractModuleGetRequest : IModuleGetRequest, ICacheState
    {
        #region Implementation of ICacheState

        public CacheState CacheState { get; set; }

        #endregion

        #region Implementation of IModuleBaseRequest

        /// <summary>
        /// Gets or sets the page ID.
        /// </summary>
        /// <value>The page ID.</value>
        public string PageId { get; set; }

        #endregion

        #region Implementation of IModuleRequest

        /// <summary>
        /// Gets or sets the module ID.
        /// </summary>
        /// <value>The module ID.</value>
        /// <remarks></remarks>
        public string ModuleId { get; set; }

        #endregion
        
        #region Implementation of IValidate

        public abstract bool IsValid();

        #endregion
    }
}
