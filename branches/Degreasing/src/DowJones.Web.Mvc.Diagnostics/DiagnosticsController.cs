using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Mvc.Diagnostics
{
    public abstract class DiagnosticsController : Controller
    {
        protected internal static IEnumerable<Assembly> CurrentAssemblies
        {
            get
            {
                if (_currentAssemblies == null)
                    _currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();

                return _currentAssemblies;
            }
            set { _currentAssemblies = value; }
        }
        private static IEnumerable<Assembly> _currentAssemblies;


        protected DiagnosticsViewAction<TView> View<TView>(object model = null, string contentType = null) 
            where TView : ViewComponentBase, new()
        {
            return new DiagnosticsViewAction<TView>(model);
        }

    }
}
