using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.Resources;

[assembly: WebResource(EmbeddedResources.Js.ColorPicker, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Common, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.CompositeComponent, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.CrossDomain, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.DowJonesJQueryExtensions, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Ellipsis, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.ErrorManager, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Highcharts, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQuery, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryCarousel, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryCycle, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryJScrollPane, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryJson, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryPopupBalloon, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryTimeAgo, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryTools, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryTouch, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUICore, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUIEffects, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUIInteractions, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUIWidgets, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUnobtrusiveAjax, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryValidate, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryValidateUnobtrusive, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Json2, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Overlay, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.ODSManager, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.PubSubManager, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Require, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.ServiceProxy, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Underscore, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.WindowManager, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Mvc.Resources
{
    public static class EmbeddedResources
    {

        [ScriptResource(ResourceName = ColorPicker, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "color-picker")]
        [ScriptResource(ResourceName = Common, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "common")]
        [ScriptResource(ResourceName = CompositeComponent, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "composite-component")]
        [ScriptResource(ResourceName = CrossDomain, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "crossdomain")]
        [ScriptResource(ResourceName = DowJonesJQueryExtensions, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "dj-jquery-ext")]
        [ScriptResource(ResourceName = Ellipsis, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "ellipsis")]
        [ScriptResource(ResourceName = ErrorManager, DependencyLevel = ClientResourceDependencyLevel.Global, Name = "error-manager")]
        [ScriptResource(ResourceName = Highcharts, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "highcharts")]
        [ScriptResource(ResourceName = JQuery, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "jquery")]
        [ScriptResource(ResourceName = JQueryCarousel, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-carousel")]
        [ScriptResource(ResourceName = JQueryCycle, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-cycle")]
        [ScriptResource(ResourceName = JQueryJScrollPane, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-scrollpane")]
        [ScriptResource(ResourceName = JQueryJson, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "jquery-json")]
        [ScriptResource(ResourceName = JQueryPopupBalloon, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-popupballon")]
        [ScriptResource(ResourceName = JQueryTimeAgo, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-timeago")]
        [ScriptResource(ResourceName = JQueryTools, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-tools")]
        [ScriptResource(ResourceName = JQueryTouch, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-touch")]
        [ScriptResource(ResourceName = JQueryUICore, DependencyLevel = ClientResourceDependencyLevel.Global, Name = "jquery-ui")]
        [ScriptResource(ResourceName = JQueryUIEffects, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-ui-effects", DependsOn = new[] { "jquery-ui" })]
        [ScriptResource(ResourceName = JQueryUIInteractions, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-ui-interactions", DependsOn = new[] { "jquery-ui" })]
        [ScriptResource(ResourceName = JQueryUIWidgets, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-ui-widgets", DependsOn = new[] { "jquery-ui" })]
        [ScriptResource(ResourceName = JQueryUnobtrusiveAjax, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-unobtrusive-ajax")]
        [ScriptResource(ResourceName = JQueryValidate, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-validate")]
        [ScriptResource(ResourceName = JQueryValidateUnobtrusive, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-validate-unobtrusive")]
        [ScriptResource(ResourceName = Json2, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "JSON")]
		[ScriptResource(ResourceName = ODSManager, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = Overlay, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "overlay")]
        [ScriptResource(ResourceName = PubSubManager, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "pubsub")]
        [ScriptResource(ResourceName = Require, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "require")]
        [ScriptResource(ResourceName = ServiceProxy, DependencyLevel = ClientResourceDependencyLevel.Global, Name = "service-proxy", DependsOn = new[] { "error-manager" })]
        [ScriptResource(ResourceName = Underscore, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "underscore")]
        [ScriptResource(ResourceName = WindowManager, DependencyLevel = ClientResourceDependencyLevel.Global, Name = "window-manager")]

        public static class Js
        {
            
            private const string JsRoot = "DowJones.Web.Mvc.Resources.js.";

            public const string ColorPicker = JsRoot + "color-picker.js";
            public const string Common = JsRoot + "common.js";
            public const string CompositeComponent = JsRoot + "composite-component.js";
            public const string CrossDomain = JsRoot + "crossdomain.js";
            public const string DowJonesJQueryExtensions = JsRoot + "dj-jquery-ext.js";
            public const string Ellipsis = JsRoot + "ellipsis.js";
            public const string ErrorManager = JsRoot + "error-manager.js";
            public const string Highcharts = JsRoot + "highcharts.js";
            public const string JQuery = JsRoot + "jquery.js";
            public const string JQueryCarousel = JsRoot + "jquery.carousel.js";
            public const string JQueryCycle = JsRoot + "jquery.cycle.all.js";
            public const string JQueryJScrollPane = JsRoot + "jquery.jscrollpane.js";
            public const string JQueryJson = JsRoot + "jquery-json.js";
            public const string JQueryPopupBalloon = JsRoot + "jquery.popupballoon.js";
            public const string JQueryTimeAgo = JsRoot + "jquery.timeago.js";
            public const string JQueryTools = JsRoot + "jquery.tools.min.js";
            public const string JQueryTouch = JsRoot + "jquery.touchwipe.js";
            public const string JQueryUICore = JsRoot + "jquery-ui.core.js";
            public const string JQueryUIEffects = JsRoot + "jquery-ui.effects.js";
            public const string JQueryUIInteractions = JsRoot + "jquery-ui.interactions.js";
            public const string JQueryUIWidgets = JsRoot + "jquery-ui.widgets.js";
            public const string JQueryUnobtrusiveAjax = JsRoot + "jquery.unobtrusive-ajax.js";
            public const string JQueryValidate = JsRoot + "jquery.validate.js";
            public const string JQueryValidateUnobtrusive = JsRoot + "jquery.validate.unobtrusive.js";
            public const string Json2 = JsRoot + "json2.js";
			public const string ODSManager = JsRoot + "ODSManager.js";
            public const string Overlay = JsRoot + "overlay.js";
            public const string PubSubManager = JsRoot + "pubsub.js";
            public const string Require = JsRoot + "require.js";
            public const string ServiceProxy = JsRoot + "service-proxy.js";
            public const string Underscore = JsRoot + "underscore.js";
            public const string WindowManager = JsRoot + "window-manager.js";
        }
    }
}
