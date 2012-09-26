using System.Collections.Generic;
using System.Linq;
using DowJones.Pages;
using DowJones.Pages.Layout;
using DowJones.Pages.Modules;
using DowJones.Pages.Modules.Templates;
using log4net;

namespace DowJones.Dash.DataGenerators
{
    public class PageGenerator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (PageGenerator));

        private readonly IScriptModuleTemplateManager _templateManager;

        public PageGenerator(IScriptModuleTemplateManager templateManager)
        {
            _templateManager = templateManager;
        }

        public Page GeneratePage(string userId)
        {
            Log.DebugFormat("Generating new page for {0}", userId);

            var templates = _templateManager.GetTemplates();

            var moduleId = 0;
            var modules =
                (
                    from template in templates
                    select new ScriptModule
                        {
                            Id = moduleId += 1,
                            TemplateId = template.Id,
                            Title = template.Title,
                        }
                ).ToArray();


            var zones = GetZones(3, modules).ToArray();

            var page = new Page
                {
                    OwnerUserId = userId,
                    Layout = new ZoneLayout(zones),
                    ModuleCollection = new List<Module>(modules),
                };

            return page;
        }

        private IEnumerable<ZoneLayout.Zone> GetZones(int zoneCount, IEnumerable<ScriptModule> modules)
        {
            var moduleArray = modules.ToArray();

            for (int zoneIndex = 1; zoneIndex <= zoneCount; zoneIndex++)
            {
                var zone = new ZoneLayout.Zone();

                int moduleIndex = 0;

                zone.AddRange(
                    from module in moduleArray
                    where ((moduleIndex += 1) % zoneIndex) == 0
                    select module.Id);

                yield return zone;
            }
        }
    }
}