// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeadlineListConversionManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the HeadlineListConversionManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Net;
using System.Text.RegularExpressions;
using Argotic.Common;
using Argotic.Syndication;
using DowJones.Ajax.HeadlineList;
using DowJones.Extensions;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Mapping;
using DowJones.Url;
using Factiva.Gateway.Messages.PCM.Search.V1_0;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Trigger.V1_1;
using log4net;
using AccessionNumberSearchResponse = DowJones.Managers.Search.Responses.AccessionNumberSearchResponse;

namespace DowJones.Assemblers.Headlines
{
    /// <summary>
    /// </summary>
    public class HeadlineListConversionManager :
        ITypeMapper<Factiva.Gateway.Messages.Search.V2_0.PerformContentSearchResponse, HeadlineListDataResult>,
        ITypeMapper<IPerformContentSearchResponse, HeadlineListDataResult>,
        ITypeMapper<AccessionNumberSearchResponse, HeadlineListDataResult>
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(HeadlineListConversionManager));
        private readonly DateTimeFormatter datetimeFormatter;
 
        /// <summary>
        /// Initializes a new instance of the <see cref="HeadlineListConversionManager"/> class.
        /// </summary>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public HeadlineListConversionManager(DateTimeFormatter dateTimeFormatter)
        {
            datetimeFormatter = dateTimeFormatter;
        }
                                                  
        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="generateExternalUrlForHeadlineInfo">The generate external URL.</param>
        /// <param name="generateSnippetThumbnailForHeadlineInfo">The update thumbnail.</param>
        /// <returns>A HeadlineListDataResult object</returns>
        public HeadlineListDataResult Process(IPerformContentSearchResponse response, int startIndex, GenerateExternalUrlForHeadlineInfo generateExternalUrlForHeadlineInfo = null, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo = null)
        {
            var converter = new PerformContentSearchResponseConverter(response, startIndex, datetimeFormatter);
            return (HeadlineListDataResult)converter.Process(generateExternalUrlForHeadlineInfo, generateSnippetThumbnailForHeadlineInfo);
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="generateExternalUrlForHeadlineInfo">The generate external URL.</param>
        /// <param name="generateSnippetThumbnailForHeadlineInfo">The update thumbnail.</param>
        /// <returns>A HeadlineListDataResult object</returns>
        public HeadlineListDataResult Process(IPerformContentSearchResponse response, GenerateExternalUrlForHeadlineInfo generateExternalUrlForHeadlineInfo = null, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo = null)
        {
            var converter = new PerformContentSearchResponseConverter(response, -1, datetimeFormatter);
            return (HeadlineListDataResult)converter.Process(generateExternalUrlForHeadlineInfo, generateSnippetThumbnailForHeadlineInfo);
        }

        /// <param name="response">The response.</param>
        /// <param name="generateExternalUrlForHeadlineInfo">The generate external URL.</param>
        /// <param name="generateSnippetThumbnailForHeadlineInfo">The update thumbnail.</param>
        /// <returns>A HeadlineListDataResult object</returns>
        public HeadlineListDataResult Process(AccessionNumberSearchResponse response, GenerateExternalUrlForHeadlineInfo generateExternalUrlForHeadlineInfo = null, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo = null)
        {
            var converter = new AccessionNumberSearchResponseConverter(response, datetimeFormatter);
            return (HeadlineListDataResult)converter.Process(generateExternalUrlForHeadlineInfo, generateSnippetThumbnailForHeadlineInfo);
        }

        /// <param name="response">The response.</param>
        /// <param name="generateExternalUrlForHeadlineInfo">The generate external URL.</param>
        /// <param name="generateSnippetThumbnailForHeadlineInfo">The update thumbnail.</param>
        /// <returns>A HeadlineListDataResult object</returns>
        public HeadlineListDataResult Process(PCMGetCollectionHeadlinesByCodeResponse response, GenerateExternalUrlForHeadlineInfo generateExternalUrlForHeadlineInfo = null, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo = null)
        {
            var converter = new PCMAccessionNumberSearchResponseConverter(response, datetimeFormatter);
            return (HeadlineListDataResult)converter.Process(generateExternalUrlForHeadlineInfo, generateSnippetThumbnailForHeadlineInfo);
        }

        public SyndicationDataResult ProcessFeed(string feedUri)
        {
            var feedTitle = string.Empty;
            var settings = new SyndicationResourceLoadSettings
            {
                Timeout = new TimeSpan(0, 0, 1),
                RetrievalLimit = 10,
            };

            var feed = GenericSyndicationFeed.Create(new EscapedUri(feedUri), settings);
            IListDataResultConverter converter = null;
            switch (feed.Format)
            {
                case SyndicationContentFormat.Rss:
                    {
                        var rssFeed = feed.Resource as RssFeed;
                        if (rssFeed != null)
                        {
                            converter = new RssFeedConverter(rssFeed, datetimeFormatter);
                            feedTitle = rssFeed.Channel.Title;
                        }
                    }

                    break;
                case SyndicationContentFormat.Atom:
                    {
                        var atomFeed = feed.Resource as AtomFeed;
                        if (atomFeed != null)
                        {
                            converter = new AtomFeedConverter(atomFeed, datetimeFormatter);
                            feedTitle = atomFeed.Title.ToString();
                        }
                    }

                    break;
            }

           return new SyndicationDataResult
                      {
                          feedTitle = CleanFeedTitle(feedTitle),
                          result = converter != null ? (HeadlineListDataResult)converter.Process() : null,
                      };
        }
        
        /// <summary>
        /// Processes the RSS feed URI.
        /// </summary>
        /// <param name="feedUri">The RSS feed URI.</param>
        /// <param name="feedTitle">The feed title.</param>
        /// <returns>A HeadlineListDataResult object</returns>
        public HeadlineListDataResult Process(string feedUri, out string feedTitle)
        {
            feedTitle = string.Empty;
            var settings = new SyndicationResourceLoadSettings
            {
                Timeout = new TimeSpan(0, 0, 1),
                RetrievalLimit = 10,
            };

            var feed = GenericSyndicationFeed.Create(new EscapedUri(feedUri), settings);
            IListDataResultConverter converter = null;
            switch (feed.Format)
            {
                case SyndicationContentFormat.Rss:
                    {
                        var rssFeed = feed.Resource as RssFeed;
                        if (rssFeed != null)
                        {
                            converter = new RssFeedConverter(rssFeed, datetimeFormatter);
                            feedTitle = rssFeed.Channel.Title;
                        }
                    }

                    break;
                case SyndicationContentFormat.Atom:
                    {
                        var atomFeed = feed.Resource as AtomFeed;
                        if (atomFeed != null)
                        {
                            converter = new AtomFeedConverter(atomFeed, datetimeFormatter);
                            feedTitle = atomFeed.Title.ToString();
                        }
                    }

                    break;
            }

            return converter != null ? (HeadlineListDataResult)converter.Process() : null;
        }

        /// <summary>
        /// Processes the RSS feed URI.
        /// </summary>
        /// <param name="feedUri">The RSS feed URI.</param>
        /// <returns>A HeadlineListDataResult object</returns>
        public HeadlineListDataResult Process(string feedUri)
        {
            var settings = new SyndicationResourceLoadSettings
                               {
                                   Timeout = new TimeSpan(0, 0, 1),
                                   RetrievalLimit = 10,
                               };

            WebProxy webProxy = null;
            if (!Properties.Settings.Default.WebResourcesProxy.IsNullOrEmpty())
            {
                webProxy = new WebProxy(Properties.Settings.Default.WebResourcesProxy);
            }

            // credentials is null
            var feed = GenericSyndicationFeed.Create(new EscapedUri(feedUri), null, webProxy, settings);

            IListDataResultConverter converter = null;
            switch (feed.Format)
            {
                case SyndicationContentFormat.Rss:
                    {
                        var rssFeed = feed.Resource as RssFeed;
                        if (rssFeed != null)
                        {
                            converter = new RssFeedConverter(rssFeed, datetimeFormatter);
                        }
                    }

                    break;
                case SyndicationContentFormat.Atom:
                    {
                        var atomFeed = feed.Resource as AtomFeed;
                        if (atomFeed != null)
                        {
                            converter = new AtomFeedConverter(atomFeed, datetimeFormatter);
                        }
                    }

                    break;
            }

            return converter != null ? (HeadlineListDataResult)converter.Process() : null;
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="generateExternalUrlForHeadlineInfo">The generate external URL.</param>
        /// <returns>A HeadlineListDataResult object</returns>
        public HeadlineListDataResult Process(TriggerDetailResponse response, GenerateExternalUrlForHeadlineInfo generateExternalUrlForHeadlineInfo)
        {
            var converter = new GetTriggerDetailsResultConverter(response, datetimeFormatter);
            return (HeadlineListDataResult)converter.Process(generateExternalUrlForHeadlineInfo);
        }
        
        // De-HTMLize the title
        private static string CleanFeedTitle(string feedTitle)
        {
            return Regex.Replace(feedTitle ?? string.Empty, "<[^>]*>", string.Empty);
        }

        public HeadlineListDataResult Map(IPerformContentSearchResponse source)
        {
            return Process(source);
        }

        public HeadlineListDataResult Map(Factiva.Gateway.Messages.Search.V2_0.PerformContentSearchResponse source)
        {
            return Process(source);
        }

        public HeadlineListDataResult Map(AccessionNumberSearchResponse source)
        {
            return Process(source);
        }

        public object Map(object source)
        {
            if (source is AccessionNumberSearchResponse)
                return Map((AccessionNumberSearchResponse)source);

            if (source is Factiva.Gateway.Messages.Search.V2_0.PerformContentSearchResponse)
                return Map((Factiva.Gateway.Messages.Search.V2_0.PerformContentSearchResponse)source);

            if (source is IPerformContentSearchResponse)
                return Map((IPerformContentSearchResponse) source);

            throw new NotSupportedException();
        }
    }
}