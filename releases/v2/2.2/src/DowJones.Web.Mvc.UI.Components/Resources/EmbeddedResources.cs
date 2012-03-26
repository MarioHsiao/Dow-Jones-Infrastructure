using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Components.Resources;

#region Scripts

[assembly: WebResource(EmbeddedResources.Js.JQueryThreeDots, KnownMimeTypes.JavaScript)]

#endregion


namespace DowJones.Web.Mvc.UI.Components.Resources
{

    public static class EmbeddedResources
    {

        [ScriptResource(ResourceName = JQueryThreeDots, DependencyLevel = ClientResourceDependencyLevel.Component)]

        public static class Js
        {
            public const string JQueryThreeDots = "DowJones.Web.Mvc.UI.Components.Resources.js.jquery.ThreeDots.js";
        }

    }

}