using System;
using System.Collections.Generic;
using EMG.Tools.Ajax.HeadlineList;
using EMG.Utility.Formatters;
using EMG.Utility.Formatters.Globalization;
using Factiva.Gateway.Messages.Search.V2_0;
using log4net;

namespace EMG.Tools.Ajax.Converters.HeadlineList
{
   

    internal class PerformContentSearchResponseConverter : AbstractHeadlineListDataResultSetConverter, IExtendedListDataResultConverter
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof (PerformContentSearchResponseConverter));
        private readonly PerformContentSearchResponse _response;
        private readonly HeadlineListDataResult _result = new HeadlineListDataResult();
        private GenerateExternalUrl _generateExternalUrl;
        private GenerateSnippetThumbnail _generateSnippetThumbnail;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public PerformContentSearchResponseConverter(PerformContentSearchResponse response, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            _response = response;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        public PerformContentSearchResponseConverter(PerformContentSearchResponse response)
            : this(response, "en")
        {
        }

        public override IListDataResult Process()
        {
            return Process(null, null);
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="generateExternalUrl">The generate external URL.</param>
        /// <param name="generateSnippetThumbnail">The update thumbnail.</param>
        /// <returns></returns>
        public IListDataResult Process(GenerateExternalUrl generateExternalUrl, GenerateSnippetThumbnail generateSnippetThumbnail)
        {
            _generateExternalUrl = generateExternalUrl;
            _generateSnippetThumbnail = generateSnippetThumbnail;

            if (_response == null || _response.ContentSearchResult == null || _response.ContentSearchResult.ContentHeadlineResultSet == null || _response.ContentSearchResult.HitCount <= 0)
                return _result;

            // Add the HitCount to the result set
            _result.hitCount = new WholeNumber(_response.ContentSearchResult.HitCount);
            // Format
            NumberFormatter.Format(_result.hitCount);

            if (_response.ContentSearchResult.ContentHeadlineResultSet == null)
                return _result;
            ContentHeadlineResultSet resultSet = _response.ContentSearchResult.ContentHeadlineResultSet;
            _result.resultSet.first = new WholeNumber(resultSet.First);
            _result.resultSet.count = new WholeNumber(resultSet.Count);

            // Format
            NumberFormatter.Format(_result.resultSet.first);
            NumberFormatter.Format(_result.resultSet.count);

            if (resultSet.Count <= 0)
                return _result;

            if (_response.ContentSearchResult.DeduplicatedHeadlineSet != null  &&
                _response.ContentSearchResult.DeduplicatedHeadlineSet.Count > 0)
            {
                ProcessDeduplicatedHeadlines(resultSet, _response.ContentSearchResult.DeduplicatedHeadlineSet);
            }
            else
            {
                ProcessContentHeadlines(resultSet);
            }
            return _result;
        }


        private void ProcessDeduplicatedHeadlines(ContentHeadlineResultSet contentHeadlineResultSet, DeduplicatedHeadlineSet deduplicatedHeadlineSet)
        {
            Dictionary<string, ContentHeadline> headlineDictionary = new Dictionary<string, ContentHeadline>();

            // generate a dictionary obj
            foreach (ContentHeadline headline in contentHeadlineResultSet.ContentHeadlineCollection)
            {
                headlineDictionary.Add(headline.AccessionNo, headline);
            }

            int i = contentHeadlineResultSet.First;
            foreach (DeduplicatedHeadlineRef reference in deduplicatedHeadlineSet.HeadlineRefCollection)
            {
                ContentHeadline curHeadline = headlineDictionary[reference.AccessionNo];
                HeadlineInfo curHeadlineInfo = new HeadlineInfo();
                Convert(curHeadlineInfo, curHeadline, ++i, false);
                if (reference.Duplicates.Count > 0)
                {
                    int j = 0;
                    foreach (HeadlineRef headlineRef in reference.Duplicates.DuplicateRefCollection)
                    {
                        DedupHeadlineInfo curDedupHeadlineInfo = new DedupHeadlineInfo();
                        curDedupHeadlineInfo.ParentAccessionNo = reference.AccessionNo;
                        ContentHeadline dCurHeadline = headlineDictionary[headlineRef.AccessionNo];
                        Convert(curDedupHeadlineInfo, dCurHeadline, ++j, true);
                        curHeadlineInfo.duplicateHeadlines.Add(curDedupHeadlineInfo);
                    }
                }
                _result.resultSet.headlines.Add(curHeadlineInfo);
            }
        }

        private void ProcessContentHeadlines(ContentHeadlineResultSet contentHeadlineResultSet)
        {
            int i = contentHeadlineResultSet.First;
            foreach (ContentHeadline headline in contentHeadlineResultSet.ContentHeadlineCollection)
            {
                HeadlineInfo headlineInfo = new HeadlineInfo();
                Convert(headlineInfo, headline, i++, false);
                _result.resultSet.headlines.Add(headlineInfo);
            }
        }

        private ThumbnailImage GetThumbnailImage(ContentHeadline contentHeadline)
        {
            if (contentHeadline == null || contentHeadline.ContentItems == null)
                return null;
            foreach (ContentItem item in contentHeadline.ContentItems.ItemCollection)
            {
                if (!string.IsNullOrEmpty(item.Mimetype) || item.Type.ToLower() != "dispix" || item.Size.ToLower() != "0" || item.Subtype.ToLower() != "primary")
                    continue;
                ThumbnailImage image = new ThumbnailImage();
                image.guid = item.Ref;
                if (_generateSnippetThumbnail != null)
                {
                    _generateSnippetThumbnail(image, contentHeadline);
                }
                return image;
            }
            return null;
        }

        private string UpdateReferenceUrl(ContentCategory contentCategory, ContentHeadline contentHeadline, bool isDuplicate)
        {
            if (contentHeadline == null || contentHeadline.ContentItems == null)
                return null;

            // look through based on type and provide the correct url
            switch (contentCategory)
            {
                case ContentCategory.Blog:
                case ContentCategory.Board:
                case ContentCategory.CustomerDoc:
                case ContentCategory.Internal:
                    foreach (ContentItem item in contentHeadline.ContentItems.ItemCollection)
                    {
                        if (item.Type.ToLower() == "webpage")
                        {
                            return item.Ref;
                        }
                    }
                    break;
            }
            return _generateExternalUrl != null ? _generateExternalUrl(contentHeadline, isDuplicate) : null;
        }

        /// <summary>
        /// Converts the specified content headline.
        /// </summary>
        /// <param name="headlineInfo">The headline info.</param>
        /// <param name="contentHeadline">The content headline.</param>
        /// <param name="index">The index.</param>
        /// <param name="isDuplicate">if set to <c>true</c> [is duplicate].</param>
        private void Convert(HeadlineInfo headlineInfo, ContentHeadline contentHeadline, int index, bool isDuplicate)
        {
            if (headlineInfo == null || contentHeadline == null)
                return;
            
            // update category information
            headlineInfo.contentCategory = MapContentCategory(contentHeadline);
            headlineInfo.contentCategoryDescriptor = headlineInfo.contentCategory.ToString().ToLower();

            headlineInfo.contentSubCategory = MapContentSubCategory(contentHeadline);
            headlineInfo.contentSubCategoryDescriptor = headlineInfo.contentSubCategory.ToString().ToLower();

            headlineInfo.reference.type = "accessionNo";
            headlineInfo.reference.guid = contentHeadline.AccessionNo;
            headlineInfo.reference.externalUri = UpdateReferenceUrl(headlineInfo.contentCategory, contentHeadline, isDuplicate);
            MapExtraReferenceInformation(headlineInfo,contentHeadline);


            // update language
            headlineInfo.baseLanguage = contentHeadline.BaseLanguage;
            headlineInfo.baseLanaguageDescriptor = GetLanguageToContentLanguage(contentHeadline.BaseLanguage);

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

            if (contentHeadline.PublicationTime > DateTime.MinValue)
            {
                headlineInfo.publicationDateTime = DateTimeFormatter.Merge(contentHeadline.PublicationDate, contentHeadline.PublicationTime);
                headlineInfo.publicationDateTimeDescriptor = DateTimeFormatter.FormatLongDateTime(headlineInfo.publicationDateTime);
            }

            headlineInfo.wordCount = contentHeadline.WordCount;
            headlineInfo.wordCountDescriptor = string.Format("{0} {1}",
                                                             NumberFormatter.Format(contentHeadline.WordCount, NumberFormatType.Whole),
                                                             ResourceText.GetString("words"));

            headlineInfo.title = ParseMarkup(contentHeadline.Headline);
            headlineInfo.snippet = ParseMarkup(contentHeadline.Snippet);
            headlineInfo.byline = ParseMarkup(contentHeadline.Byline);
            headlineInfo.credit = ParseMarkup(contentHeadline.Credit);
            headlineInfo.copyright = ParseMarkup(contentHeadline.Copyright);
            headlineInfo.sectionName = ParseMarkup(contentHeadline.SectionName);
            headlineInfo.thumbnailImage = GetThumbnailImage(contentHeadline);
        }
    }

}