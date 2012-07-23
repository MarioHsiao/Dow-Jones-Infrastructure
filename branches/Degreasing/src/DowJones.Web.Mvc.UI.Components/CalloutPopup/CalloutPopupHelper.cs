using System.Reflection;
using DowJones.Web.Mvc.UI.Components.CalloutPopup;

[assembly: System.Web.UI.WebResourceAttribute(CalloutPopupHelper.CalloutpopupScriptResourceName, "text/javascript")]

namespace DowJones.Web.Mvc.UI.Components.CalloutPopup
{

    public static class CalloutPopupHelper
    {
        private static readonly Assembly TargetAssembly = typeof(CalloutPopupHelper).Assembly;
        internal const string CalloutpopupScriptResourceName = "DowJones.Web.Mvc.UI.Components.CalloutPopup.js.popup.js";

        public static ViewComponentFactory CalloutPopup(this ViewComponentFactory factory)
        {
            factory.ScriptRegistry().WithPopupBalloon().WithJScrollPane().IncludeResource(
                    TargetAssembly,
                    CalloutpopupScriptResourceName,
                    ClientResourceDependencyLevel.MidLevel
                );
            return factory;
        }

    }
}
