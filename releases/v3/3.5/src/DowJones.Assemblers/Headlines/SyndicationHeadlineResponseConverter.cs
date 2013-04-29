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
using DowJones.Extensions;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using Factiva.Gateway.Messages.PCM.Syndication.V1_0;
using log4net;

namespace DowJones.Assemblers.Headlines
{
    public class SyndicationHeadlineResponseConverter : AbstractHeadlineListDataResultSetConverter
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(SyndicationHeadlineResponseConverter));
        private readonly GetSyndicationHeadlinesResponse _getSyndicationHeadlinesResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeedConverter"/> class.
        /// </summary>
        /// <param name="getSyndicationHeadlinesResponse">The get syndication headlines response.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public SyndicationHeadlineResponseConverter(GetSyndicationHeadlinesResponse getSyndicationHeadlinesResponse, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            _getSyndicationHeadlinesResponse = getSyndicationHeadlinesResponse;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeedConverter"/> class.
        /// </summary>
        /// <param name="getSyndicationHeadlinesResponse">The get syndication headlines response.</param>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public SyndicationHeadlineResponseConverter(GetSyndicationHeadlinesResponse getSyndicationHeadlinesResponse, DateTimeFormatter dateTimeFormatter)
            : base(dateTimeFormatter)
        {
            _getSyndicationHeadlinesResponse = getSyndicationHeadlinesResponse;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeedConverter"/> class.
        /// </summary>
        /// <param name="getSyndicationHeadlinesResponse">The get syndication headlines response.</param>
        public SyndicationHeadlineResponseConverter(GetSyndicationHeadlinesResponse getSyndicationHeadlinesResponse)
            : this(getSyndicationHeadlinesResponse, "en")
        {
        }

        public override IListDataResult Process()
        {
            return Process(0);
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <returns>A Data Result Obj</returns>
        public IListDataResult Process(int index)
        {
            if (_getSyndicationHeadlinesResponse == null || 
                _getSyndicationHeadlinesResponse.SyndicationItemCollection == null ||
                _getSyndicationHeadlinesResponse.SyndicationItemCollection.Count == 0 ||
                _getSyndicationHeadlinesResponse.SyndicationItemCollection[0].Channel == null ||
                _getSyndicationHeadlinesResponse.SyndicationItemCollection[0].Channel.ItemCollection == null ||
                _getSyndicationHeadlinesResponse.SyndicationItemCollection[0].Channel.ItemCollection.Count == 0)
            {
                return null;
            }

            var syndicationCollection = _getSyndicationHeadlinesResponse.SyndicationItemCollection[0];

            // Process RSS format specific information
            var result = new PortalHeadlineListDataResult();
            var language = GetLanguageFromFeed(_getSyndicationHeadlinesResponse);

            foreach (var item in syndicationCollection.Channel.ItemCollection)
            {
               result.ResultSet.Headlines.Add(Convert(item, ++index, language));
            }

            result.HitCount = new WholeNumber(syndicationCollection.TotalRecords);
            result.ResultSet.First = new WholeNumber(syndicationCollection.FirstResult <= 0 ? 0 : syndicationCollection.FirstResult-1);
            result.ResultSet.Count = new WholeNumber(syndicationCollection.Channel.ItemCollection.Count);

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
                if (item.Author.IsNotEmpty())
                {
                    headlineInfo.Authors = new AuthorCollection(new List<string> {item.Author});
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
