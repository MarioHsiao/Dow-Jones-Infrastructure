using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Web.Mvc.UI.Components.DiscoveryGraph;

namespace DowJones.Web.Mvc.UI.Components.DiscoveryFilters
{
    public class DiscoveryFiltersModel : ViewComponentModel
    {
        /// <summary>
        /// Client Data
        /// </summary>
        [ClientData]
        public Entities Data { get; set; }
    }
}
