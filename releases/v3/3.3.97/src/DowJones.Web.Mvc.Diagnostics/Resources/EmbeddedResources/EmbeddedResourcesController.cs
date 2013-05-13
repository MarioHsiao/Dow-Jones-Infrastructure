using System.Linq;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.Diagnostics.Resources
{
    public class EmbeddedResourcesController : DiagnosticsController
    {
        public ActionResult Index()
        {
            return Index(new EmbeddedResourceDebuggerRequest());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(EmbeddedResourceDebuggerRequest request)
        {
            request.ReferencedAssemblies = 
                new SelectList(
                    CurrentAssemblies.OrderBy(x => x.FullName), 
                    "FullName",
                    "FullName", 
                    request.AssemblyName
                );

            return View<EmbeddedResourceDebuggerView>(request);
        }
    }
}
