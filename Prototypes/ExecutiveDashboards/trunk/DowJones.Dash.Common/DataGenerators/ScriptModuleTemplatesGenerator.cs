using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DowJones.Pages.Modules.Templates;

namespace DowJones.Dash.DataGenerators
{
    public class ScriptModuleTemplatesGenerator
    {
        public string StylesheetPath
        {
            get { return _stylesheetPath ?? ConfigurationManager.AppSettings["ScriptModuleTemplateStylesheetPath"]; }
            set { _stylesheetPath = value; }
        }
        private string _stylesheetPath;

        public IEnumerable<ScriptModuleTemplate> GenerateScripModuleTemplates()
        {
            var dashboardComponents = new[] {
                new { Title = "Browser Share", Component = "BrowserShare" },
                new { Title = "Browser Stats", Component = "BrowserStats" },
                new { Title = "Concurrent Visits", Component = "ConcurrentVisits" },
                new { Title = "Page Timings", Component = "PageTimings" },
                new { Title = "Platform Stats", Component = "PlatformStats" },
                new { Title = "Top Pages", Component = "TopPages" },
                new { Title = "Top Referrers", Component = "TopReferrer" },
                new { Title = "Users Stats", Component = "UsersStats" },
				new { Title = "Page Load Details By City", Component = "StatsMap" },
            };

            return dashboardComponents.Select(component => 
                new ScriptModuleTemplate {
                    Title = component.Title,
                    Script = string.Format(@"DJ.add('{0}', {{ container: this.container }})", component.Component),
                    ExternalIncludes = new string[] {},
                }
            );
        }
    }
}