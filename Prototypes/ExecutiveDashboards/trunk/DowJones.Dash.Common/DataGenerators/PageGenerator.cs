using System.Collections.Generic;
using System.Linq;
using DowJones.Pages;
using DowJones.Pages.Modules;
using DowJones.Pages.Modules.Templates;
using DowJones.Web.Mvc.UI.Canvas;
using Factiva.Gateway.Messages.DJComm.BriefingBook.V1_0;
using log4net;
using Module = DowJones.Pages.Modules.Module;
using ZoneLayout = DowJones.Pages.Layout.ZoneLayout;

namespace DowJones.Dash.DataGenerators
{
    public class PageGenerator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (PageGenerator));

        private readonly IScriptModuleTemplateManager _templateManager;
	    private readonly IPageTemplateManager _pageTemplateManager;


	    public PageGenerator(IScriptModuleTemplateManager templateManager, IPageTemplateManager pageTemplateManager)
        {
	        _templateManager = templateManager;
	        _pageTemplateManager = pageTemplateManager;
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
				)
				.Union(_pageTemplateManager.GetDefaultModules())
				.ToArray();


            var zones = GetZones(3, modules).ToArray();

            var page = new Page
                {
                    OwnerUserId = userId,
                    Layout = new ZoneLayout(zones),
                    ModuleCollection = new List<Module>(modules),
                };

            return page;
        }

        private static IEnumerable<ZoneLayout.Zone> GetZones(int zoneCount, IEnumerable<Module> modules)
        {
            var moduleArray = modules.ToArray();

            // generate three zones
            var zone1 = new ZoneLayout.Zone(new[] {moduleArray[0].Id, moduleArray[1].Id, moduleArray[2].Id});
            yield return zone1;

            var zone2 = new ZoneLayout.Zone(new[] { moduleArray[3].Id, moduleArray[4].Id});
            yield return zone2;

            var zone3 = new ZoneLayout.Zone(new[] { moduleArray[5].Id, moduleArray[6].Id, moduleArray[7].Id });
            yield return zone3;
            
            /*
            for (var zoneIndex = 1; zoneIndex <= zoneCount; zoneIndex++)
            {
                var zone = new ZoneLayout.Zone();

                var moduleIndex = 0;

                zone.AddRange(
                    from module in moduleArray
                    where ((moduleIndex += 1) % zoneIndex) == 0
                    select module.Id);

                yield return zone;
            }*/
        }
    }
}