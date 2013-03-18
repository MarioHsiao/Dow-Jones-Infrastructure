using System;
using System.Web.Mvc;
using System.Web.UI;
using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.UI
{
    public class ViewComponentViewResult : PartialViewResult
    {
        private readonly ViewComponentFactory _componentFactory;

        public string Callback { get; set; }


        public ViewComponentViewResult(ViewComponentFactory componentFactory)
        {
            Guard.IsNotNull(componentFactory, "componentFactory");

            _componentFactory = componentFactory;
        }


        public override void ExecuteResult(ControllerContext context)
        {
            string clientId;
            ExecuteResult(context, new HtmlTextWriter(context.HttpContext.Response.Output), out clientId);
        }

        public void ExecuteResult(ControllerContext context, HtmlTextWriter output, out string clientId)
        {
            var scriptRegistry = _componentFactory.ScriptRegistry();

            var viewComponent = _componentFactory.Create(Model) as ViewComponentBase;
            viewComponent.Html = new HtmlHelper(new ViewContext(context, viewComponent, ViewData, TempData, output), viewComponent);

            if (string.IsNullOrEmpty(viewComponent.ClientID))
                viewComponent.ClientID = string.Format("{0}_{1}", viewComponent.GetType().Name, Guid.NewGuid().ToString().Replace("-", ""));

            if (!string.IsNullOrWhiteSpace(Callback))
            {
                viewComponent.EnsureClientID();
                scriptRegistry.OnDocumentReady(string.Format("{0}({{ 'id': '{1}' }});", Callback, viewComponent.ClientID));
            }

            clientId = viewComponent.ClientID;

            viewComponent.Render(output);

            if(!context.IsChildAction)
            {
                _componentFactory.StylesheetRegistry().Render(output);
                scriptRegistry.Render(output);
            }
        }
    }
}