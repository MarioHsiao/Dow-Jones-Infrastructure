using System.Linq;
using System.Threading;
using System.Web.Mvc;
using DowJones.DegreasedDashboards.Website.Models;
using DowJones.Pages;
using DowJones.Pages.Modules;
using DowJones.Pages.Modules.Templates;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.ActionResults;
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
            var templates = _templateManager.GetTemplates().OrderBy(x => x.Title).ToArray();
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
        public ActionResult Edit(string id, EditScriptModuleTemplateRequest template, string returnUrl = null)
        {
            string templateId;

            if (TryUpdateTemplate(id, template, out templateId))
            {
                TempData["SuccessMessage"] = "Your template has been saved";
                TempData["ModuleTemplateId"] = templateId;

                if (!string.IsNullOrWhiteSpace(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index");
            }

            return Edit(id);
        }

        private bool TryUpdateTemplate(string templateId, EditScriptModuleTemplateRequest request, out string newTemplateId)
        {
            newTemplateId = templateId;

            if (ModelState.IsValid)
            {
                ScriptModuleTemplate template;

                if (request.IsNew)
                    template = new ScriptModuleTemplate { Id = templateId };
                else
                    template = _templateManager.GetTemplate(templateId);

                if (template != null)
                {
                    request.Update(template);

                    if (request.IsNew)
                    {
                        newTemplateId = _templateManager.CreateTemplate(template);
                        // HACK: Wait for RavenDB to do whatever it needs to do...
                        Thread.Sleep(100);
                    }
                    else
                        _templateManager.UpdateTemplate(template);

                    return true;
                }
            }

            return false;
        }

        public ActionResult Export(string id)
        {
            var template = _templateManager.GetTemplate(id);

            var viewModel = new EditScriptModuleTemplateRequest(template);

            return new XmlResult(viewModel);
        }

        public ActionResult Import(string id, [XmlModelBinder]EditScriptModuleTemplateRequest template)
        {
            string templateId;

            if (TryUpdateTemplate(id, template, out templateId))
            {
                return Content(templateId);
            }

            return new HttpStatusCodeResult(500, "Could not import template");
        }

        public ActionResult Delete(string id)
        {
            _templateManager.DeleteTemplate(id);

            TempData["SuccessMessage"] = "Template deleted";

            return RedirectToAction("Index");
        }

        public ActionResult Clone(string id)
        {
            var template = _templateManager.GetTemplate(id ?? "0");

            if (template == null)
            {
                TempData["ErrorMessage"] = "Cloning failed: could not find Template " + id;
                return Index();
            }

            var clone = new EditScriptModuleTemplateRequest(template)
                            {
                                Id = null,
                                Title = string.Format("** CLONE **  {0}", template.Title),
                            };

            return Edit(null, clone);
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
                                    Templates = _templateManager.GetTemplates().OrderBy(x => x.Title),
                                };
            return PartialView("_ModuleGallery", viewModel);
        }


        // ##########################  Service Methods  ################################
        // These should/will be "REST" services, not controller actions


        [Route("modules/Script/1.0/data/json")]
        public ActionResult Save(string pageId, int? moduleId, [Bind(Prefix = "")]ScriptModule module)
        {
            if (moduleId == null)
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
    }
}