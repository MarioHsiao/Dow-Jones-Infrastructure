using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Components.RealtimeHeadlineList;

[assembly: WebResource(RealtimeHeadlineListComponent.ScriptFile, KnownMimeTypes.JavaScript)]
[assembly: WebResource(RealtimeHeadlineListComponent.BASE_IMAGE_DIRECTORY + "list-header.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineListComponent.BASE_IMAGE_DIRECTORY + "play.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineListComponent.BASE_IMAGE_DIRECTORY + "play_blue.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineListComponent.BASE_IMAGE_DIRECTORY + "pause.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineListComponent.BASE_IMAGE_DIRECTORY + "pause_blue.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineListComponent.BASE_IMAGE_DIRECTORY + "previous.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineListComponent.BASE_IMAGE_DIRECTORY + "next.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineListComponent.BASE_IMAGE_DIRECTORY + "status_green.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineListComponent.BASE_IMAGE_DIRECTORY + "status_red.png", "image/png")]
[assembly: WebResource(RealtimeHeadlineListComponent.BASE_IMAGE_DIRECTORY + "refresh.png", "image/png")]

namespace DowJones.Web.Mvc.UI.Components.RealtimeHeadlineList
{
    // Auto-Registered Client Resources
    [ScriptResource("RealtimeHeadlineListScript", ResourceName = ScriptFile, DeclaringType = typeof(RealtimeHeadlineListComponent))]

    public class RealtimeHeadlineListComponent : ViewComponentBase<RealtimeHeadlineListModel>
    {
        internal const string BaseDirectory = "DowJones.Web.Mvc.UI.Components.RealtimeHeadlineList";
        internal const string BASE_IMAGE_DIRECTORY = BaseDirectory + ".images.";
        internal const string ScriptFile = BaseDirectory + ".RealtimeHeadlineList.js";

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