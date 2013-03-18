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
        /// Specifies the max length for the chart item label. The rest of the string will be truncated.
        /// </summary>
        [ClientProperty]
        public int TruncationLength { get; set; }

        /// <summary>
        /// Client Data
        /// </summary>
        [ClientData]
        public Entities Data { get; set; }
    }
}
