using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.Infrastructure;
using DowJones.Mapping.Types;
using DowJones.Pages.Modules;
using DowJones.Web.Mvc.UI.Canvas;

namespace DowJones.DegreasedDashboards.Website.Controllers
{
    public class PagesController : DowJones.Web.Mvc.UI.Canvas.Controllers.DashboardControllerBase
    {
        private readonly IModuleTemplateManager _moduleTemplateManager;
        private readonly IDictionary<string, Type> _moduleEditorTypes;

        public PagesController(IAssemblyRegistry assemblyRegistry, IModuleTemplateManager moduleTemplateManager)
        {
            _moduleTemplateManager = moduleTemplateManager;
            _moduleEditorTypes =
                assemblyRegistry.GetConcreteTypesDerivingFrom(typeof(IAbstractCanvasModuleEditor))
                    .ToGenericBaseTypeMappings()
                    .Select(x => x.GenericType)
                    .Select(x => new KeyValuePair<string,Type>(x.Name.Replace("Editor", string.Empty), x))
                    .ToDictionary(x => x.Key, y => y.Value);
        }

        public ActionResult Index()
        {
            return RedirectToAction("Page", new {id = "1234"});
        }

        public ActionResult Editors(string id)
        {
            Type editorType;
            if (!_moduleEditorTypes.TryGetValue(id, out editorType))
                return HttpNotFound();

            var model = Activator.CreateInstance(editorType);
            return ViewComponent(model);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var moduleTemplates = _moduleTemplateManager.GetTemplates();
            ViewBag.ModuleTemplates = moduleTemplates;
        }
    }

    public class ModuleType
    {
        public string DisplayName { get; set; }
        public string EditorClass { get; set; }
    }
}
