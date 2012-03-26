using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.UI.Components.HeadlineListCarousel
{
    public class HeadlineListCarouselClientEventsBuilder : IAmFluent
    {
        private readonly HeadlineListCarouselModel _clientEvents;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadlineListCarouselClientEventsBuilder"/> class.
        /// </summary>
        /// <param name="clientEvents">Client events of the calendar.</param>
        public HeadlineListCarouselClientEventsBuilder(HeadlineListCarouselModel clientEvents)
        {
            Guard.IsNotNull(clientEvents, "clientEvents");

            this._clientEvents = clientEvents;
        }

        /// <summary>
        ///  Defines the name of the JavaScript function that will handle the OnHeadlineClick client-side event.
        /// </summary>
        /// <param name="OnHeadlineClickHandlerName">The name of the JavaScript function that will handle the event.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.DJ().HeadlineListCarousel()
        ///             .ClientEvents(events => events.OnHeadlineClick("OnHeadlineClick"))
        /// %&gt;
        /// </code>
        /// </example>
        public HeadlineListCarouselClientEventsBuilder OnHeadlineClick(string OnHeadlineClickHandlerName)
        {
            Guard.IsNotNullOrEmpty(OnHeadlineClickHandlerName, "OnHeadlineClickHandlerName");
            _clientEvents.OnHeadlineClick = OnHeadlineClickHandlerName;
            return this;

        }

        /// <summary>
        ///  Defines the name of the JavaScript function that will handle the OnHeadlineImageClick client-side event.
        /// </summary>
        /// <param name="OnHeadlineImageClickHandlerName">The name of the JavaScript function that will handle the event.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.DJ().HeadlineListCarousel()
        ///             .ClientEvents(events => events.OnHeadlineImageClick("OnHeadlineImageClick"))
        /// %&gt;
        /// </code>
        /// </example>
        public HeadlineListCarouselClientEventsBuilder OnHeadlineImageClick(string OnHeadlineImageClickHandlerName)
        {
            Guard.IsNotNullOrEmpty(OnHeadlineImageClickHandlerName, "OnHeadlineImageClickHandlerName");
            _clientEvents.OnHeadlineImageClick = OnHeadlineImageClickHandlerName;
            return this;

        }

        /// <summary>
        ///  Defines the name of the JavaScript function that will handle the OnExtensionItemClick client-side event.
        /// </summary>
        /// <param name="OnExtensionItemClickHandlerName">The name of the JavaScript function that will handle the event.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.DJ().HeadlineListCarousel()
        ///             .ClientEvents(events => events.OnExtensionItemClick("OnExtensionItemClick"))
        /// %&gt;
        /// </code>
        /// </example>
        public HeadlineListCarouselClientEventsBuilder OnExtensionItemClick(string OnExtensionItemClickHandlerName)
        {
            Guard.IsNotNullOrEmpty(OnExtensionItemClickHandlerName, "OnExtensionItemClickHandlerName");
            _clientEvents.OnExtensionItemClick = OnExtensionItemClickHandlerName;
            return this;

        }

    }
}
