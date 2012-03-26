using System.Reflection;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Components.CalloutPopup;

[assembly: System.Web.UI.WebResourceAttribute(CalloutPopupHelper.CalloutpopupScriptResourceName, "text/javascript")]

namespace DowJones.Web.Mvc.UI.Components.CalloutPopup
{

    public static class CalloutPopupHelper
    {
        private static readonly Assembly TargetAssembly = typeof(CalloutPopupHelper).Assembly;
        internal const string CalloutpopupScriptResourceName = "DowJones.Web.Mvc.UI.Components.CalloutPopup.js.popup.js";
        //internal const string CalloutjQuerypopupScriptResourceName = "DowJones.Web.Mvc.UI.Components.CalloutPopup.js.jquery.popupballoon.js";
        //internal const string CalloutjQueryScrollScriptResourceName = "DowJones.Web.Mvc.UI.Components.CalloutPopup.js.jquery.jscrollpane.js";
        public static ViewComponentFactory CalloutPopup(this ViewComponentFactory factory)
        {
            factory.ScriptRegistry().WithPopupBalloon().WithJScrollPane().IncludeResource(
                    TargetAssembly,
                    CalloutpopupScriptResourceName,
                    ClientResourceDependencyLevel.MidLevel,
                    performSubstitution: false
                );
            return factory;
        }

    }
}
