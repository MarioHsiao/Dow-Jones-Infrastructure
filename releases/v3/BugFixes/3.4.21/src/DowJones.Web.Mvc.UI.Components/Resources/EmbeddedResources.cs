using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Components.Resources;

#region Scripts

[assembly: WebResource(EmbeddedResources.Js.JQueryThreeDots, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryTooltip, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JCarouselLite, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryImagesLoaded, KnownMimeTypes.JavaScript)]

#endregion


namespace DowJones.Web.Mvc.UI.Components.Resources
{

    public static class EmbeddedResources
    {

		[ScriptResource(ResourceName = JQueryThreeDots, DependencyLevel = ClientResourceDependencyLevel.Component, Name = "jquery-three-dots")]
        [ScriptResource(ResourceName = JQueryTooltip, DependencyLevel = ClientResourceDependencyLevel.Component, Name = "jquery-tooltip")]
        [ScriptResource(ResourceName = JCarouselLite, DependencyLevel = ClientResourceDependencyLevel.Component, Name = "jcarousel-lite")]
        [ScriptResource(ResourceName = JQueryImagesLoaded, DependencyLevel = ClientResourceDependencyLevel.Component, Name = "jquery-imagesLoaded")]

        public static class Js
        {
            public const string JQueryThreeDots = "DowJones.Web.Mvc.UI.Components.Resources.js.jquery.ThreeDots.js";
            public const string JQueryTooltip = "DowJones.Web.Mvc.UI.Components.Resources.js.jquery.tooltip.js";
            public const string JCarouselLite = "DowJones.Web.Mvc.UI.Components.Resources.js.jcarousellite.js";
            public const string JQueryImagesLoaded = "DowJones.Web.Mvc.UI.Components.Resources.js.jquery.imagesloaded.min.js";
        }

    }

}