using System;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public class CanvasModuleViewResult : ViewComponentViewResult
    {
        public CanvasModuleViewResult(ViewComponentFactory componentFactory) 
            : base(componentFactory)
        {
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (Model == null || !(Model is IModule))
                throw new CanvasModuleModelNotSpecifiedException();

            base.ExecuteResult(context);
        }

        public class CanvasModuleModelNotSpecifiedException : Exception
        {
            public CanvasModuleModelNotSpecifiedException()
                : base("Could not execute CanvasModuleViewResult because no Canvas Module model was specified")
            {
            }
        }
    }
}
