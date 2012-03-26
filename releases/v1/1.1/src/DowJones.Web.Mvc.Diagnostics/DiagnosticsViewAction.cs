using System.Web.Mvc;
using DowJones.Web.Mvc.Diagnostics.Fakes;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Mvc.Diagnostics
{
    public class DiagnosticsViewAction<TView> : ContentResult 
        where TView : ViewComponentBase, new()
    {
        public object Model { get; set; }

        public DiagnosticsViewAction(object model = null)
        {
            Model = model;
            ContentType = "text/html";
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