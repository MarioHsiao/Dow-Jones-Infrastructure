using System.Collections.Generic;
using DowJones.Web.Mvc.UI;

namespace DowJones.Dash.Components.Models.ThemeManager
{
    public class ThemeManagerModel : ViewComponentModel
    {
        [ClientProperty("colors")]
        public List<string> Colors { get; set; }

        [ClientProperty("debug")]
        public bool Debug { get; set; }
    }
}