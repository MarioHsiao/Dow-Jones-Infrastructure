using System.Web.Mvc;
using DowJones.Web.Mvc.Diagnostics.Fakes;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Mvc.Diagnostics
{
    public abstract class DiagnosticsViewAction : ContentResult
    {
        public object Model { get; set; }

        protected DiagnosticsViewAction(object model = null)
        {
            Model = model;
            ContentType = "text/html";
        }
    }

    public class DiagnosticsViewAction<TView> : DiagnosticsViewAction 
        where TView : ViewComponentBase, new()
    {
        public DiagnosticsViewAction(object model = null)
            : base(model)
        {
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var template = new TView
                               {
                                   Html = FakeHtmlHelper.Create(context, Model),
                                   Model = Model, 
                                   TagName = "html",
                               };

            Content = template.ToHtmlString();
            
            base.ExecuteResult(context);
        }
    }
}