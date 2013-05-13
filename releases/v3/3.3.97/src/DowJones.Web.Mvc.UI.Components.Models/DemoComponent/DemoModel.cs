using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Web.Mvc.UI.Components.DemoComponent
{
    public class DemoModel
    {
        public string Id { get; set; }

        [ClientData("name")]
        public string Name { get; set; }

        [ClientData("time")]
        public TimeSpan Time { get; set; }

        [ClientProperty("show")]
        public bool Show { get; set; }
    }
}
