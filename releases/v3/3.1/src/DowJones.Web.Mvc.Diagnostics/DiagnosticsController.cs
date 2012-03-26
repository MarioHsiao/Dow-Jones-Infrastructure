using System.Web.Mvc;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Mvc.Diagnostics
{
    public abstract class DiagnosticsController : Controller
    {

        protected DiagnosticsViewAction<TView> View<TView>(object model = null, string contentType = null) 
            where TView : ViewComponentBase, new()
        {
            return new DiagnosticsViewAction<TView>(model);
        }

    }
}
