using System;
using System.Collections.Generic;
using Argotic.Extensions.Core;
using Argotic.Syndication;
using EMG.Tools.Ajax.HeadlineList;
using EMG.Utility.Formatters;
using EMG.Utility.Formatters.Globalization;
using EMG.Utility.Uri;
using log4net;

namespace EMG.Tools.Ajax.Converters.HeadlineList
{
    internal class AtomFeedConverter : AbstractHeadlineListDataResultSetConverter
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof (RssFeedConverter));
        private readonly AtomFeed _atomFeed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomFeedConverter"/> class.
        /// </summary>
        /// <param name="atomFeed">The atom feed.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public AtomFeedConverter(AtomFeed atomFeed, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            _atomFeed = atomFeed;
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
        /// Converts the specified item.
        /// </summary>
        /// <param name="atomEntry">The atom entry.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private HeadlineInfo Convert(AtomEntry atomEntry, int index)
        {
            HeadlineInfo headlineInfo = new HeadlineInfo();
            if (atomEntry != null)
            {
                if (atomEntry.HasExtensions)
                {
                    //  Retrieve iTunes syndication extension for channel item using predicate-based search
                    ITunesSyndicationExtension itunesItemExtension = atomEntry.FindExtension(ITunesSyndicationExtension.MatchByType) as ITunesSyndicationExtension;
                    if (itunesItemExtension != null && itunesItemExtension.Context != null)
                    {
                        headlineInfo.byline = new List<Para>();
                        headlineInfo.byline.Add(new Para(new MarkupItem(EntityType.Textual, itunesItemExtension.Context.Author)));
                    }

                    YahooMediaSyndicationExtension yahooMediaExtention = atomEntry.FindExtension(YahooMediaSyndicationExtension.MatchByType) as YahooMediaSyndicationExtension;
                    if (yahooMediaExtention != null && yahooMediaExtention.Context != null)
                    {
                    }
                }
                // Add Title
                headlineInfo.title = new List<Para>();
                headlineInfo.title.Add(new Para(new MarkupItem(EntityType.Textual, atomEntry.Title.Content)));

                headlineInfo.index = new WholeNumber(index);
                NumberFormatter.Format(headlineInfo.index);

                // Add Snippet
                headlineInfo.snippet = new List<Para>();
                headlineInfo.snippet.Add(new Para(new MarkupItem(EntityType.Textual, atomEntry.Content.Content)));

                headlineInfo.publicationDateTime = DateTimeFormatter.ConvertToUtc(atomEntry.PublishedOn);

                headlineInfo.publicationDateTimeDescriptor = headlineInfo.publicationDateTime.TimeOfDay == new TimeSpan() ? DateTimeFormatter.FormatLongDate(headlineInfo.publicationDateTime) : DateTimeFormatter.FormatLongDateTime(headlineInfo.publicationDateTime);

                // update language
                if (atomEntry.Language != null)
                {
                    headlineInfo.baseLanguage = ValidateLanguageName(atomEntry.Language.Name);
                    headlineInfo.baseLanaguageDescriptor = GetLanguageToContentLanguage(headlineInfo.baseLanguage);
                }


                if (atomEntry.Source != null)
                {
                    headlineInfo.sourceDescriptor = atomEntry.Source.Title.Content;
                    //headlineInfo.sourceReference = new UriBuilder(atomEntry.Source.Title.).ToString();
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

        public override IListDataResult Process()
        {
            if (_atomFeed == null)
            {
                return null;
            } 
            
            //  Process RSS format specific information
            HeadlineListDataResult result = new HeadlineListDataResult();
            int i = 1;
            foreach (AtomEntry atomEntry in _atomFeed.Entries)
            {
                result.resultSet.headlines.Add(Convert(atomEntry, i++));
            }
            result.hitCount = new WholeNumber(i-1);
            result.resultSet.first = new WholeNumber(1);
            result.resultSet.count = new WholeNumber(i-1);
            NumberFormatter.Format(result.hitCount);
            NumberFormatter.Format(result.resultSet.first);
            NumberFormatter.Format(result.resultSet.count);
            return result;
        }
    }
}