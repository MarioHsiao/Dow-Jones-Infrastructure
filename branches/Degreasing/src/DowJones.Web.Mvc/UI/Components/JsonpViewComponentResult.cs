using System;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;
using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.UI.Components
{
    public class JsonpViewComponentResult : JsonpResult
    {
        public ViewComponentViewResult ViewComponent;

        public JsonpViewComponentResult(ViewComponentViewResult viewComponent = null)
        {
            ViewComponent = viewComponent;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            Guard.IsNotNull(ViewComponent, "ViewComponent");

            using (var componentHtmlWriter = new StringWriter())
            {
                string clientId;
                ViewComponent.ExecuteResult(context, new HtmlTextWriter(componentHtmlWriter), out clientId);

                Model = new { id = clientId, html = componentHtmlWriter.ToString() };
            }

            base.ExecuteResult(context);
        }
    }
}