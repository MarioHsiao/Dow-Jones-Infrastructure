using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Canvas;

[assembly: WebResource($rootnamespace$.$safeitemname$.ScriptFile, KnownMimeTypes.JavaScript)]

namespace $rootnamespace$
{
    [DowJones.Web.ScriptResource("$safeitemname$", ResourceName = ScriptFile)]
    public class $safeitemname$ : AbstractCanvas<$safeitemname$Model>
    {
        internal const string ScriptFile = "$rootnamespace$.$safeitemname$.js";

        public override string ClientPluginName
        {
            get { return "dj_$safeitemname$"; }
        }
    }
}