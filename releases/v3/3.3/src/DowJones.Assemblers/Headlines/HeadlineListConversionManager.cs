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
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Trigger.V1_1;
using log4net;
using AccessionNumberSearchResponse = DowJones.Managers.Search.Responses.AccessionNumberSearchResponse;

namespace DowJones.Assemblers.Headlines
{
    /// <summary>
    /// </summary>
    public class HeadlineListConversionManager :
        ITypeMapper<PerformContentSearchResponse, HeadlineListDataResult>,
        ITypeMapper<IPerformContentSearchResponse, HeadlineListDataResult>,
        ITypeMapper<AccessionNumberSearchResponse, HeadlineListDataResult>
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(HeadlineListConversionManager));
        private readonly DateTimeFormatter _datetimeFormatter;
 
        /// <summary>
        /// Initializes a new instance of the <see cref="HeadlineListConversionManager"/> class.
        /// </summary>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public HeadlineListConversionManager(DateTimeFormatter dateTimeFormatter)
        {
            _datetimeFormatter = dateTimeFormatter;
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
            var converter = new PerformContentSearchResponseConverter(response, startIndex, _datetimeFormatter);
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
            var converter = new PerformContentSearchResponseConverter(response, -1, _datetimeFormatter);
            return (HeadlineListDataResult)converter.Process(generateExternalUrlForHeadlineInfo, generateSnippetThumbnailForHeadlineInfo);
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="includeInvalidHeadlines">if set to <c>true</c> [include invalid headlines].</param>
        /// <param name="generateExternalUrlForHeadlineInfo">The generate external URL.</param>
        /// <param name="generateSnippetThumbnailForHeadlineInfo">The update thumbnail.</param>
        /// <returns>
        /// A HeadlineListDataResult object
        /// </returns>
        public HeadlineListDataResult Process(AccessionNumberSearchResponse response, bool includeInvalidHeadlines = false, GenerateExternalUrlForHeadlineInfo generateExternalUrlForHeadlineInfo = null, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo = null)
        {
            var converter = new AccessionNumberSearchResponseConverter(response, _datetimeFormatter)
                                {
                                    IncludeInvalidHeadlines = includeInvalidHeadlines,
                                };
            return (HeadlineListDataResult)converter.Process(generateExternalUrlForHeadlineInfo, generateSnippetThumbnailForHeadlineInfo);
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="includeInvalidHeadlines">if set to <c>true</c> [include invalid headlines].</param>
        /// <param name="generateExternalUrlForHeadlineInfo">The generate external URL.</param>
        /// <param name="generateSnippetThumbnailForHeadlineInfo">The update thumbnail.</param>
        /// <returns>
        /// A HeadlineListDataResult object
        /// </returns>
        public HeadlineListDataResult Process(PCMGetCollectionHeadlinesByCodeResponse response, bool includeInvalidHeadlines = false, GenerateExternalUrlForHeadlineInfo generateExternalUrlForHeadlineInfo = null, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo = null)
        {
            var converter = new PCMAccessionNumberSearchResponseConverter(response, _datetimeFormatter)
                                {
                                    IncludeInvalidHeadlines = includeInvalidHeadlines
                                };

            return (HeadlineListDataResult)converter.Process(generateExternalUrlForHeadlineInfo, generateSnippetThumbnailForHeadlineInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feedUri"></param>
        /// <returns></returns>
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
                            converter = new RssFeedConverter(rssFeed, _datetimeFormatter);
                            feedTitle = rssFeed.Channel.Title;
                        }
                    }

                    break;
                case SyndicationContentFormat.Atom:
                    {
                        var atomFeed = feed.Resource as AtomFeed;
                        if (atomFeed != null)
                        {
                            converter = new AtomFeedConverter(atomFeed, _datetimeFormatter);
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
                            converter = new RssFeedConverter(rssFeed, _datetimeFormatter);
                            feedTitle = rssFeed.Channel.Title;
                        }
                    }

                    break;
                case SyndicationContentFormat.Atom:
                    {
                        var atomFeed = feed.Resource as AtomFeed;
                        if (atomFeed != null)
                        {
                            converter = new AtomFeedConverter(atomFeed, _datetimeFormatter);
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
                            converter = new RssFeedConverter(rssFeed, _datetimeFormatter);
                        }
                    }

                    break;
                case SyndicationContentFormat.Atom:
                    {
                        var atomFeed = feed.Resource as AtomFeed;
                        if (atomFeed != null)
                        {
                            converter = new AtomFeedConverter(atomFeed, _datetimeFormatter);
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
            var converter = new GetTriggerDetailsResultConverter(response, _datetimeFormatter);
            return (HeadlineListDataResult)converter.Process(generateExternalUrlForHeadlineInfo);
        }
        
        // De-HTMLize the title
        private static string CleanFeedTitle(string feedTitle)
        {
            return Regex.Replace(feedTitle ?? string.Empty, "<[^>]*>", string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public HeadlineListDataResult Map(IPerformContentSearchResponse source)
        {
            return Process(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public HeadlineListDataResult Map(PerformContentSearchResponse source)
        {
            return Process(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public HeadlineListDataResult Map(AccessionNumberSearchResponse source)
        {
            return Process(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public object Map(object source)
        {
            var accessionNumberSearchResponse = source as AccessionNumberSearchResponse;
            if (accessionNumberSearchResponse != null)
                return Map(accessionNumberSearchResponse);

            var performContentSearchResponse = source as PerformContentSearchResponse;
            if (performContentSearchResponse != null)
                return Map(performContentSearchResponse);

            var contentSearchResponse = source as IPerformContentSearchResponse;
            if (contentSearchResponse != null)
                return Map(contentSearchResponse);

            throw new NotSupportedException();
        }
    }
}