using System.Collections.Generic;
using DowJones.Pages.Modules.Templates;

namespace DowJones.DegreasedDashboards.Website.Models
{
    public class ModuleGalleryModel
    {
        public string PageId { get; set; }

        public IEnumerable<ScriptModuleTemplate> Templates { get; set; }
    }
}