using System.IO;
using System.Linq;
using System.Web;
using DowJones.DependencyInjection;
using DowJones.Pages.Modules.Templates;

namespace DowJones.Web.Mvc.UI.Canvas.Modules.ScriptModule
{
    public class ScriptModuleHandler : HttpHandlerBase
    {
        [Inject("Cannot use constructor injection with HttpHandler")]
        public IScriptModuleTemplateManager TemplateManager { get; set; }

        public override void OnProcessRequest(HttpContextBase context)
        {
            var filename = context.Request.Url.Segments.Last();
            var templateId = Path.GetFileNameWithoutExtension(filename);
            var extension = Path.GetExtension(filename);

            RenderTemplate(context, templateId, extension);
        }

        public void RenderTemplate(HttpContextBase context, string templateId, string extension)
        {
            var template = TemplateManager.GetTemplate(templateId);

            if(template == null)
            {
                context.Response.StatusCode = 404;
                context.Response.StatusDescription = string.Format("Script Template {0} not found", templateId);
                return;
            }

            if (extension == ".css")
            {
                context.Response.ContentType = KnownMimeTypes.Stylesheet;
                context.Response.Write(template.Styles);
            }
            else
            {
                context.Response.ContentType = KnownMimeTypes.JavaScript;
                context.Response.Write(template.Script);
            }
        }
    }
}
