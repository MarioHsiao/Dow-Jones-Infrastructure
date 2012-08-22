using System.Linq;
using System.Web.Mvc;
using DowJones.DegreasedDashboards.Website.Models;
using DowJones.Pages;
using DowJones.Pages.Modules;
using DowJones.Pages.Modules.Templates;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.UI.Canvas.Modules.ScriptModule.Editor;

namespace DowJones.DegreasedDashboards.Website.Controllers
{
    public class ScriptModuleController : DowJones.Web.Mvc.ControllerBase
    {
        private readonly IPageRepository _pageRepository;
        private readonly IScriptModuleTemplateManager _templateManager;

        public ScriptModuleController(IPageRepository pageRepository, IScriptModuleTemplateManager templateManager)
        {
            _pageRepository = pageRepository;
            _templateManager = templateManager;
        }

        public ActionResult Index()
        {
            var templates = _templateManager.GetTemplates();
            var viewModels = templates.Select(x => new ScriptModuleTemplateViewModel(x));
            return View("List", viewModels);
        }

        [HttpGet]
        public ActionResult Create()
        {
            TempData["ReturnUrl"] = Request.UrlReferrer;
            return View("Edit", new EditScriptModuleTemplateRequest());
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var template = _templateManager.GetTemplate(id);

            if (template == null)
                return HttpNotFound("Module Template " + id + " not found");

            var viewModel = new EditScriptModuleTemplateRequest(template);
            return View("Edit", viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(string id, EditScriptModuleTemplateRequest request, string returnUrl)
        {
            var templateId = id;

            if(ModelState.IsValid)
            {
                ScriptModuleTemplate template;

                if (request.IsNew)
                    template = new ScriptModuleTemplate { Id = templateId };
                else
                    template = _templateManager.GetTemplate(id);

                if (template != null)
                {
                    template.Description = request.Description;
                    template.ExternalIncludes = request.ExternalIncludes;
                    template.HtmlLayout = request.Html;
                    template.Options = request.Options ?? Enumerable.Empty<ScriptModuleTemplateOption>();
                    template.Script = request.Script;
                    template.Title = request.Title;

                    if (request.IsNew)
                        templateId = _templateManager.CreateTemplate(template);
                    else
                        _templateManager.UpdateTemplate(template);

                    TempData["SuccessMessage"] = "Your template has been saved";
                    TempData["ModuleTemplateId"] = templateId;

                    if (!string.IsNullOrWhiteSpace(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index");
                }
            }

            return Edit(id);
        }

        public ActionResult Delete(string id)
        {
            _templateManager.DeleteTemplate(id);

            TempData["SuccessMessage"] = "Template deleted";

            return RedirectToAction("Index");
        }

        public ActionResult Editor(string id, string pageId)
        {
            ViewBag.PageId = pageId;

            var template = _templateManager.GetTemplate(id ?? "0");

            if (template == null)
                return HttpNotFound("Script Module Template " + id + " not found");

            var editor = new ScriptModuleEditor
                             {
                                 TemplateId = id,
                                 Options = template.Options,
                                 PageId = pageId,
                                 Title = template.Title,
                             };

            return ViewComponent(editor);
        }

        public ActionResult Gallery(string pageId)
        {
            var viewModel = new ModuleGalleryModel
                                {
                                    PageId = pageId,
                                    Templates = _templateManager.GetTemplates(),
                                };
            return PartialView("_ModuleGallery", viewModel);
        }


        // ##########################  Service Methods  ################################
        // These should/will be "REST" services, not controller actions


        [Route("modules/Script/1.0/data/json")]
        public ActionResult Save(string pageId, int? moduleId, [Bind(Prefix = "")]ScriptModule module)
        {
            if(moduleId == null)
            {
                moduleId = _pageRepository.AddModuleToPage(pageId, module);
            }
            else
            {
                var existing = _pageRepository.GetModule(pageId, moduleId.Value) as ScriptModule;
                UpdateModel(existing, "");
                _pageRepository.UpdateModule(existing);
            }

            return Json(new { moduleId });
        }

        [Route("modules/Script/1.0/data/json/script/{templateId}")]
        public ActionResult ScriptModuleScript(string templateId)
        {
            var template = _templateManager.GetTemplate(templateId);

            var scriptResult = new System.Text.StringBuilder();

            scriptResult.AppendFormat("console.log('Script Template {0}>> Executing with context ', this);{1}", templateId, template.Script);

            return Content(scriptResult.ToString(), "text/javascript");
        }

    }
}