// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessionNumberSearchResponseConverter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.Linq;
using DowJones.Ajax;
using DowJones.Ajax.HeadlineList;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Formatters.Globalization.TimeZone;
using DowJones.Managers.Search.Responses;
using Factiva.Gateway.Messages.Search.V2_0;
using log4net;

namespace DowJones.Assemblers.Headlines
{
    /// <summary>
    /// The accession number search response converter.
    /// </summary>
    internal class AccessionNumberSearchResponseConverter : AbstractHeadlineListDataResultSetConverter, IExtendedListDataResultConverter
    {
        /// <summary>
        /// The logger.
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(AccessionNumberSearchResponseConverter));

        /// <summary>
        /// The _response.
        /// </summary>
        private readonly AccessionNumberSearchResponse response;

        /// <summary>
        /// The _result.
        /// </summary>
        private readonly HeadlineListDataResult result = new HeadlineListDataResult();

        /// <summary>
        /// The _generate external Uri for headline info.
        /// </summary>
        private GenerateExternalUrlForHeadlineInfo generateExternalUrlForHeadlineInfo;

        /// <summary>
        /// The _generate snippet thumbnail for headline info.
        /// </summary>
        private GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessionNumberSearchResponseConverter"/> class. 
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="interfaceLanguage">
        /// The interface language.
        /// </param>
        public AccessionNumberSearchResponseConverter(AccessionNumberSearchResponse response, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            this.response = response;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessionNumberSearchResponseConverter"/> class. 
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        public AccessionNumberSearchResponseConverter(AccessionNumberSearchResponse response)
            : this(response, "en")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessionNumberSearchResponseConverter"/> class. 
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="dateTimeFormatter">
        /// The date time formatter.
        /// </param>
        public AccessionNumberSearchResponseConverter(AccessionNumberSearchResponse response, DateTimeFormatter dateTimeFormatter)
            : base(dateTimeFormatter)
        {
            this.response = response;
        }

        #region IExtendedListDataResultConverter Members

        /// <summary>
        /// The process.
        /// </summary>
        /// <param name="generateExternalUrl">
        /// The generate external url.
        /// </param>
        /// <param name="generateSnippetThumbnailForHeadlineInfo">
        /// The generate snippet thumbnail for headline info.
        /// </param>
        /// <returns>
        /// </returns>
        public IListDataResult Process(Delegate generateExternalUrl, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo)
        {
            return Process((GenerateExternalUrlForHeadlineInfo)generateExternalUrl, generateSnippetThumbnailForHeadlineInfo);
        }

        #endregion

        /// <summary>
        /// The process.
        /// </summary>
        /// <returns>
        /// </returns>
        public override IListDataResult Process()
        {
            return Process(null, null);
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="generateExternalUrl">
        /// The generate external URL.
        /// </param>
        /// <param name="generateSnippetThumbnail">
        /// The update thumbnail.
        /// </param>
        /// <returns>
        /// </returns>
        public IListDataResult Process(GenerateExternalUrlForHeadlineInfo generateExternalUrl, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnail)
        {
            generateExternalUrlForHeadlineInfo = generateExternalUrl;
            generateSnippetThumbnailForHeadlineInfo = generateSnippetThumbnail;

            if (response == null || response.AccessionNumberBasedContentItemSet == null || response.AccessionNumberBasedContentItemSet.Count <= 0)
                return result;

            // Add the HitCount to the result set
            result.hitCount = new WholeNumber(response.AccessionNumberBasedContentItemSet.Count);

// Format
            NumberFormatter.Format(result.hitCount);

            if (response.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection == null)
                return result;

            result.resultSet.first = new WholeNumber(0);

            if (response.AccessionNumberBasedContentItemSet.Count <= 0)
                return result;

            result.resultSet.count = new WholeNumber(response.AccessionNumberBasedContentItemSet.Count);
            result.resultSet.duplicateCount = result.resultSet.count;
            ProcessContentHeadlines(response.AccessionNumberBasedContentItemSet);

            NumberFormatter.Format(result.resultSet.first);
            NumberFormatter.Format(result.resultSet.count);
            NumberFormatter.Format(result.resultSet.duplicateCount);

            result.isTimeInGMT = DateTimeFormatter.CurrentTimeZone == TimeZoneManager.GmtTimeZone;
            return result;
        }


        /// <summary>
        /// The process content headlines.
        /// </summary>
        /// <param name="contentHeadlineResultSet">
        /// The content headline result set.
        /// </param>
        private void ProcessContentHeadlines(AccessionNumberBasedContentItemSet contentHeadlineResultSet)
        {
            int i = 0;
            foreach (AccessionNumberBasedContentItem headline in contentHeadlineResultSet.AccessionNumberBasedContentItemCollection)
            {
                if (!headline.HasBeenFound)
                {
                    continue;
                }

                var headlineInfo = new HeadlineInfo();
                Convert(headlineInfo, headline.ContentHeadline, ++i, false);
                result.resultSet.headlines.Add(headlineInfo);
            }
        }

        /// <summary>
        /// The get thumbnail image.
        /// </summary>
        /// <param name="contentHeadline">
        /// The content headline.
        /// </param>
        /// <returns>
        /// </returns>
        private ThumbnailImage GetThumbnailImage(ContentHeadline contentHeadline)
        {
            if (contentHeadline == null || contentHeadline.ContentItems == null)
            {
                return null;
            }

            foreach (var item in contentHeadline.ContentItems.ItemCollection)
            {
                if (!string.IsNullOrEmpty(item.Mimetype) || item.Type.ToLower() != "dispix" || item.Size.ToLower() != "0" || item.Subtype.ToLower() != "primary")
                {
                    continue;
                }

                var image = new ThumbnailImage
                                {
                                    GUID = item.Ref
                                };
                if (generateSnippetThumbnailForHeadlineInfo != null)
                {
                    generateSnippetThumbnailForHeadlineInfo(image, contentHeadline);
                }

                return image;
            }

            return null;
        }

        /// <summary>
        /// The get time in seconds.
        /// </summary>
        /// <param name="contentHeadline">The content headline.</param>
        /// <returns>
        /// The get time in seconds.
        /// </returns>
        private static string GetTimeInSeconds(ContentHeadline contentHeadline)
        {
            if (contentHeadline == null || contentHeadline.ContentItems == null)
            {
                return null;
            }

            foreach (var item in contentHeadline.ContentItems.ItemCollection.Where(item => item.Type == "audio" || item.Type == "video"))
            {
                int seconds;
                if (Int32.TryParse(item.Size, out seconds))
                {
                    var t = new TimeSpan(0, 0, seconds);
                    return string.Concat(t.Minutes, ":", t.Seconds);
                }

                return null;
            }

            return null;
        }

        /// <summary>
        /// Converts the specified content headline.
        /// </summary>
        /// <param name="headlineInfo">
        /// The headline info.
        /// </param>
        /// <param name="contentHeadline">
        /// The content headline.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="isDuplicate">
        /// if set to <c>true</c> [is duplicate].
        /// </param>
        internal void Convert(HeadlineInfo headlineInfo, ContentHeadline contentHeadline, int index, bool isDuplicate)
        {
            if (headlineInfo == null || contentHeadline == null)
                return;

            // update category information
            headlineInfo.contentSubCategory = headlineInfo.reference.contentSubCategory = MapContentSubCategory(contentHeadline);
            headlineInfo.contentSubCategoryDescriptor = headlineInfo.reference.contentSubCategoryDescriptor = headlineInfo.contentSubCategory.ToString().ToLower();

            headlineInfo.contentSubCategory = MapContentSubCategory(contentHeadline);
            headlineInfo.contentSubCategoryDescriptor = headlineInfo.contentSubCategory.ToString().ToLower();

            headlineInfo.reference.type = "accessionNo";
            headlineInfo.reference.guid = contentHeadline.AccessionNo;
            headlineInfo.reference.originalContentCategory = GetOriginalContentType(contentHeadline);
            headlineInfo.reference.externalUri = UpdateReferenceUrl(headlineInfo.contentCategory, contentHeadline, isDuplicate);
            MapExtraReferenceInformation(headlineInfo, contentHeadline);


            // update language
            headlineInfo.baseLanguage = contentHeadline.BaseLanguage;
            headlineInfo.baseLanguageDescriptor = GetLanguageToContentLanguage(contentHeadline.BaseLanguage);

            // update the index
            headlineInfo.index = new WholeNumber(index);
            NumberFormatter.Format(headlineInfo.index);

            // update source information
            headlineInfo.sourceReference = contentHeadline.SourceCode;
            headlineInfo.sourceDescriptor = contentHeadline.SourceName;

            headlineInfo.documentVector = contentHeadline.DocumentVector;

            // update publication date/time information
            headlineInfo.publicationDateTime = contentHeadline.PublicationDate;
            headlineInfo.publicationDateTimeDescriptor = DateTimeFormatter.FormatLongDate(headlineInfo.publicationDateTime);

            headlineInfo.modificationDateTime = DateTimeFormatter.ConvertToUtc(contentHeadline.ModificationDate);
            headlineInfo.modificationDateTimeDescriptor = DateTimeFormatter.FormatLongDate(contentHeadline.ModificationDate);
            headlineInfo.modificationDateDescriptor = DateTimeFormatter.FormatLongDate(headlineInfo.publicationDateTime);
            headlineInfo.modificationTimeDescriptor = DateTimeFormatter.FormatTime(contentHeadline.PublicationTime);

            if (contentHeadline.PublicationTime > DateTime.MinValue)
            {
                headlineInfo.hasPublicationTime = true;
                headlineInfo.publicationDateTime = DateTimeFormatter.Merge(contentHeadline.PublicationDate, contentHeadline.PublicationTime);
                headlineInfo.publicationDateTimeDescriptor = DateTimeFormatter.FormatLongDateTime(headlineInfo.publicationDateTime);
                headlineInfo.publicationDateDescriptor = DateTimeFormatter.FormatLongDate(headlineInfo.publicationDateTime);
                headlineInfo.publicationTimeDescriptor = DateTimeFormatter.FormatTime(contentHeadline.PublicationTime);
            }

            headlineInfo.wordCount = contentHeadline.WordCount;
            headlineInfo.wordCountDescriptor = string.Format("{0} {1}", 
                                                             NumberFormatter.Format(contentHeadline.WordCount, NumberFormatType.Whole), 
                                                             ResourceText.GetString("words"));

            headlineInfo.title = ParseMarkup(contentHeadline.Headline);
            headlineInfo.snippet = ParseMarkup(contentHeadline.Snippet);
            headlineInfo.byline = ParseMarkup(contentHeadline.Byline);
            headlineInfo.codedAuthors = ReplaceMarkup(contentHeadline.Byline, contentHeadline.CodeSets);
            headlineInfo.credit = ParseMarkup(contentHeadline.Credit);
            headlineInfo.copyright = ParseMarkup(contentHeadline.Copyright);
            headlineInfo.sectionName = ParseMarkup(contentHeadline.SectionName);
            headlineInfo.truncationRules = GetTruncationRules(contentHeadline.TruncationRules);
            headlineInfo.thumbnailImage = GetThumbnailImage(contentHeadline);
            headlineInfo.time = GetTimeInSeconds(contentHeadline);
        }
    }
}