using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Components.SocialButtons;

#region ..:: Embedded Resources ::..

[assembly: WebResource(SocialButtonsControl.CORE_RESOURCE_FILE, KnownMimeTypes.JavaScript)]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.SocialButtons.SocialButtons.css", "text/css", PerformSubstitution = true)]

#endregion

namespace DowJones.Web.Mvc.UI.Components.SocialButtons
{
    /// <summary>
    /// The social buttons.
    /// </summary>
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName = "DowJones.Web.Mvc.UI.Components.SocialButtons.SocialBehavior.js", ResourceKind = DowJones.Web.ClientResourceKind.Script, DeclaringType = typeof(DowJones.Web.Mvc.UI.Components.SocialButtons.SocialButtonsControl))]
    [DowJones.Web.StylesheetResourceAttribute(null, ResourceName = "DowJones.Web.Mvc.UI.Components.SocialButtons.SocialButtons.css", ResourceKind = DowJones.Web.ClientResourceKind.Stylesheet, DeclaringType = typeof(DowJones.Web.Mvc.UI.Components.SocialButtons.SocialButtonsControl))]
    public class SocialButtonsControl : ViewComponentBase<SocialButtonsModel>
    {
        #region ..:: Constants ::..

        // <summary>
        /// Base Resource Directory
        /// </summary>
        internal const string BASE_RESOURCE_DIRECTORY = "DowJones.Web.Mvc.UI.Components.SocialButtons";
       
        /// <summary>
        /// Core Resource File 'SocialBehavior.js'
        /// </summary>
        internal const string CORE_RESOURCE_FILE = BASE_RESOURCE_DIRECTORY + ".SocialBehavior.js";

        internal const string CORE_STYLE_FILE = BASE_RESOURCE_DIRECTORY + ".SocialButtons.css";

        #endregion

        public override string ClientPluginName
        {
            get { return "dj_SocialButtons"; }
        }

        protected override void WriteContent(HtmlTextWriter writer)
        {

        }
    }
}
