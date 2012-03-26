using System;
using System.Collections.Generic;
using DowJones.Tools.Ajax;
using DowJones.Tools.Ajax.Converters;
using DowJones.Tools.Ajax.Converters.HeadlineList;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Tools.Ajax.TriggerList;
using DowJones.Utilities.Formatters;
using DowJones.Utilities.Formatters.Globalization;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Trigger.V2_0;
using log4net;


namespace DowJones.Utilities.Ajax.TriggerList.Converters
{
    public class SearchTriggersResponseConverter: AbstractHeadlineListDataResultSetConverter
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(SearchTriggersResponseConverter));
        private readonly TriggerListDataResult Result = new TriggerListDataResult();
        private readonly SearchTriggersResponse Response;

        private GenerateExternalUrlForSearchTriggerInfo GenerateExternalUrlForTriggerInfo;
        private GenerateSnippetThumbnailForHeadlineInfo GenerateSnippetThumbnailForHeadlineInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchTriggersResponseConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public SearchTriggersResponseConverter(SearchTriggersResponse response, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            Response = response;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchTriggersResponseConverter"/> class.
        /// </summary>
        public SearchTriggersResponseConverter(SearchTriggersResponse response)
            : this(response, "en")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchTriggersResponseConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public SearchTriggersResponseConverter(SearchTriggersResponse response, DateTimeFormatter dateTimeFormatter)
            : base(dateTimeFormatter)
        {
            Response = response;
        }

        private void ProcessTriggers(SearchTriggersResultsContent resultSet)
        {
            int i = resultSet.FirstResult + 1;
            foreach (MasterTrigger trigger in resultSet.MasterTriggerCollection)
            {
                var triggerInfo = new TriggerInfo();
                Convert(triggerInfo, trigger, i++);
                Result.resultSet.triggers.Add(triggerInfo);
            }
        }

        private string UpdateTriggerExternalUri(MasterTrigger trigger)
        {
            if (trigger == null)
                return null;
            return GenerateExternalUrlForTriggerInfo != null ? GenerateExternalUrlForTriggerInfo(trigger) : null;
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
                var image = new ThumbnailImage {GUID = item.Ref};
                if (GenerateSnippetThumbnailForHeadlineInfo != null)
                {
                    GenerateSnippetThumbnailForHeadlineInfo(image, contentHeadline);
                }
                return image;
            }
            return null;
        }


        private void Convert(TriggerInfo triggerInfo, MasterTrigger trigger, int index)
        {
            triggerInfo.id = trigger.Id;

            triggerInfo.externalUri = UpdateTriggerExternalUri(trigger);

            // update the index
            triggerInfo.index = new WholeNumber(index);
            NumberFormatter.Format(triggerInfo.index);

            triggerInfo.lowerDate = trigger.DisplayLowerDateTime;
            triggerInfo.lowerDateDescriptor = DateTimeFormatter.FormatDate(triggerInfo.lowerDate);

            triggerInfo.upperDate = trigger.DisplayUpperDateTime;
            triggerInfo.upperDateDescriptor = DateTimeFormatter.FormatDate(triggerInfo.upperDate);

            triggerInfo.createDateTime = trigger.CreateDateTime;
            triggerInfo.createDateTimeDescriptor = DateTimeFormatter.FormatDateTime(triggerInfo.createDateTime);

            triggerInfo.createDate = trigger.CreateDateTime.Date;
            triggerInfo.createDateDescriptor = DateTimeFormatter.FormatDate(triggerInfo.createDate);

            triggerInfo.modifiedDateTime = trigger.ModifiedDateTime;
            triggerInfo.modifiedDateTimeDescriptor = DateTimeFormatter.FormatDate(triggerInfo.modifiedDateTime);

            triggerInfo.modifiedDate = trigger.ModifiedDateTime.Date;
            triggerInfo.modifiedDateDescriptor = DateTimeFormatter.FormatDate(triggerInfo.modifiedDate);

            triggerInfo.typeCode = trigger.Code;
            triggerInfo.category = CategoryType.Master;
            triggerInfo.categoryDescriptor = CategoryType.Master.ToString();

            triggerInfo.displayName = trigger.DisplayName;
            
            if (trigger.DocumentCount > 0)
                triggerInfo.masterHeadlineDataResult = Convert(trigger.DocumentCollection[0].ContentHeadline);

            if (trigger.InstanceCount > 0)
                triggerInfo.detectionPhrase =
                    MapDetectionPhrase(
                        trigger.DocumentCollection[0].InstanceTriggerCollection[0].DetectionPhraseCollection);

            triggerInfo.additionalDetails = GetAdditionalDetails(trigger.PropertyCollection);
        }

        private static List<DetectionPhrase> MapDetectionPhrase(IEnumerable<DetectionWord> collection)
        {
           var detectionPhraseList = new List<DetectionPhrase>();

            foreach (var detection in collection)
            {
                var detectionPhrase = new DetectionPhrase
                                          {
                                              StartIndex = detection.StartIndex,
                                              EndIndex = detection.EndIndex,
                                              Value = detection.MixedValue,
                                          };

                detectionPhraseList.Add(detectionPhrase);
            }

            return detectionPhraseList;
        }

        private static List<GenericPropertyInfo>GetAdditionalDetails(IEnumerable<Property> propertyCollection)
        {
            var list = new List<GenericPropertyInfo>();
            foreach (Property property in propertyCollection)
            {
               var info = new GenericPropertyInfo {displayName = property.DisplayName};

                if (property.ValueCollection != null && property.ValueCollection.Count > 0)
                {
                    if (!string.IsNullOrEmpty(property.ValueCollection[0].Content))
                        info.value = property.ValueCollection[0].Content;

                    info.isMostRecent = property.ValueCollection[0].IsMostRecent;
                }

                list.Add(info);
            }
            return list;
        }


        private HeadlineListDataResult Convert(ContentHeadline contentHeadline)
        {
            var result = new HeadlineListDataResult
                             {
                                 resultSet = new HeadlineListDataResultSet(),
                                 hitCount = new WholeNumber(1)
                             };
            result.resultSet.first = new WholeNumber(1);
            result.resultSet.count = new WholeNumber(1);

            // Format
            NumberFormatter.Format(Result.hitCount);
            NumberFormatter.Format(Result.resultSet.first);
            NumberFormatter.Format(Result.resultSet.count);

            var headlineInfo = new HeadlineInfo();
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

        public IListDataResult Process(GenerateExternalUrlForSearchTriggerInfo generateExternalURLForTriggerInfo, GenerateExternalUrlForHeadlineInfo generateExternalURLForHeadlineInfo, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo, GenerateExternalUrlForPropertyInfo generateExternalURLForPropertyInfo)
        {
            GenerateExternalUrlForTriggerInfo = generateExternalURLForTriggerInfo;
            GenerateExternalUrlForHeadlineInfo = generateExternalURLForHeadlineInfo;
            GenerateSnippetThumbnailForHeadlineInfo = generateSnippetThumbnailForHeadlineInfo;

            if (Response == null
                || Response.SearchTriggersResponseContent == null
                || Response.SearchTriggersResponseContent.SearchTriggersResults == null 
                || Response.SearchTriggersResponseContent.SearchTriggersResults.TotalCount <= 0)
                return Result;

            // Add the HitCount to the result set
            Result.hitCount = new WholeNumber(Response.SearchTriggersResponseContent.SearchTriggersResults.TotalCount);
            NumberFormatter.Format(Result.hitCount);

            var resultSet = Response.SearchTriggersResponseContent;
            Result.resultSet.first = new WholeNumber(resultSet.SearchTriggersResults.FirstResult);
            Result.resultSet.count = new WholeNumber(resultSet.SearchTriggersResults.TotalCount);

            // Format
            NumberFormatter.Format(Result.resultSet.first);
            NumberFormatter.Format(Result.resultSet.count);

            if (resultSet.SearchTriggersResults.TotalCount <= 0)
                return Result;
            ProcessTriggers(resultSet.SearchTriggersResults);

            return Result;
        }

        #endregion
    }
}

