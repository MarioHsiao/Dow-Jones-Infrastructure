using System.Collections.Generic;
using System.Linq;
using DowJones.Pages.Modules.Templates;

namespace DowJones.Dash.DataGenerators
{
    public class ScriptModuleTemplatesGenerator
    {
        public IEnumerable<ScriptModuleTemplate> GenerateScripModuleTemplates(string stylesheetBasePath)
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
            };

            return dashboardComponents.Select(component => 
                new ScriptModuleTemplate {
                    Title = component.Title,
                    Script = string.Format(@"DJ.add('{0}', {{ container: this.container }})", component.Component),
                    ExternalIncludes = new[] { string.Format("{0}/{1}.css", stylesheetBasePath, component.Component) }
                }
            );
        }
    }
}