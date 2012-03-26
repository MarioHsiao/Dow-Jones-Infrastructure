// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AtomFeedConverter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the AtomFeedConverter type.
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
using DowJones.Utilities.Uri;
using log4net;

namespace DowJones.Tools.Ajax.Converters.HeadlineList
{
    internal class AtomFeedConverter : AbstractHeadlineListDataResultSetConverter
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RssFeedConverter));
        private readonly AtomFeed atomFeed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomFeedConverter"/> class.
        /// </summary>
        /// <param name="atomFeed">The atom feed.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public AtomFeedConverter(AtomFeed atomFeed, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            this.atomFeed = atomFeed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomFeedConverter"/> class.
        /// </summary>
        /// <param name="atomFeed">The atom feed.</param>
        public AtomFeedConverter(AtomFeed atomFeed)
            : this(atomFeed, "en")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomFeedConverter"/> class.
        /// </summary>
        /// <param name="atomFeed">The atom feed.</param>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public AtomFeedConverter(AtomFeed atomFeed, DateTimeFormatter dateTimeFormatter)
            : base(dateTimeFormatter)
        {
            this.atomFeed = atomFeed;
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        /// <returns>A Data Result object.</returns>
        public override IListDataResult Process()
        {
            if (atomFeed == null)
            {
                return null;
            }

            // Process RSS format specific information
            var result = new HeadlineListDataResult();
            var i = 1;
            foreach (var atomEntry in atomFeed.Entries)
            {
                result.resultSet.headlines.Add(Convert(atomEntry, i++));
            }

            result.hitCount = new WholeNumber(i - 1);
            result.resultSet.first = new WholeNumber(1);
            result.resultSet.count = new WholeNumber(i - 1);
            result.isTimeInGMT = DateTimeFormatter.CurrentTimeZone == TimeZoneManager.GmtTimeZone;
            return result;
        }

        /// <summary>
        /// Converts the specified item.
        /// </summary>
        /// <param name="atomEntry">The atom entry.</param>
        /// <param name="index">The index.</param>
        /// <returns>A HeadlineInfo object.</returns>
        private HeadlineInfo Convert(AtomEntry atomEntry, int index)
        {
            var headlineInfo = new HeadlineInfo();
            if (atomEntry != null)
            {
                if (atomEntry.HasExtensions)
                {
                    // Retrieve iTunes syndication extension for channel item using predicate-based search
                    var itunesItemExtension = atomEntry.FindExtension(ITunesSyndicationExtension.MatchByType) as ITunesSyndicationExtension;
                    if (itunesItemExtension != null && itunesItemExtension.Context != null)
                    {
                        headlineInfo.byline = new List<Para> { new Para(new MarkupItem(EntityType.Textual, itunesItemExtension.Context.Author)) };
                    }

                    var yahooMediaExtention = atomEntry.FindExtension(YahooMediaSyndicationExtension.MatchByType) as YahooMediaSyndicationExtension;
                    if (yahooMediaExtention != null && yahooMediaExtention.Context != null)
                    {
                    }
                }

                // Add Title
                headlineInfo.title = new List<Para> { new Para(new MarkupItem(EntityType.Textual, atomEntry.Title.Content)) };

                headlineInfo.index = new WholeNumber(index);
                NumberFormatter.Format(headlineInfo.index);

                // Add Snippet
                headlineInfo.snippet = new List<Para> { new Para(new MarkupItem(EntityType.Textual, atomEntry.Content.Content)) };

                headlineInfo.publicationDateTime = DateTimeFormatter.ConvertToUtc(atomEntry.PublishedOn);
                headlineInfo.hasPublicationTime = headlineInfo.publicationDateTime.TimeOfDay == new TimeSpan();
                headlineInfo.publicationDateTimeDescriptor = headlineInfo.hasPublicationTime ? DateTimeFormatter.FormatLongDate(headlineInfo.publicationDateTime) : DateTimeFormatter.FormatLongDateTime(headlineInfo.publicationDateTime);
                headlineInfo.publicationDateDescriptor = DateTimeFormatter.FormatLongDate(headlineInfo.publicationDateTime);
                headlineInfo.publicationTimeDescriptor = headlineInfo.hasPublicationTime ? DateTimeFormatter.FormatTime(headlineInfo.publicationDateTime) : string.Empty;

                // update language
                if (atomEntry.Language != null)
                {
                    headlineInfo.baseLanguage = ValidateLanguageName(atomEntry.Language.Name);
                    headlineInfo.baseLanguageDescriptor = GetLanguageToContentLanguage(headlineInfo.baseLanguage);
                }

                if (atomEntry.Source != null)
                {
                    headlineInfo.sourceDescriptor = atomEntry.Source.Title.Content;
                    //// headlineInfo.sourceReference = new UriBuilder(atomEntry.Source.Title.).ToString();
                }
                
                // update category information
                headlineInfo.contentCategory = ContentCategory.External;
                headlineInfo.contentCategoryDescriptor = headlineInfo.contentCategory.ToString().ToLower();

                headlineInfo.contentSubCategory = ContentSubCategory.Atom;
                headlineInfo.contentSubCategoryDescriptor = headlineInfo.contentSubCategory.ToString().ToLower();

                headlineInfo.importance = Importance.Normal.ToString();
                headlineInfo.importanceDescriptor = GetAssignedToken(Importance.Normal);

                headlineInfo.reference.guid = atomEntry.Id.ToString();
                headlineInfo.reference.type = "atom";
                headlineInfo.reference.externalUri = new UrlBuilder(atomEntry.Links[0].Uri.ToString()).ToString();
            }

            return headlineInfo;
        }
    }
}