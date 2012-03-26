using System.Web.UI;
using DowJones.Web;

[assembly: WebResource("DowJones.Web.Mvc.UI.Components.SocialButtons.SocialButtons.js", KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Mvc.UI.Components.SocialButtons
{
    /// <summary>
    /// The social buttons.
    /// </summary>
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName = "DowJones.Web.Mvc.UI.Components.SocialButtons.SocialButtons.js", ResourceKind = DowJones.Web.ClientResourceKind.Script, DeclaringType = typeof(DowJones.Web.Mvc.UI.Components.SocialButtons.SocialButtonsControl))]
    public class SocialButtonsControl : ViewComponentBase<SocialButtonsModel>
    {
        public override string ClientPluginName
        {
            get { return "dj_SocialButtons"; }
        }

        public SocialButtonsControl()
        {
            //TagName = "ul";
            //CssClass = "actions";
        }
    }
}
