using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Models.Common;

namespace DowJones.Web.Mvc.UI.Components.DiscoveryFilters
{
    public class DiscoveryFiltersModel : ViewComponentModel
    {
        /// <summary>
        /// Specifies whether the hit count should be rounded
        /// </summary>
        [ClientProperty]
        public bool RoundHitCount { get; set; }

        /// <summary>
        /// Client Data
        /// </summary>
        [ClientData]
        public Entities Data { get; set; }
    }
}
