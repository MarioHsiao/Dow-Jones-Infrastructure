using System;
using System.Collections.Generic;
using DowJones.Tools.Ajax;
using DowJones.Tools.Ajax.Converters;
using DowJones.Tools.Ajax.Converters.HeadlineList;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Utilities.Formatters;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Formatters.Globalization.TimeZone;
using Factiva.Gateway.Messages.RTQueue.V1_0;
using Factiva.Gateway.Messages.Search.V2_0;
using log4net;

namespace DowJones.Utilities.Ajax.RealtimeHeadlineList.Converters
{
    internal class CreateSharedAlertResponseConverter : AbstractHeadlineListDataResultSetConverter
    {
        public GenerateSnippetThumbnailForHeadlineInfo GenerateSnippetThumbnailForHeadlineInfo { get; set; }
        protected static readonly ILog _log = LogManager.GetLogger(typeof(CreateSharedAlertResponseConverter));
        private readonly CreateSharedAlertResponse _response;
        private readonly int _startIndex;
        private readonly HeadlineListDataResult _result = new HeadlineListDataResult();

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public CreateSharedAlertResponseConverter(CreateSharedAlertResponse response, int startIndex, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            _response = response;
            _startIndex = startIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        public CreateSharedAlertResponseConverter(CreateSharedAlertResponse response, int startIndex)
            : this(response, startIndex, "en")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public CreateSharedAlertResponseConverter(CreateSharedAlertResponse response, int startIndex, DateTimeFormatter dateTimeFormatter)
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

            if (_response.ContentHeadlineResultSet == null)
                return _result;
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
                Convert(headlineInfo, headline, ++i, false);
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
        internal void Convert(HeadlineInfo headlineInfo, ContentHeadline contentHeadline, int index, bool isDuplicate)
        {
            if (headlineInfo == null || contentHeadline == null)
                return;

            // update category information
            headlineInfo.contentCategory = MapContentCategory(contentHeadline);
            headlineInfo.contentCategoryDescriptor = headlineInfo.contentCategory.ToString().ToLower();

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


            //Update Word Count
            headlineInfo.wordCount = contentHeadline.WordCount;

            headlineInfo.title = ParseMarkup(contentHeadline.Headline);
            headlineInfo.truncatedTitle = ParseTruncatedTitle(headlineInfo.title, contentHeadline.TruncationRules);
            headlineInfo.time = GetTimeInSeconds(contentHeadline);

            //if (contentHeadline.PublicationTime != DateTime.MinValue)
            //{
            //    // update using both the date and time fields of the content headline object.
            //    headlineInfo.publicationDateTime = DateTimeFormatter.Merge(contentHeadline.PublicationDate, contentHeadline.PublicationTime);
            //    headlineInfo.publicationDateTimeDescriptor = DateTimeFormatter.FormatLongDateTime(headlineInfo.publicationDateTime);
            //}
            //else
            //{
            //    // update publication date field of the content headline object.
            //    headlineInfo.publicationDateTime = contentHeadline.PublicationDate;
            //    headlineInfo.publicationDateTimeDescriptor = DateTimeFormatter.FormatLongDate(headlineInfo.publicationDateTime);
            //}

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

        private static string GetTimeInSeconds(ContentHeadline contentHeadline)
        {
            if (contentHeadline == null || contentHeadline.ContentItems == null)
                return null;
            foreach (var item in contentHeadline.ContentItems.ItemCollection)
            {
                if (item.Type == "audio" || item.Type == "video")
                {
                    int seconds;
                    if (Int32.TryParse(item.Size, out seconds))
                    {
                        var t = new TimeSpan(0, 0, seconds);
                        return string.Concat(t.Minutes, ":", t.Seconds);
                    }
                    return null;
                }
            }
            return null;
        }

        private static string ParseTruncatedTitle(IList<Para> paras, TruncationRules rules)
        {
            if (paras.Count == 1)
            {
                if (paras[0].items.Count == 1 && paras[0].items[0].type.ToLower() == "textual")
                {
                    return rules.Small > 0 ? string.Concat(paras[0].items[0].value.Substring(0, rules.Small), "...") : paras[0].items[0].value;
                }
            }
            return null;
        }

    }
}
