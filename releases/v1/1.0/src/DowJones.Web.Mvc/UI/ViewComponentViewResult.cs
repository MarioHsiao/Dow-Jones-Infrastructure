using System;
using System.Web.Mvc;
using System.Web.UI;
using DowJones.DependencyInjection;

namespace DowJones.Web.Mvc.UI
{
    public class ViewComponentViewResult : PartialViewResult
    {

        public override void ExecuteResult(ControllerContext context)
        {
            var componentFactory = ServiceLocator.Resolve<ViewComponentFactory>();
            var scriptRegistry = componentFactory.ScriptRegistry();
            
            var viewComponent = componentFactory.Create(Model) as ViewComponentBase;

            if (string.IsNullOrEmpty(viewComponent.ClientID))
                viewComponent.ClientID = string.Format("{0}_{1}", viewComponent.GetType().Name, Guid.NewGuid().ToString().Replace("-", ""));

            string callback = ViewBag.Callback;
            if (!string.IsNullOrWhiteSpace(callback))
            {
                viewComponent.EnsureClientID();
                scriptRegistry.OnDocumentReady(string.Format("{0}('{1}');", callback, viewComponent.ClientID));
            }

            using(var writer = new HtmlTextWriter(context.HttpContext.Response.Output))
            {
                viewComponent.Render(writer);
                componentFactory.StylesheetRegistry().Render(writer);
                scriptRegistry.RenderPartial(writer);
            }
        }

    }
}