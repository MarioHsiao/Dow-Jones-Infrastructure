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
                new { Title = "Browser Stats",      Description = "Percentage of users and page load times by browser", Component = "BrowserStats" },
                new { Title = "Concurrent Visits",  Description = "Number of users currently visiting the site",        Component = "ConcurrentVisits" },
                new { Title = "Page Load",          Description = "Historical average page load times",                 Component = "PageTimings" },
                new { Title = "Platform Stats",     Description = "Desktop vs. Mobile user load",                       Component = "PlatformStats" },
                new { Title = "Top Pages",          Description = "Top most visited pages on the site",                 Component = "TopPages" },
                new { Title = "Top Referrers",      Description = "External domains referring the most visitors to the site", Component = "TopReferrer" },
                new { Title = "Engagements",        Description = "How users are interacting with the site",            Component = "UsersStats" },
				new { Title = "Page Load By State", Description = "Average page load times, broken down by state",      Component = "StatsMap" },
            };

            return dashboardComponents.Select(component => 
                new ScriptModuleTemplate {
                    Title = component.Title,
                    Description = component.Description,
                    Script = string.Format(@"DJ.add('{0}', {{ container: this.container }})", component.Component),
                    ExternalIncludes = new string[] {},
                }
            );
        }
    }
}