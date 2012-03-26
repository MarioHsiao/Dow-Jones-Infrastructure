using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.Resources;

#region Scripts

[assembly: WebResource(EmbeddedResources.Js.DateFormat, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.DowJonesCommon, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.ErrorManager, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Highcharts, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQuery, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryCycle, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryJScrollPane, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryJson, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryPopupBalloon, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUnobtrusiveAjax, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUICore, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUIEffects, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUIInteractions, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUIWidgets, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryValidate, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryValidateUnobtrusive, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Json2, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Modernizer, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.PubSubManager, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.ServiceProxy, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Underscore, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.WindowManager, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.YepNope, KnownMimeTypes.JavaScript)]

#endregion

namespace DowJones.Web.Mvc.Resources
{
    public static class EmbeddedResources
    {

        [ScriptResource(ResourceName = ColorPicker, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = DateFormat, DependencyLevel = ClientResourceDependencyLevel.Global)]
        [ScriptResource(ResourceName = DowJonesCommon, DependencyLevel = ClientResourceDependencyLevel.Core)]
        [ScriptResource(ResourceName = ErrorManager, DependencyLevel = ClientResourceDependencyLevel.Global)]
        [ScriptResource(ResourceName = Highcharts, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQuery, DependencyLevel = ClientResourceDependencyLevel.Core)]
        [ScriptResource(ResourceName = JQueryCycle, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryJScrollPane, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryJson, DependencyLevel = ClientResourceDependencyLevel.Core)]
        [ScriptResource(ResourceName = JQueryPopupBalloon, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryTimeAgo, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryTouch, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryUICore, DependencyLevel = ClientResourceDependencyLevel.Global)]
        [ScriptResource(ResourceName = JQueryUIEffects, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryUIInteractions, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryUIWidgets, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryUnobtrusiveAjax, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryValidate, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryValidateUnobtrusive, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = Json2, DependencyLevel = ClientResourceDependencyLevel.Core)]
        [ScriptResource(ResourceName = Modernizer, DependencyLevel = ClientResourceDependencyLevel.Core)]
        [ScriptResource(ResourceName = ODSManager, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = PubSubManager, DependencyLevel = ClientResourceDependencyLevel.Global)]
        [ScriptResource(ResourceName = ServiceProxy, DependencyLevel = ClientResourceDependencyLevel.Global)]
        [ScriptResource(ResourceName = Underscore, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = WindowManager, DependencyLevel = ClientResourceDependencyLevel.Global)]
        [ScriptResource(ResourceName = YepNope, DependencyLevel = ClientResourceDependencyLevel.Core)]

        public static class Js
        {
            private const string JsRoot = "DowJones.Web.Mvc.Resources.js.";

            public const string ColorPicker = JsRoot + "colorPicker.js";
            public const string DateFormat = JsRoot + "dateFormat.js";
            public const string DowJonesCommon = JsRoot + "common.js";
            public const string ErrorManager = JsRoot + "ErrorManager.js";
            public const string Highcharts = JsRoot + "highcharts.js";
            public const string JQuery = JsRoot + "jquery.js";
            public const string JQueryCycle = JsRoot + "jquery.cycle.all.js";
            public const string JQueryJScrollPane = JsRoot + "jquery.jscrollpane.js";
            public const string JQueryJson = JsRoot + "jquery-json.js";
            public const string JQueryPopupBalloon = JsRoot + "jquery.popupballoon.js";
            public const string JQueryTouch = JsRoot + "jquery.touchwipe.js";
            public const string JQueryTimeAgo = JsRoot + "jquery.timeago.js";
            public const string JQueryUICore = JsRoot + "jquery-ui.core.js";
            public const string JQueryUIEffects = JsRoot + "jquery-ui.effects.js";
            public const string JQueryUIInteractions = JsRoot + "jquery-ui.interactions.js";
            public const string JQueryUIWidgets = JsRoot + "jquery-ui.widgets.js";
            public const string JQueryUnobtrusiveAjax = JsRoot + "jquery.unobtrusive-ajax.js";
            public const string JQueryValidate = JsRoot + "jquery.validate.js";
            public const string JQueryValidateUnobtrusive = JsRoot + "jquery.validate.unobtrusive.js";
            public const string Json2 = JsRoot + "json2.js";
            public const string Modernizer = JsRoot + "modernizr.js";
            public const string ODSManager = JsRoot + "ODSManager.js";
            public const string PubSubManager = JsRoot + "PubSubManager.js";
            public const string ServiceProxy = JsRoot + "ServiceProxy.js";
            public const string Underscore = JsRoot + "underscore.js";
            public const string WindowManager = JsRoot + "WindowManager.js";
            public const string YepNope = JsRoot + "yepnope.js";
        }

    }

}
