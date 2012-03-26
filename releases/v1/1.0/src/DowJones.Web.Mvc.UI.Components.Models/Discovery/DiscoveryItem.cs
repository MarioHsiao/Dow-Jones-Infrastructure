using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Web.Mvc.UI.Components.Models.Discovery
{
    public class DiscoveryItem
    {
        public string Id { get; set; }
        public int Count { get; set; }
        public string Value { get; set; }
        internal double ShareCount { get; set; }
        public double ShareOfTop { get; set; }


        public DiscoveryItem()
        {
        }

    }
}
