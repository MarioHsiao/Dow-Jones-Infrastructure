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
using System.Text.RegularExpressions;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Tools.ServiceLayer.WebServices;
using DowJones.Utilities.Formatters;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Formatters.Globalization.TimeZone;
using Factiva.Gateway.Messages.PCM.Syndication.V1_0;
using log4net;

namespace DowJones.Tools.Ajax.Converters.HeadlineList
{
    internal class SyndicationHeadlineResponseConverter : AbstractHeadlineListDataResultSetConverter
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(SyndicationHeadlineResponseConverter));
        private readonly GetSyndicationHeadlinesResponse getSyndicationHeadlinesResponse;
        private readonly GetSyndicationHeadlinesRequest getSyndicationHeadlinesRequest;
        // private const string RssToken = "rss";

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeedConverter"/> class.
        /// </summary>
        /// <param name="getSyndicationHeadlinesResponse">The get syndication headlines response.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public SyndicationHeadlineResponseConverter(GetSyndicationHeadlinesResponse getSyndicationHeadlinesResponse, string interfaceLanguage,GetSyndicationHeadlinesRequest getSyndicationHeadlinesRequest)
            : base(interfaceLanguage)
        {
            this.getSyndicationHeadlinesResponse = getSyndicationHeadlinesResponse;
            this.getSyndicationHeadlinesRequest = getSyndicationHeadlinesRequest;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeedConverter"/> class.
        /// </summary>
        /// <param name="getSyndicationHeadlinesResponse">The get syndication headlines response.</param>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public SyndicationHeadlineResponseConverter(GetSyndicationHeadlinesResponse getSyndicationHeadlinesResponse, DateTimeFormatter dateTimeFormatter,GetSyndicationHeadlinesRequest getSyndicationHeadlinesRequest)
            : base(dateTimeFormatter)
        {
            this.getSyndicationHeadlinesResponse = getSyndicationHeadlinesResponse;
            this.getSyndicationHeadlinesRequest = getSyndicationHeadlinesRequest;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeedConverter"/> class.
        /// </summary>
        /// <param name="getSyndicationHeadlinesResponse">The get syndication headlines response.</param>
        public SyndicationHeadlineResponseConverter(GetSyndicationHeadlinesResponse getSyndicationHeadlinesResponse,GetSyndicationHeadlinesRequest getSyndicationHeadlinesRequest)
            : this(getSyndicationHeadlinesResponse, "en",getSyndicationHeadlinesRequest)
        {
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <returns>A Data Result Obj</returns>
        public override IListDataResult Process()
        {
            if (getSyndicationHeadlinesResponse == null)
            {
                return null;
            }

            // Process RSS format specific information
            var result = new PortalHeadlineListDataResult
            {
                ResultSet = 
                {
                    First = new WholeNumber(getSyndicationHeadlinesRequest.FirstResultToReturn)
                }
            };

            var language = GetLanguageFromFeed(getSyndicationHeadlinesResponse);

            var i = 0;
            foreach (var item in getSyndicationHeadlinesResponse.SyndicationItemCollection[0].Channel.ItemCollection)
            {

               result.ResultSet.Headlines.Add(Convert(item, ++i, language));
            }

            result.HitCount = new WholeNumber(result.ResultSet.Headlines.Count);
            result.ResultSet.First = new WholeNumber(getSyndicationHeadlinesRequest.FirstResultToReturn);
            result.ResultSet.Count = new WholeNumber(result.ResultSet.Headlines.Count);
            //result.isTimeInGMT = DateTimeFormatter.CurrentTimeZone == TimeZoneManager.GmtTimeZone;

            return result;
        }

        private static string GetLanguageFromFeed(GetSyndicationHeadlinesResponse getSyndicationHeadlinesResponse)
        {
            if (getSyndicationHeadlinesResponse == null || getSyndicationHeadlinesResponse.SyndicationItemCollection.Count == 0)
            {
                return null;
            }

            return ValidateLanguageName(getSyndicationHeadlinesResponse.SyndicationItemCollection[0].Channel.Language);
        }

        /// <summary>
        /// Converts the specified item.
        /// </summary>
        /// <param name="item">The RSS item.</param>
        /// <param name="index">The index.</param>
        /// <param name="language">The language.</param>
        /// <returns>A HeadlineInfo Object.</returns>
        private PortalHeadlineInfo Convert(Item item, int index, string language)
        {

            var headlineInfo = new PortalHeadlineInfo();
        
            if (item != null)
            {
                //if (item.HasExtensions)
                //{
                //    // Retrieve iTunes syndication extension for channel item using predicate-based search
                //    var itunesItemExtension = item.FindExtension(ITunesSyndicationExtension.MatchByType) as ITunesSyndicationExtension;
                //    if (itunesItemExtension != null && itunesItemExtension.Context != null)
                //    {
                //        headlineInfo.byline = new List<Para> { new Para(new MarkupItem(EntityType.Textual, itunesItemExtension.Context.Author)) };
                //    }

                //    var yahooMediaExtention = item.FindExtension(YahooMediaSyndicationExtension.MatchByType) as YahooMediaSyndicationExtension;
                //    if (yahooMediaExtention != null && yahooMediaExtention.Context != null)
                //    {
                //    }
                //}

                // Add Title
                headlineInfo.Title = StripHTML(item.Title);


                // update the index
               // headlineInfo.index = new WholeNumber(index);
               // NumberFormatter.Format(headlineInfo.index);

                // Add Snippet
                headlineInfo.Snippets = new SnippetCollection(new List<string> { item.Description });
                
                // update publication date
                headlineInfo.PublicationDateTime = DateTimeFormatter.ConvertToUtc(item.PubDate);
                headlineInfo.HasPublicationTime = headlineInfo.PublicationDateTime.TimeOfDay == new TimeSpan();
                headlineInfo.PublicationDateTimeDescriptor = headlineInfo.HasPublicationTime ? DateTimeFormatter.FormatLongDate(headlineInfo.PublicationDateTime) : DateTimeFormatter.FormatLongDateTime(headlineInfo.PublicationDateTime);
                headlineInfo.PublicationDateDescriptor = DateTimeFormatter.FormatLongDate(headlineInfo.PublicationDateTime);
                headlineInfo.PublicationTimeDescriptor = headlineInfo.HasPublicationTime ? DateTimeFormatter.FormatTime(headlineInfo.PublicationDateTime) : string.Empty;

                // update language
                headlineInfo.BaseLanguage = language;
                headlineInfo.BaseLanguageDescriptor = GetLanguageToContentLanguage(language);

                //if (item.Source != null)
                //{
                //    headlineInfo.sourceDescriptor = item.Source.Title;
                //    headlineInfo.sourceReference = new UriBuilder(item.Source.Url.ToString()).ToString();
                //}

                // update category information
               // headlineInfo.ContentCategory = ContentCategory.External;
                headlineInfo.ContentCategoryDescriptor = ContentCategory.External.ToString().ToLower();

                //headlineInfo.contentSubCategory = ContentSubCategory.Rss;
                headlineInfo.ContentSubCategoryDescriptor = ContentSubCategory.Rss.ToString().ToLower();
                headlineInfo.HeadlineUrl = item.Link;
                headlineInfo.Authors = new AuthorCollection(new List<string>{item.Author});
                
                //headlineInfo.Importance = Importance.Normal.ToString();
                //headlineInfo.importanceDescriptor = GetAssignedToken(Importance.Normal);

                //headlineInfo.reference.guid = (item.Guid != null) ? item.Guid.Value : new Utilities.Uri.UrlBuilder(item.Link.ToString()).ToString();
                //headlineInfo.reference.type = RssToken;
                //headlineInfo.reference.externalUri = (item.Guid != null && item.Guid.IsPermanentLink) ? item.Guid.Value : new Utilities.Uri.UrlBuilder(item.Link.ToString()).ToString(); 
            }

            return headlineInfo;
        }

        // De-HTMLize the title
        internal static string StripHTML(string feedTitle)
        {
            var htmlTags = new Regex("<[^>]*>");
            return string.IsNullOrWhiteSpace(feedTitle) ? string.Empty : htmlTags.Replace(feedTitle, string.Empty);
        }
    }
}
