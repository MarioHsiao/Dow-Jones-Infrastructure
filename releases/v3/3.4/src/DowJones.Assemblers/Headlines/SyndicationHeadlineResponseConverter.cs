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
using DowJones.Ajax;
using DowJones.Ajax.HeadlineList;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using Factiva.Gateway.Messages.PCM.Syndication.V1_0;
using log4net;

namespace DowJones.Assemblers.Headlines
{
    public class SyndicationHeadlineResponseConverter : AbstractHeadlineListDataResultSetConverter
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(SyndicationHeadlineResponseConverter));
        private readonly GetSyndicationHeadlinesResponse getSyndicationHeadlinesResponse;
        private readonly GetSyndicationHeadlinesRequest getSyndicationHeadlinesRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeedConverter"/> class.
        /// </summary>
        /// <param name="getSyndicationHeadlinesResponse">The get syndication headlines response.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="getSyndicationHeadlinesRequest">The get syndication headlines request.</param>
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
        /// <param name="getSyndicationHeadlinesRequest">The get syndication headlines request.</param>
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
        /// <param name="getSyndicationHeadlinesRequest">The get syndication headlines request.</param>
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
                // add the Index
                headlineInfo.Index = new WholeNumber(index);

                // Add Title
                headlineInfo.Title = StripHTML(item.Title);

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

                // update category information
                headlineInfo.ContentCategoryDescriptor = ContentCategory.External.ToString().ToLower();
                headlineInfo.ContentSubCategoryDescriptor = ContentSubCategory.Rss.ToString().ToLower();
                headlineInfo.HeadlineUrl = item.Link;
                new AuthorCollection(new List<string>{item.Author});
                headlineInfo.CodedAuthors = new List<Para>
                                           {
                                               new Para
                                                   {
                                                       items = new List<MarkupItem>
                                                                   {
                                                                       new MarkupItem
                                                                           {
                                                                               EntityType = EntityType.Textual,
                                                                               value = item.Author
                                                                           }
                                                                   }
                                                   }
                                           };
            }

            return headlineInfo;
        }

        // De-HTMLize the title
        public static string StripHTML(string feedTitle)
        {
            var htmlTags = new Regex("<[^>]*>");
            return string.IsNullOrWhiteSpace(feedTitle) ? string.Empty : htmlTags.Replace(feedTitle, string.Empty);
        }
    }
}
