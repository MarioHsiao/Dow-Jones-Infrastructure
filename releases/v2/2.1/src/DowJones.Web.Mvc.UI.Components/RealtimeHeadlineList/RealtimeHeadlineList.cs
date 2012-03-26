using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Components.RealtimeHeadlineList;

#region [Embeded Resources]
[assembly: WebResource(RealtimeHeadlineList.ScriptFile, KnownMimeTypes.JavaScript)]
[assembly: WebResource(RealtimeHeadlineList.Stylesheet, KnownMimeTypes.Stylesheet, PerformSubstitution=true)]
#region Images
[assembly: WebResource(RealtimeHeadlineList.BASE_IMAGE_DIRECTORY + "list-header.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineList.BASE_IMAGE_DIRECTORY + "play.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineList.BASE_IMAGE_DIRECTORY + "play_blue.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineList.BASE_IMAGE_DIRECTORY + "pause.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineList.BASE_IMAGE_DIRECTORY + "pause_blue.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineList.BASE_IMAGE_DIRECTORY + "previous.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineList.BASE_IMAGE_DIRECTORY + "next.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineList.BASE_IMAGE_DIRECTORY + "status_green.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineList.BASE_IMAGE_DIRECTORY + "status_red.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineList.BASE_IMAGE_DIRECTORY + "refresh.png", "image/png")]
#endregion
#endregion

namespace DowJones.Web.Mvc.UI.Components.RealtimeHeadlineList
{
    // Auto-Registered Client Resources
    [ScriptResource("RealtimeHeadlineListScript", ResourceName = ScriptFile, DeclaringType = typeof(RealtimeHeadlineList))]
    [StylesheetResource("RealtimeHeadlineListStylesheet", ResourceName = Stylesheet, DeclaringType = typeof(RealtimeHeadlineList))]

    public class RealtimeHeadlineList : ViewComponentBase<RealtimeHeadlineListModel>
    {
        internal const string BaseDirectory = "DowJones.Web.Mvc.UI.Components.RealtimeHeadlineList";
        internal const string BASE_IMAGE_DIRECTORY = BaseDirectory + ".images.";
        // The JavaScript file for this module
        internal const string ScriptFile = BaseDirectory + ".RealtimeHeadlineList.js";

        // The CSS Stylesheet for this module
        internal const string Stylesheet = BaseDirectory + ".RealtimeHeadlineList.css";


        // The function name for the jQuery plugin
        // e.g.  $('.my-module').dj_RealtimeHeadlineList();
        // Return null if this component does not have a jQuery plugin
        public override string ClientPluginName
        {
            get { return "dj_RealtimeHeadlineList"; }
        }

        protected override void WriteContent(HtmlTextWriter writer)
        {
            ComponentFactory.ScriptRegistry().WithJQueryThreeDots();

            var manager = new RealtimeHeadlineListDataResultRenderManager(Model.Result)
            {
                ContainerWidth = Model.ContainerWidth,
                AlertContext = Model.AlertContext,
                MaxHeadlinesToReturn = Model.MaxHeadlinesToReturn,
                ClockType = Model.ClockType,
                DateTimeFormatingPreference = Model.DateTimeFormatingPreference,
                Tokens = Model.Tokens,
                Id = ClientID
            };

            writer.Write(manager.GetHtml());
        }

    }
}