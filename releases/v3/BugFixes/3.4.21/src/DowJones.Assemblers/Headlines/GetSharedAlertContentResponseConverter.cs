using System;
using DowJones.Ajax;
using DowJones.Ajax.HeadlineList;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Formatters.Globalization.TimeZone;
using Factiva.Gateway.Messages.RTQueue.V1_0;
using Factiva.Gateway.Messages.Search.V2_0;
using log4net;

namespace DowJones.Assemblers.Headlines
{
    internal class GetSharedAlertContentResponseConverter : AbstractHeadlineListDataResultSetConverter
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(GetSharedAlertContentResponseConverter));
        private readonly GetSharedAlertContentResponse _response;
        private readonly int _startIndex;
        private readonly HeadlineListDataResult _result = new HeadlineListDataResult();

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public GetSharedAlertContentResponseConverter(GetSharedAlertContentResponse response, int startIndex, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            _response = response;
            _startIndex = startIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        public GetSharedAlertContentResponseConverter(GetSharedAlertContentResponse response, int startIndex)
            : this(response, startIndex, "en")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public GetSharedAlertContentResponseConverter(GetSharedAlertContentResponse response, int startIndex, DateTimeFormatter dateTimeFormatter)
            : base(dateTimeFormatter)
        {
            _response = response;
            _startIndex = startIndex;
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
        public IListDataResult Process(GenerateExternalUrlForHeadlineInfo generateExternalUrl, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnail)
        {
            if (_response == null || _response.ContentHeadlineResultSet == null || _response.ContentHeadlineResultSet.Count <= 0)
                return _result;

            // Add the HitCount to the result set
            _result.hitCount = new WholeNumber(_response.ContentHeadlineResultSet.Count);
            // Format
            NumberFormatter.Format(_result.hitCount);

            ContentHeadlineResultSet resultSet = _response.ContentHeadlineResultSet;
            _result.resultSet.first = new WholeNumber(resultSet.First);

            if (resultSet.Count <= 0)
                return _result;

            _result.resultSet.count = new WholeNumber(resultSet.Count);
            ProcessContentHeadlines(resultSet);
            NumberFormatter.Format(_result.resultSet.first);
            NumberFormatter.Format(_result.resultSet.count);

            _result.isTimeInGMT = (DateTimeFormatter.CurrentTimeZone == TimeZoneManager.GmtTimeZone);
            return _result;
        }

        private void ProcessContentHeadlines(ContentHeadlineResultSet contentHeadlineResultSet)
        {
            var i = (_startIndex > 0) ? _startIndex : contentHeadlineResultSet.First;
            foreach (ContentHeadline headline in contentHeadlineResultSet.ContentHeadlineCollection)
            {
                var headlineInfo = new HeadlineInfo();
                Convert(headlineInfo, headline, false, ++i);
                _result.resultSet.headlines.Add(headlineInfo);
            }
        }

        /// <summary>
        /// Converts the specified content headline.
        /// </summary>
        /// <param name="headlineInfo">The headline info.</param>
        /// <param name="contentHeadline">The content headline.</param>
        /// <param name="index">The index.</param>
        /// <param name="isDuplicate">if set to <c>true</c> [is duplicate].</param>
        protected internal override void Convert(HeadlineInfo headlineInfo, ContentHeadline contentHeadline, bool isDuplicate, int index = 0)
        {
            if (headlineInfo == null || contentHeadline == null)
                return;

            // update category information
            headlineInfo.contentSubCategory = headlineInfo.reference.contentSubCategory = MapContentSubCategory(contentHeadline);
            headlineInfo.contentSubCategoryDescriptor = headlineInfo.reference.contentSubCategoryDescriptor = headlineInfo.contentSubCategory.ToString().ToLower();

            headlineInfo.contentSubCategory = MapContentSubCategory(contentHeadline);
            headlineInfo.contentSubCategoryDescriptor = headlineInfo.contentSubCategory.ToString().ToLower();

            //headlineInfo.reference.externalUri = UpdateReferenceUrl(headlineInfo.contentCategory, contentHeadline, isDuplicate);
            MapExtraReferenceInformation(headlineInfo, contentHeadline);
            headlineInfo.reference.type = "accessionNo";
            headlineInfo.reference.guid = contentHeadline.AccessionNo;

            // update language
            headlineInfo.baseLanguage = contentHeadline.BaseLanguage;
            headlineInfo.baseLanguageDescriptor = GetLanguageToContentLanguage(contentHeadline.BaseLanguage);

            // update the index
            headlineInfo.index = new WholeNumber(index);
            NumberFormatter.Format(headlineInfo.index);

            // update source information
            headlineInfo.sourceReference = contentHeadline.SourceCode;
            headlineInfo.sourceDescriptor = contentHeadline.SourceName;

            // Update Word Count
            headlineInfo.wordCount = contentHeadline.WordCount;

            headlineInfo.title = ParseMarkup(contentHeadline.Headline);
            headlineInfo.time = GetTimeInSeconds(contentHeadline);

            headlineInfo.publicationDateTime = contentHeadline.PublicationDate;
            headlineInfo.publicationDateTimeDescriptor = DateTimeFormatter.FormatLongDate(headlineInfo.publicationDateTime);

            if (contentHeadline.PublicationTime <= DateTime.MinValue)
            {
                return;
            }

            var currentDate = DateTimeFormatter.Merge(contentHeadline.PublicationDate, contentHeadline.PublicationTime);
            headlineInfo.publicationDateTime = currentDate;
            // update publication date/time information
            //headlineInfo.publicationDateTimeDescriptor = string.Concat(DateTimeFormatter.FormatShortDate(headlineInfo.publicationDateTime), " ", DateTimeFormatter.FormatTime(headlineInfo.publicationDateTime));
            headlineInfo.publicationDateTimeDescriptor = DateTimeFormatter.FormatTime(headlineInfo.publicationDateTime);
        }
    }
}
