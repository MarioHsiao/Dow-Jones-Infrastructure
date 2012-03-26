// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformContentSearchResponseConverter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Utilities.Formatters;
using DowJones.Utilities.Formatters.Globalization;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using log4net;

namespace DowJones.Tools.Ajax.Converters.HeadlineList
{
    internal class PerformContentSearchResponseConverter : AbstractHeadlineListDataResultSetConverter, IExtendedListDataResultConverter
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(PerformContentSearchResponseConverter));
        private readonly IPerformContentSearchResponse response;
        private readonly int startIndex;
        private readonly HeadlineListDataResult result = new HeadlineListDataResult();
        private GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public PerformContentSearchResponseConverter(IPerformContentSearchResponse response, int startIndex, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            this.response = response;
            this.startIndex = startIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="startIndex">
        /// The start Index.
        /// </param>
        public PerformContentSearchResponseConverter(IPerformContentSearchResponse response, int startIndex)
            : this(response, startIndex, "en")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public PerformContentSearchResponseConverter(IPerformContentSearchResponse response, int startIndex, DateTimeFormatter dateTimeFormatter)
            : base(dateTimeFormatter)
        {
            this.response = response;
            this.startIndex = startIndex;
        }

        public override IListDataResult Process()
        {
            return Process(null, null);
        }

        public IListDataResult Process(Delegate generateExternalUrl, GenerateSnippetThumbnailForHeadlineInfo genSnippetThumbnailForHeadlineInfo)
        {
            return Process((GenerateExternalUrlForHeadlineInfo)generateExternalUrl, genSnippetThumbnailForHeadlineInfo);
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="generateExternalUrl">The generate external URL.</param>
        /// <param name="generateSnippetThumbnail">The update thumbnail.</param>
        /// <returns> an IListDataResult object</returns>
        public IListDataResult Process(GenerateExternalUrlForHeadlineInfo generateExternalUrl, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnail)
        {
            GenerateExternalUrlForHeadlineInfo = generateExternalUrl;
            generateSnippetThumbnailForHeadlineInfo = generateSnippetThumbnail;

            if (response == null || response.ContentSearchResult == null || response.ContentSearchResult.ContentHeadlineResultSet == null || response.ContentSearchResult.HitCount <= 0)
            {
                // Add the HitCount to the result set
                result.hitCount = new WholeNumber(0);

                result.resultSet.first = result.hitCount;
                result.resultSet.count = result.hitCount;
                result.resultSet.duplicateCount = result.hitCount;
                return result;
            }

            // Add the HitCount to the result set
            result.hitCount = new WholeNumber(response.ContentSearchResult.HitCount);

            var resultSet = response.ContentSearchResult.ContentHeadlineResultSet;
            result.resultSet.first = new WholeNumber(resultSet.First);

            if (resultSet.Count <= 0)
            {
                return result;
            }
            
            if (response.ContentSearchResult.DeduplicatedHeadlineSet != null  && response.ContentSearchResult.DeduplicatedHeadlineSet.Count > 0)
            {
                result.resultSet.count = new WholeNumber(response.ContentSearchResult.DeduplicatedHeadlineSet.Count);
                result.resultSet.duplicateCount = new WholeNumber(resultSet.Count - response.ContentSearchResult.DeduplicatedHeadlineSet.Count);
                
                // Format
                ProcessDeduplicatedHeadlines(resultSet, response.ContentSearchResult.DeduplicatedHeadlineSet);
            }
            else
            {
                result.resultSet.count = new WholeNumber(resultSet.Count);
                result.resultSet.duplicateCount = new WholeNumber(0);
                ProcessContentHeadlines(resultSet);
            }

            return result;
        }

        private void ProcessDeduplicatedHeadlines(ContentHeadlineResultSet contentHeadlineResultSet, DeduplicatedHeadlineSet deduplicatedHeadlineSet)
        {
            var headlineDictionary = contentHeadlineResultSet.ContentHeadlineCollection.ToDictionary(headline => headline.AccessionNo);
            var i = (startIndex > 0) ? startIndex : contentHeadlineResultSet.First;

            foreach (var reference in deduplicatedHeadlineSet.HeadlineRefCollection)
            {
                var curHeadline = headlineDictionary[reference.AccessionNo];
                var curHeadlineInfo = new HeadlineInfo();
                Convert(curHeadlineInfo, curHeadline, ++i, false);
                if (reference.Duplicates.Count > 0)
                {
                    var j = 0;
                    foreach (var headlineRef in reference.Duplicates.DuplicateRefCollection)
                    {
                        var curDedupHeadlineInfo = new DedupHeadlineInfo { ParentAccessionNo = reference.AccessionNo };
                        var tempCurHeadline = headlineDictionary[headlineRef.AccessionNo];
                        Convert(curDedupHeadlineInfo, tempCurHeadline, ++j, true);
                        curHeadlineInfo.duplicateHeadlines.Add(curDedupHeadlineInfo);
                    }
                }

                result.resultSet.headlines.Add(curHeadlineInfo);
            }
        }

        private void ProcessContentHeadlines(ContentHeadlineResultSet contentHeadlineResultSet)
        {
            var i = (startIndex > 0) ? startIndex : contentHeadlineResultSet.First;
            foreach (ContentHeadline headline in contentHeadlineResultSet.ContentHeadlineCollection)
            {
                var headlineInfo = new HeadlineInfo();
                Convert(headlineInfo, headline, ++i, false);
                result.resultSet.headlines.Add(headlineInfo);
            }
        }

        private ThumbnailImage GetThumbnailImage(ContentHeadline contentHeadline)
        {
            if (contentHeadline == null || contentHeadline.ContentItems == null)
            {
                return null;
            }

            if (generateSnippetThumbnailForHeadlineInfo != null)
            {
                var image = new ThumbnailImage();
                generateSnippetThumbnailForHeadlineInfo(image, contentHeadline);
                return image;
            }

            if (contentHeadline.ContentItems.ItemCollection != null && contentHeadline.ContentItems.ItemCollection.Count > 0)
            {
                foreach (var item in contentHeadline.ContentItems.ItemCollection.Where(item => (string.IsNullOrEmpty(item.Mimetype) && item.Type.ToLower() == "tnail" && !string.IsNullOrEmpty(item.Ref))))
                {
                    return new ThumbnailImage { URI = item.Ref };
                }

                return (from item in contentHeadline.ContentItems.ItemCollection.Where(item => (string.IsNullOrEmpty(item.Mimetype) && item.Type.ToLower() == "dispix" && item.Size.ToLower() == "0" && item.Subtype.ToLower() == "primary" && !string.IsNullOrEmpty(item.Ref))) where !string.IsNullOrEmpty(item.Ref) select new ThumbnailImage { GUID = item.Ref }).FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Gets the time in seconds.
        /// </summary>
        /// <param name="contentHeadline">The content headline.</param>
        /// <returns></returns>
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
                    return t.ToString("mm':'ss");
                }

                return null;
            }

            return null;
        }

        /// <summary>
        /// Converts the specified content headline.
        /// </summary>
        /// <param name="headlineInfo">The headline info.</param>
        /// <param name="contentHeadline">The content headline.</param>
        /// <param name="index">The index.</param>
        /// <param name="isDuplicate">if set to <c>true</c> [is duplicate].</param>
        protected internal void Convert(HeadlineInfo headlineInfo, ContentHeadline contentHeadline, int index, bool isDuplicate)
        {
            if (headlineInfo == null || contentHeadline == null)
            {
                return;
            }
            
            // update category information
            headlineInfo.contentCategory = MapContentCategory(contentHeadline);
            headlineInfo.contentCategoryDescriptor = headlineInfo.contentCategory.ToString().ToLower();

            headlineInfo.contentSubCategory = MapContentSubCategory(contentHeadline);
            headlineInfo.contentSubCategoryDescriptor = headlineInfo.contentSubCategory.ToString().ToLower();

            headlineInfo.reference.type = "accessionNo";
            headlineInfo.reference.guid = contentHeadline.AccessionNo;
            headlineInfo.reference.externalUri = UpdateReferenceUrl(headlineInfo.contentCategory, contentHeadline, isDuplicate);
            MapExtraReferenceInformation(headlineInfo, contentHeadline);

            // update language
            headlineInfo.baseLanguage = contentHeadline.BaseLanguage;
            headlineInfo.baseLanguageDescriptor = GetLanguageToContentLanguage(contentHeadline.BaseLanguage);

            // update the index
            headlineInfo.index = new WholeNumber(index);

            // update source information
            headlineInfo.sourceReference = contentHeadline.SourceCode;
            headlineInfo.sourceDescriptor = contentHeadline.SourceName;

            headlineInfo.documentVector = contentHeadline.DocumentVector;

            // update publication date/time information
            headlineInfo.publicationDateTime = contentHeadline.PublicationDate;
            headlineInfo.publicationDateTimeDescriptor = DateTimeFormatter.FormatLongDate(headlineInfo.publicationDateTime);

            headlineInfo.modificationDateTime = DateTimeFormatter.Merge(contentHeadline.ModificationDate, contentHeadline.ModificationTime);
            headlineInfo.modificationDateTimeDescriptor = DateTimeFormatter.FormatLongDateTime(headlineInfo.modificationDateTime);
            headlineInfo.modificationDateDescriptor = DateTimeFormatter.FormatLongDate(headlineInfo.modificationDateTime);
            headlineInfo.modificationTimeDescriptor = DateTimeFormatter.FormatTime(contentHeadline.ModificationTime);

            if (contentHeadline.PublicationTime > DateTime.MinValue)
            {
                headlineInfo.hasPublicationTime = true;
                headlineInfo.publicationDateTime = DateTimeFormatter.Merge(contentHeadline.PublicationDate, contentHeadline.PublicationTime);
                headlineInfo.publicationDateTimeDescriptor = DateTimeFormatter.FormatLongDateTime(headlineInfo.publicationDateTime);
                headlineInfo.publicationDateDescriptor = DateTimeFormatter.FormatLongDate(headlineInfo.publicationDateTime);
                headlineInfo.publicationTimeDescriptor = DateTimeFormatter.FormatTime(contentHeadline.PublicationTime);
            }

            headlineInfo.wordCount = contentHeadline.WordCount;
            headlineInfo.wordCountDescriptor = string.Format(
                "{0} {1}", 
                NumberFormatter.Format(contentHeadline.WordCount, NumberFormatType.Whole), 
                ResourceText.GetString("words"));

            headlineInfo.title = ParseMarkup(contentHeadline.Headline);
            headlineInfo.snippet = ParseMarkup(contentHeadline.Snippet);
            headlineInfo.byline = ParseMarkup(contentHeadline.Byline);
            headlineInfo.credit = ParseMarkup(contentHeadline.Credit);
            headlineInfo.copyright = ParseMarkup(contentHeadline.Copyright);
            headlineInfo.sectionName = ParseMarkup(contentHeadline.SectionName);
            headlineInfo.truncationRules = GetTruncationRules(contentHeadline.TruncationRules);
            headlineInfo.thumbnailImage = GetThumbnailImage(contentHeadline);
            headlineInfo.time = GetTimeInSeconds(contentHeadline);
        }
    }
}