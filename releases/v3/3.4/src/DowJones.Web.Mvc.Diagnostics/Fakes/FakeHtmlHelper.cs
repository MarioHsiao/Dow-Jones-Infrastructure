using System.IO;
using System.Web.Mvc;
using System.Web.Routing;

namespace DowJones.Web.Mvc.Diagnostics.Fakes
{
    public class FakeHtmlHelper : HtmlHelper
    {
        public FakeHtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer) : base(viewContext, viewDataContainer)
        {
        }

        public FakeHtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer, RouteCollection routeCollection) : base(viewContext, viewDataContainer, routeCollection)
        {
        }

        public static HtmlHelper Create(ControllerContext controllerContext = null, object model = null)
        {
            var dataContainer = new FakeViewDataContainer(model);
            var viewContent = new FakeViewContext(controllerContext, dataContainer.ViewData);

            return new HtmlHelper(viewContent, dataContainer);
        }
    }

    public class FakeViewContext : ViewContext
    {
        public FakeViewContext(ControllerContext controllerContext, ViewDataDictionary viewData, TempDataDictionary tempData = null)
            : base(controllerContext, new FakeView(), viewData, tempData ?? new TempDataDictionary(), new StringWriter())
        {
        }
    }
}
