using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DowJones.AlertEditor;
using DowJones.Extensions;
using DowJones.Managers.Search;
using DowJones.Preferences;
using DowJones.Search;
using DowJones.Session;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Track.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;

namespace DowJones.Managers.Alert
{
    /// <summary>
    /// AlertManager
    /// </summary>
    /// 
  
    public class AlertManager
    {
        private readonly IControlData _controlData;
        private readonly IPreferences _preferences;
        private readonly SearchQueryBuilder _queryBuilder;

        private const char TimezoneSeparator = '|';


        public AlertManager(IControlData controlData, IPreferences preferences, SearchQueryBuilder queryBuilder)
        {
            _controlData = controlData;
            _preferences = preferences;
            _queryBuilder = queryBuilder;
        }

        public FolderIDResponse CreateAlert(AlertRequestBase alertRequestBase)
        {
            var createFolderRequest = new CreateFolderRequest();

            //Build request
            Map(createFolderRequest, alertRequestBase);

            //Create alert
            return CreateAlert(createFolderRequest);
        }

        public FolderIDResponse UpdateAlert(AlertRequestBase alertRequestBase)
        {
            var updateRequest = new ReviseFolderRequest();

            //Build request
            Map(updateRequest, alertRequestBase);

            //Create alert
            return RevieseAlert(updateRequest);
        }

        public FolderIDResponse UpdateAlertProperties(AlertProperties properties)
        {
            var details = GetAlertDetails(properties.AlertId);

            var updateRequest = new ReviseFolderRequest();
            Map(properties, updateRequest);

            //Copy non properties fields from original response
            updateRequest.userQuery = details.userQuery;
            updateRequest.userQueryDefinition = details.userQueryDefinition;

            //Update alert
            return RevieseAlert(updateRequest);
        }



        public AlertDetails GetAlert(string alertId)
        {
            var folderDetails = GetAlertDetails(alertId);

            var properties = new AlertProperties();
            Map(folderDetails, properties);

            AbstractSearchQuery searchQuery = null;
            var setup = SearchSetupScreen.SimpleSearch;
            if (folderDetails.userQueryDefinition != null && IsCommunicatorTypeAlert(properties.ProductType))
            {
                var displayOptions = (CommunicatorDisplayOptions)folderDetails.userQueryDefinition.DisplayOptions;
                setup = displayOptions.SearchSetupScreen;
            }
            switch (setup)
            {
               case SearchSetupScreen.AdvancedSearch:
                    searchQuery = Mapper.Map<FreeTextSearchQuery>(folderDetails.userQueryDefinition);
                    break;
               case SearchSetupScreen.SimpleSearch:
                    searchQuery = Mapper.Map<SimpleSearchQuery>(folderDetails.userQueryDefinition);
                    break;
                default:
                    throw new NotImplementedException("Can not convert query asset to search query");
            }

            var response = new AlertDetails(searchQuery, properties, folderDetails);
            response.SearchType = setup;

            return response;
        }

        

        private void Map(TrackFolder trackFolder, AlertRequestBase alertRequestBase)
        {
            //Set alert properties
            AlertProperties alertProp = alertRequestBase.Properties;
            Map(alertProp, trackFolder);

            //Set search query
            AbstractSearchQuery query = alertRequestBase.SearchQuery;
            StructuredSearch structuredSearch = _queryBuilder.GetRequest<PerformContentSearchRequest>(query).StructuredSearch;
            
            structuredSearch.Query.Dates = null;

            trackFolder.userQuery = GenerateStructuredSearchQuery(structuredSearch);

            //Set user date in query object
            trackFolder.userQueryDefinition = GetUseQueryDefinition(alertRequestBase);
        }


        private static UserQueryDefinition GetUseQueryDefinition(AlertRequestBase searchQuery)
        {
            var query = new UserQueryDefinition
                            {
                                Groups = Mapper.Map<GroupCollection>(searchQuery.SearchQuery)
                            };

            if (IsCommunicatorTypeAlert(searchQuery.Properties.ProductType))
            {
                var displayOptions = new CommunicatorDisplayOptions
                                         {
                                             SearchSetupScreen = searchQuery.SearchQuery.GetSearchSetupScreen()
                                         };
                query.DisplayOptions = displayOptions;
            }
        

            return query;
        }

        private static bool IsCommunicatorTypeAlert(ProductType productType)
        {
            switch (productType)
            {
                case ProductType.Made_Author:
                case ProductType.Made_New_Author:
                case ProductType.Made_News:
                    return true;
            }
            return false;
        }


