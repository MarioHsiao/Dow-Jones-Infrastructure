// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RssFeedConverter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the RssFeedConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Argotic.Extensions.Core;
using Argotic.Syndication;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Utilities.Formatters;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Formatters.Globalization.TimeZone;
using log4net;

namespace DowJones.Tools.Ajax.Converters.HeadlineList
{
    internal class RssFeedConverter : AbstractHeadlineListDataResultSetConverter
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RssFeedConverter));
        private readonly RssFeed rssFeed;
        private const string RssToken = "rss";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeedConverter"/> class.
        /// </summary>
        /// <param name="rssFeed">The RSS feed.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public RssFeedConverter(RssFeed rssFeed, string interfaceLanguage) : base(interfaceLanguage)
        {
            this.rssFeed = rssFeed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeedConverter"/> class.
        /// </summary>
        /// <param name="rssFeed">The RSS feed.</param>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public RssFeedConverter(RssFeed rssFeed, DateTimeFormatter dateTimeFormatter)
            : base(dateTimeFormatter)
        {
            this.rssFeed = rssFeed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeedConverter"/> class.
        /// </summary>
        /// <param name="rssFeed">The RSS feed.</param>
        public RssFeedConverter(RssFeed rssFeed) : this(rssFeed, "en")
        {
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <returns>A Data Result Obj</returns>
        public override IListDataResult Process()
        {
            if (rssFeed == null)
            {
                return null;
            }

            // Process RSS format specific information
            var result = new HeadlineListDataResult
                             {
                                 resultSet =
                                     {
                                         first = new WholeNumber(0)
                                     }
                             };

            var language = GetLanguageFromFeed(rssFeed);

            var i = 0;
            foreach (var item in rssFeed.Channel.Items)
            {
                result.resultSet.headlines.Add(Convert(item, ++i, language));
            }

            result.hitCount = new WholeNumber(i - 1);
            result.resultSet.first = new WholeNumber(0);
            result.resultSet.count = new WholeNumber(i - 1);
            result.isTimeInGMT = DateTimeFormatter.CurrentTimeZone == TimeZoneManager.GmtTimeZone;
            
            return result;
        }

        private static string GetLanguageFromFeed(RssFeed rssFeed)
        {
            if (rssFeed == null || rssFeed.Channel == null || rssFeed.Channel.Language == null)
            {
                return null;
            }

            return ValidateLanguageName(rssFeed.Channel.Language.Name);
        }

        /// <summary>
        /// Converts the specified item.
        /// </summary>
        /// <param name="item">The RSS item.</param>
        /// <param name="index">The index.</param>
        /// <param name="language">The language.</param>
        /// <returns>A HeadlineInfo Object.</returns>
        private HeadlineInfo Convert(RssItem item, int index, string language)
        {
            var headlineInfo = new HeadlineInfo();
            if (item != null)
            {
                if (item.HasExtensions)
                {
                    // Retrieve iTunes syndication extension for channel item using predicate-based search
                    var itunesItemExtension = item.FindExtension(ITunesSyndicationExtension.MatchByType) as ITunesSyndicationExtension;
                    if (itunesItemExtension != null && itunesItemExtension.Context != null)
                    {
                        headlineInfo.byline = new List<Para> { new Para(new MarkupItem(EntityType.Textual, itunesItemExtension.Context.Author)) };
                    }

                    var yahooMediaExtention = item.FindExtension(YahooMediaSyndicationExtension.MatchByType) as YahooMediaSyndicationExtension;
                    if (yahooMediaExtention != null && yahooMediaExtention.Context != null)
                    {
                    }
                }

                // Add Title
                headlineInfo.title = new List<Para> { new Para(new MarkupItem(EntityType.Textual, item.Title)) };

                // update the index
                headlineInfo.index = new WholeNumber(index);
                NumberFormatter.Format(headlineInfo.index);

                // Add Snippet
                headlineInfo.snippet = new List<Para> { new Para(new MarkupItem(EntityType.Textual, item.Description)) };

                // update publication date
                headlineInfo.publicationDateTime = DateTimeFormatter.ConvertToUtc(item.PublicationDate);
                headlineInfo.hasPublicationTime = headlineInfo.publicationDateTime.TimeOfDay == new TimeSpan();
                headlineInfo.publicationDateTimeDescriptor = headlineInfo.hasPublicationTime ? DateTimeFormatter.FormatLongDate(headlineInfo.publicationDateTime) : DateTimeFormatter.FormatLongDateTime(headlineInfo.publicationDateTime);
                headlineInfo.publicationDateDescriptor = DateTimeFormatter.FormatLongDate(headlineInfo.publicationDateTime);
                headlineInfo.publicationTimeDescriptor = headlineInfo.hasPublicationTime ? DateTimeFormatter.FormatTime(headlineInfo.publicationDateTime) : string.Empty;

                // update language
                headlineInfo.baseLanguage = language;
                headlineInfo.baseLanguageDescriptor = GetLanguageToContentLanguage(language);

                if (item.Source != null)
                {
                    headlineInfo.sourceDescriptor = item.Source.Title;
                    headlineInfo.sourceReference = new UriBuilder(item.Source.Url.ToString()).ToString();
                }

                // update category information
                headlineInfo.contentCategory = ContentCategory.External;
                headlineInfo.contentCategoryDescriptor = headlineInfo.contentCategory.ToString().ToLower();

                headlineInfo.contentSubCategory = ContentSubCategory.Rss;
                headlineInfo.contentSubCategoryDescriptor = headlineInfo.contentSubCategory.ToString().ToLower();

                headlineInfo.importance = Importance.Normal.ToString();
                headlineInfo.importanceDescriptor = GetAssignedToken(Importance.Normal);

                headlineInfo.reference.guid = (item.Guid != null) ? item.Guid.Value : new Utilities.Uri.UrlBuilder(item.Link.ToString()).ToString();
                headlineInfo.reference.type = RssToken;
                headlineInfo.reference.externalUri = (item.Guid != null && item.Guid.IsPermanentLink) ? item.Guid.Value : new Utilities.Uri.UrlBuilder(item.Link.ToString()).ToString(); 
            }

            return headlineInfo;
        }
    }
}
