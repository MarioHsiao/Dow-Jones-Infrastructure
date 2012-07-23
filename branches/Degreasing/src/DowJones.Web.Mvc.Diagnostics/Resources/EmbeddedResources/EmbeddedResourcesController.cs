using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.Diagnostics.Resources
{
    public class EmbeddedResourcesController : DiagnosticsController
    {
        private static IEnumerable<Assembly> _currentAssemblies;

        protected internal virtual IEnumerable<Assembly> CurrentAssemblies
        {
            get
            {
                if (_currentAssemblies == null)
                    _currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();

                return _currentAssemblies;
            }
            set { _currentAssemblies = value; }
        }


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