        private void Map(FolderDetails source, AlertProperties target)
        {
            target.AlertId = source.folderID.ToString();
            target.AlertName = source.folderName;
            target.IsGroupAlert = source.isGroupFolder;
            target.ProductType = source.productType;
            target.EmailAddress = source.email;
            target.DocumentFormat = source.documentFormat;
            target.DeliveryMethod = source.deliveryMethod;
            if (source.deliveryMethod == DeliveryMethod.Continuous)
            {
                target.DeliveryTime = DeliveryTimes.Continuous;
            }
            else
            {
                target.DeliveryTime = source.deliveryTimes;
            }
            target.DispositionType = source.dispositionType;
            target.DocumentType = source.documentType;
            target.RemoveDuplicate = source.deduplicationLevel;
            if (!string.IsNullOrEmpty(source.timeZone))
            {
                var timezoneInfo = source.timeZone.Split(TimezoneSeparator);
                target.TimeZoneOffset = timezoneInfo[0];

                target.AdjustToDaylightSavingsTime = (timezoneInfo.Length > 1 &&
                                                      timezoneInfo[1].Equals("Y", StringComparison.InvariantCultureIgnoreCase));
            }
        }

        private static DeliveryMethod MapDeliveryMethod(DeliveryTimes deliveryTime)
        {
            switch (deliveryTime)
            {
                case DeliveryTimes.Both:
                case DeliveryTimes.Afternoon:
                case DeliveryTimes.Morning:
                    return DeliveryMethod.Batch;
                case DeliveryTimes.Continuous:
                    return DeliveryMethod.Continuous;
                default:
                    return DeliveryMethod.Online;
            }
        }

        private void Map(AlertProperties source, TrackFolder target)
        {
            target.folderID = source.AlertId;
            target.folderName = source.AlertName;
            target.isGroupFolder = source.IsGroupAlert;
            target.productType = source.ProductType;
            target.email = source.EmailAddress;
            target.documentFormat = source.DocumentFormat;
            target.documentFormatSpecified = true;

            target.deliveryMethod = MapDeliveryMethod(source.DeliveryTime);

            target.deliveryTimes = source.DeliveryTime;
            target.deliveryTimesSpecified = true;

            target.dispositionType = source.DispositionType;
            target.dispositionTypeSpecified = true;

            target.documentType = source.DocumentType;
            target.documentTypeSpecified = true;

            target.deduplicationLevel = source.RemoveDuplicate;

            target.timeZone = source.TimeZoneOffset;
            if (!string.IsNullOrEmpty(source.TimeZoneOffset))
            {
                target.timeZone = string.Format("{0}{1}{2}", source.TimeZoneOffset, TimezoneSeparator, source.AdjustToDaylightSavingsTime ? "Y" : "N");
            }
            target.langCode = _preferences.InterfaceLanguage;
            target.langCodeSpecified = true;
        }

        public static string GenerateStructuredSearchQuery(StructuredSearch structuredSearch)
        {
            string xmlString = null;
            using (var sw = new StringWriter())
            {
                var ser = new XmlSerializer(typeof (StructuredSearch));
                ser.Serialize(sw, structuredSearch);
                var doc = new XmlDocument();
                doc.LoadXml(sw.ToString());
                XmlElement bodyNode = doc.DocumentElement;
                if (bodyNode != null)
                {
                    bodyNode.Attributes.RemoveAll();
                    xmlString = bodyNode.OuterXml;
                }
            }
            return xmlString;
        }


        private FolderIDResponse CreateAlert(CreateFolderRequest createFolderRequest)
        {
            ServiceResponse serviceResponse = TrackService.CreateFolder(ControlDataManager.Convert(_controlData), createFolderRequest);
            var response = serviceResponse.GetObject<CreateFolderResponse>();
            return response != null ? response.folderIDResponse : null;
        }

        private FolderIDResponse RevieseAlert(ReviseFolderRequest reviseFolder)
        {
            ServiceResponse serviceResponse = TrackService.ReviseFolder(ControlDataManager.Convert(_controlData), reviseFolder);
            var response = serviceResponse.GetObject<ReviseFolderResponse>();
            return response != null ? response.folderIDResponse : null;
        }

        private FolderDetails GetAlertDetails(string alertId)
        {
            var getAlertRequest = new GetFolderRequest();
            getAlertRequest.folderID = alertId;
            ServiceResponse serviceResponse = TrackService.GetFolder(ControlDataManager.Convert(_controlData), getAlertRequest);
            var response = serviceResponse.GetObject<GetFolderResponse>();
            return (response != null && response.folderResponse != null) ? response.folderResponse.folderDetails : null;
        }
    }
}
