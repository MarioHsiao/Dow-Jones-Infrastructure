using System;
using System.Collections.Generic;
using EMG.Tools.Ajax;
using EMG.Tools.Ajax.Converters;
using EMG.Tools.Ajax.Converters.HeadlineList;
using EMG.Tools.Ajax.HeadlineList;
using EMG.Tools.Ajax.TriggerList;
using EMG.Utility.Formatters;
using EMG.Utility.Formatters.Globalization;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Trigger.V1_1;
using log4net;

namespace EMG.Utility.Ajax.Converters.TriggerList
{
    public class PerformTriggerSearchResponseConverter : AbstractHeadlineListDataResultSetConverter
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(PerformTriggerSearchResponseConverter));
        private readonly TriggerListDataResult _result = new TriggerListDataResult();
        private readonly PerformTriggerSearchResponse _response;

        private GenerateExternalUrlForHeadlineInfo _generateExternalURLForHeadlineInfo;
        private GenerateExternalUrlForTriggerInfo _generateExternalURLForTriggerInfo;
        private GenerateSnippetThumbnailForHeadlineInfo _generateSnippetThumbnailForHeadlineInfo;
        private GenerateExternalUrlForPropertyInfo _generateExternalURLForPropertyInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformTriggerSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public PerformTriggerSearchResponseConverter(PerformTriggerSearchResponse response, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            _response = response;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformTriggerSearchResponseConverter"/> class.
        /// </summary>
        public PerformTriggerSearchResponseConverter(PerformTriggerSearchResponse response)
            : this(response, "en")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformTriggerSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public PerformTriggerSearchResponseConverter(PerformTriggerSearchResponse response, DateTimeFormatter dateTimeFormatter)
            : base(dateTimeFormatter)
        {
            _response = response;
        }

        private void ProcessTriggers(TriggerSearchResult resultSet)
        {
            int i = resultSet.FirstResult;
            foreach (MasterTrigger trigger in resultSet.TriggerSearchResultSet.TriggerCollection )
            {
                TriggerInfo triggerInfo = new TriggerInfo();
                Convert(triggerInfo, trigger, i++);
                _result.resultSet.triggers.Add(triggerInfo);
            }
        }

        private string UpdateTriggerExternalUri(Trigger trigger)
        {
            if (trigger == null)
                return null;
            return _generateExternalURLForTriggerInfo != null ? _generateExternalURLForTriggerInfo(trigger) : null;
        }

        private string UpdatePropertyExternalUri(Property property)
        {
            if (property == null)
                return null;
            return _generateExternalURLForPropertyInfo != null ? _generateExternalURLForPropertyInfo(property) : null;
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
            return _generateExternalURLForHeadlineInfo != null ? _generateExternalURLForHeadlineInfo(contentHeadline, isDuplicate) : null;
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
            MapExtraReferenceInformation(headlineInfo, contentHeadline);


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
                if (_generateSnippetThumbnailForHeadlineInfo != null)
                {
                    _generateSnippetThumbnailForHeadlineInfo(image, contentHeadline);
                }
                return image;
            }
            return null;
        }


        private void Convert(TriggerInfo triggerInfo, MasterTrigger trigger, int index)
        {
            triggerInfo.displayName = trigger.DisplayName;
            triggerInfo.id = trigger.Id;

            triggerInfo.externalUri = UpdateTriggerExternalUri(trigger);

            // update the index
            triggerInfo.index = new WholeNumber(index);
            NumberFormatter.Format(triggerInfo.index);

            triggerInfo.lowerDate = trigger.LowerDate;
            triggerInfo.lowerDateDescriptor = DateTimeFormatter.FormatDate(triggerInfo.lowerDate);

            triggerInfo.upperDate = trigger.UpperDate;
            triggerInfo.upperDateDescriptor = DateTimeFormatter.FormatDate(triggerInfo.upperDate);

            triggerInfo.createDateTime = trigger.CreateDateTime;
            triggerInfo.createDateTimeDescriptor = DateTimeFormatter.FormatDateTime(triggerInfo.createDateTime);

            triggerInfo.modifiedDateTime = trigger.ModifiedDateTime;
            triggerInfo.modifiedDateTimeDescriptor = DateTimeFormatter.FormatDate(triggerInfo.modifiedDateTime);

            triggerInfo.typeCode = trigger.Typecode;
            triggerInfo.category = trigger.Category;
            triggerInfo.categoryDescriptor = trigger.Category.ToString();

            triggerInfo.displayName = trigger.DisplayName;
            
            triggerInfo.masterHeadlineDataResult = Convert(trigger.DefaultContentHeadline);
            
            triggerInfo.companies = GetCompanyProperties(trigger.PropertyCollection);
            
            triggerInfo.executives = GetExecutiveProperties(trigger.PropertyCollection);

            triggerInfo.detectionPhrase = trigger.DefaultDetectionPhraseCollection;
            
        }
        
        private List<PropertyInfo>GetCompanyProperties(IEnumerable<Property> propertyCollection)
        {
            List<PropertyInfo> list = new List<PropertyInfo>();
            foreach (Property property in propertyCollection)
            {
                if (!(property is Organization))
                    continue;
                PropertyInfo info = new PropertyInfo();
                info.code = property.Code;
                info.displayName = property.DisplayName;
                info.type = property.Type;
                info.externalUri = UpdatePropertyExternalUri(property);
                list.Add(info);
            }
            return list;
        }

        private List<PropertyInfo> GetExecutiveProperties(IEnumerable<Property> propertyCollection)
        {
            List<PropertyInfo> list = new List<PropertyInfo>();
            foreach (Property property in propertyCollection)
            {
                if (!(property is Person))
                    continue;
                PropertyInfo info = new PropertyInfo();
                info.code = property.Code;
                info.displayName = property.DisplayName;
                info.type = property.Type;
                list.Add(info);
            }
            return list;
        }

        private HeadlineListDataResult Convert(ContentHeadline contentHeadline)
        {
            HeadlineListDataResult result = new HeadlineListDataResult();
            result.resultSet = new HeadlineListDataResultSet();
            result.hitCount = new WholeNumber(1);
            result.resultSet.first = new WholeNumber(1);
            result.resultSet.count = new WholeNumber(1);

            // Format
            NumberFormatter.Format(_result.hitCount);
            NumberFormatter.Format(_result.resultSet.first);
            NumberFormatter.Format(_result.resultSet.count);

            HeadlineInfo headlineInfo = new HeadlineInfo();
            Convert(headlineInfo, contentHeadline, 1, false);
            result.resultSet.headlines.Add(headlineInfo);

            return result;
        }

        #region Overrides of AbstractHeadlineListDataResultSetConverter

        public override IListDataResult Process()
        {
            return Process(null,null, null, null);
        }

        #endregion

        #region Implementation of IExtendedListDataResultConverter

        public IListDataResult Process(GenerateExternalUrlForTriggerInfo generateExternalURLForTriggerInfo,GenerateExternalUrlForHeadlineInfo generateExternalURLForHeadlineInfo, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo, GenerateExternalUrlForPropertyInfo generateExternalURLForPropertyInfo)
        {
            _generateExternalURLForTriggerInfo = generateExternalURLForTriggerInfo;
            _generateExternalURLForHeadlineInfo = generateExternalURLForHeadlineInfo;
            _generateSnippetThumbnailForHeadlineInfo = generateSnippetThumbnailForHeadlineInfo;
            _generateExternalURLForPropertyInfo = generateExternalURLForPropertyInfo;

            if (_response == null || _response.TriggerSearchResponse == null || _response.TriggerSearchResponse.TriggerSearchResult == null || _response.TriggerSearchResponse.TriggerSearchResult.HitCount <= 0)
                return _result;

            // Add the HitCount to the result set
            _result.hitCount = new WholeNumber(_response.TriggerSearchResponse.TriggerSearchResult.HitCount);
            NumberFormatter.Format(_result.hitCount);

            if (_response.TriggerSearchResponse.TriggerSearchResult.TriggerSearchResultSet == null)
                return _result;
            TriggerSearchResult resultSet = _response.TriggerSearchResponse.TriggerSearchResult;
            _result.resultSet.first = new WholeNumber(resultSet.FirstResult);
            _result.resultSet.count = new WholeNumber(resultSet.Count);

            // Format
            NumberFormatter.Format(_result.resultSet.first);
            NumberFormatter.Format(_result.resultSet.count);

            if (resultSet.Count <= 0)
                return _result;
            ProcessTriggers(resultSet);

            return _result;
        }

        #endregion
    }
}
