using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.Infrastructure;
using DowJones.Mapping.Types;
using DowJones.Web.Mvc.UI.Canvas;

namespace DowJones.DegreasedDashboards.Website.Controllers
{
    public class PagesController : DowJones.Web.Mvc.UI.Canvas.Controllers.PageControllerBase
    {
        private readonly IDictionary<string, Type> ModuleEditorTypes;

        public PagesController(IAssemblyRegistry assemblyRegistry)
        {
            ModuleEditorTypes =
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
            if (!ModuleEditorTypes.TryGetValue(id, out editorType))
                return HttpNotFound();

            var model = Activator.CreateInstance(editorType);
            return ViewComponent(model);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var moduleTypes = new[] {
                    new ModuleType{DisplayName = "HTML Module", EditorClass = "HtmlModule"},
                    new ModuleType{DisplayName = "Embedded Module", EditorClass = "EmbeddedContent"},
                };
            
            ViewBag.ModuleTypes = moduleTypes;
        }
    }

    public class ModuleType
    {
        public string DisplayName { get; set; }
        public string EditorClass { get; set; }
    }
}
