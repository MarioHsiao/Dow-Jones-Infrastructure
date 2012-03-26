// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetTriggerDetailsResultConverter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DowJones.Ajax;
using DowJones.Ajax.HeadlineList;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Formatters.Globalization.TimeZone;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Trigger.V1_1;
using log4net;

namespace DowJones.Assemblers.Headlines
{
    public class GetTriggerDetailsResultConverter : AbstractHeadlineListDataResultSetConverter
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(GetTriggerDetailsResultConverter));

        private readonly TriggerDetailResponse response;
        
        private readonly HeadlineListDataResult result = new HeadlineListDataResult();
        
        private GenerateExternalUrlForHeadlineInfo generateExternalUrlForHeadlineInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTriggerDetailsResultConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public GetTriggerDetailsResultConverter(TriggerDetailResponse response, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            this.response = response;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTriggerDetailsResultConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        public GetTriggerDetailsResultConverter(TriggerDetailResponse response)
            : this(response, "en")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTriggerDetailsResultConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public GetTriggerDetailsResultConverter(TriggerDetailResponse response, DateTimeFormatter dateTimeFormatter)
            : base(dateTimeFormatter)
        {
            this.response = response;
        }

        #region Overrides of AbstractHeadlineListDataResultSetConverter

        public override IListDataResult Process()
        {
            return Process(null);
        }

        #endregion

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="generateExternalURLForHeadlineInfo">The generate external URL.</param>
        /// <returns></returns>
        public IListDataResult Process(GenerateExternalUrlForHeadlineInfo generateExternalURLForHeadlineInfo)
        {
            generateExternalUrlForHeadlineInfo = generateExternalURLForHeadlineInfo;

            if (response == null || response.TriggerDetailResult == null || response.TriggerDetailResult.TriggerDetailResultSet == null || response.TriggerDetailResult.TriggerDetailResultSet.TriggerCollection.Count <= 0)
            {
                return result;
            }

            // Format
            NumberFormatter.Format(result.hitCount);

            var resultSet = response.TriggerDetailResult.TriggerDetailResultSet;
            result.resultSet.first = new WholeNumber(response.TriggerDetailResult.FirstResult + 1);

            var masterTrigger = new MasterTrigger();
            if (response.TriggerDetailResult.TriggerDetailResultSet.TriggerCollection[0].GetType().FullName == typeof(MasterTrigger).FullName)
            {
                masterTrigger = response.TriggerDetailResult.TriggerDetailResultSet.TriggerCollection[0] as MasterTrigger;
            }

            result.isTimeInGMT = DateTimeFormatter.CurrentTimeZone == TimeZoneManager.GmtTimeZone;

            // Add the HitCount and Requested Count to the result set
            if (masterTrigger != null && masterTrigger.Count > 0)
            {
                result.hitCount = new WholeNumber(masterTrigger.DocumentCount);
                result.resultSet.count = new WholeNumber(masterTrigger.Count);
                result.resultSet.duplicateCount = new WholeNumber(masterTrigger.Count);
            }
            else
            {
                return result;
            }

            // Format
            NumberFormatter.Format(result.resultSet.first);
            NumberFormatter.Format(result.resultSet.count);
            NumberFormatter.Format(result.resultSet.duplicateCount);
            ProcessTriggerDetailHeadlines(resultSet);

            return result;
        }

        private void ProcessTriggerDetailHeadlines(TriggerDetailResultSet contentHeadlineResultSet)
        {
            int i = response.TriggerDetailResult.FirstResult + 1;
            foreach (MasterTrigger tr in contentHeadlineResultSet.TriggerCollection)
            {
                foreach (var document in tr.DocumentCollection)
                {
                    var headlineInfo = new HeadlineInfo();
                    Convert(headlineInfo, document.ContentHeadline, i++, false);
                    result.resultSet.headlines.Add(headlineInfo);
                }
            }
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
            {
                return;
            }

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
            headlineInfo.wordCountDescriptor = string.Format(
                                                             "{0} {1}",
                                                             NumberFormatter.Format(contentHeadline.WordCount, NumberFormatType.Whole),
                                                             ResourceText.GetString("words"));

            headlineInfo.title = ParseMarkup(contentHeadline.Headline);
            headlineInfo.snippet = ParseMarkup(contentHeadline.Snippet);
            headlineInfo.byline = ParseMarkup(contentHeadline.Byline);
            headlineInfo.codedAuthors = ReplaceMarkup( contentHeadline.Byline, contentHeadline.CodeSets );
            headlineInfo.credit = ParseMarkup(contentHeadline.Credit);
            headlineInfo.copyright = ParseMarkup(contentHeadline.Copyright);
            headlineInfo.sectionName = ParseMarkup(contentHeadline.SectionName);
            headlineInfo.truncationRules = GetTruncationRules(contentHeadline.TruncationRules);
        }
    }
}
