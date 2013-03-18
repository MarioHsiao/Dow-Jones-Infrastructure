using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DowJones.Web.Mvc.UI.Components.Gauge;

namespace DowJones.Web.Showcase.Models
{
    public class GaugeViewModel
    {
        public GaugeModel Speedometer { get; set; }

        public GaugeModel Meter{ get; set; }
    }
}