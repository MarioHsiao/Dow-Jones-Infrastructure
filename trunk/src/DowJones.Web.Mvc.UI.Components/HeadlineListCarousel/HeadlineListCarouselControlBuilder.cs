using System;
using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.UI.Components.HeadlineListCarousel
{
    public class HeadlineListCarouselControlBuilder : ViewComponentBuilderBase<HeadlineListCarouselControl, HeadlineListCarouselControlBuilder>
    {
        protected HeadlineListCarouselClientEventsBuilder ClientEventsBuilder { get; private set; }

        public HeadlineListCarouselControlBuilder(HeadlineListCarouselControl component)
            : base(component)
        {
            ClientEventsBuilder = new HeadlineListCarouselClientEventsBuilder(component.Model);
        }

        public HeadlineListCarouselControlBuilder Name(string value) { Component.Name = value; return this; }

        /// <summary>
        /// Configures the client-side events.
        /// </summary>
        /// <param name="clientEventsAction">The client events action.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.DJ().HeadlineList()
        ///             .Name("HeadlineList")
        ///             .ClientEvents(events =>
        ///                 events.OnHeadlineClick("OnHeadlineClick")
        ///             )
        /// %&gt;
        /// </code>
        /// </example>
        public HeadlineListCarouselControlBuilder ClientEvents(Action<HeadlineListCarouselClientEventsBuilder> clientEventsAction)
        {
            Guard.IsNotNull(clientEventsAction, "clientEventsAction");

            clientEventsAction(ClientEventsBuilder);

            return this;
        }
    }
}

namespace DowJones.Web.Mvc.UI.Extensions
{
    using Components.HeadlineListCarousel;

    public static class HeadlineListCarouselControlBuilderHtmlHelperExtensions
    {
        public static HeadlineListCarouselControlBuilder HeadlineListCarousel(this ViewComponentFactory viewComponentFactory)
        {
            var viewContext = viewComponentFactory.HtmlHelper.ViewContext;
            var defaultData = viewContext.ViewData.Model as HeadlineListCarouselModel ?? new HeadlineListCarouselModel();

            var headlineListCarouselControl = viewComponentFactory.Create<HeadlineListCarouselControl>(defaultData);

            var builder = HeadlineListCarouselControlBuilder.Create(headlineListCarouselControl);

            return builder;
        }
    }
}
