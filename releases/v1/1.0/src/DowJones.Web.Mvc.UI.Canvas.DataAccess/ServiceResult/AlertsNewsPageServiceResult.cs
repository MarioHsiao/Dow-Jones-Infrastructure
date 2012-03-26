// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlertsNewsPageServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using DowJones.Session;
using DowJones.Tools.Ajax.Converters;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Managers.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Alerts;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Track.V1_0;
using Alert = Factiva.Gateway.Messages.Assets.Pages.V1_0.Alert;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using ResultSortOrder = Factiva.Gateway.Messages.Search.V2_0.ResultSortOrder;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    [DataContract(Name = "alertsNewsPageModuleServiceResult", Namespace = "")]
    public class AlertsNewsPageServiceResult :
        Generic.AbstractServiceResult<AlertsNewsPageServicePartResult<AlertsPackage>, AlertsPackage>,
        IPopulate<NewsPageHeadlineModuleDataRequest>,
        IUpdateDefinitionAndPopulate<AlertsNewsPageModuleUpdateRequest, NewsPageHeadlineModuleDataRequest>
    {
        private bool hasCacheBeenEnabled = Settings.Default.IncludeCacheKeyGeneration &&
                                                    Settings.Default.CacheAlertsNewsPageModuleService;

        private HeadlineListConversionManager conversionManager;
        
        #region Implementation of IPopulate

        public void Populate(ControlData controlData, NewsPageHeadlineModuleDataRequest request, IPreferences preferences)
        {
            UpdateDefinitionAndPopulate(controlData, null, request, preferences);
        }

        #endregion

        #region Implementation of IUpateDefinitionAndPopulate

        public void UpdateDefinitionAndPopulate(ControlData controlData, AlertsNewsPageModuleUpdateRequest updateRequest, NewsPageHeadlineModuleDataRequest getRequest, IPreferences preferences)
        {
            ProcessServiceResult(
                MethodBase.GetCurrentMethod(),
                () =>
                    {
                        if (updateRequest != null && updateRequest.IsValid())
                        {
                            // fire some type of update transaction
                        }

                        if (getRequest == null || !getRequest.IsValid())
                        {
                            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidGetRequest);
                        }

                        if (preferences == null)
                        {
                            preferences = GetPreferences(controlData);
                        }

                        hasCacheBeenEnabled = hasCacheBeenEnabled && getRequest.CacheState != CacheState.Off;
                        conversionManager = new HeadlineListConversionManager(new DateTimeFormatter(preferences));
                        GetModuleData(getRequest, controlData, preferences);
                    },
                    preferences);
        }

        #endregion

        protected internal void GetModuleData(NewsPageHeadlineModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule(request, controlData, preferences);

            // get the module definition by moduleId and pageId
            MaxPartsAvailable = module.AlertCollection.Count;
            PartResults = GetAlerts(GetPage(module.AlertCollection, request), request, controlData, preferences);
        }

        protected internal AlertsNewspageModule GetModule(IModuleRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule<AlertsNewspageModule>(request, controlData, preferences);
            if (module.AlertCollection.Count <= 0)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptyAlertCollection);
            }

            return module;
        }

        protected internal List<AlertsNewsPageServicePartResult<AlertsPackage>> GetAlerts(IEnumerable<Alert> alertIds, NewsPageHeadlineModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            if (alertIds.Count() == 1)
            {
                return new List<AlertsNewsPageServicePartResult<AlertsPackage>> { ProcessAlert(request.FirstPartToReturn.ToString(), alertIds.First().AlertID, request, controlData, preferences) };
            }

            var localIndex = request.FirstPartToReturn - 1;
            var tasks = (from alertId in alertIds
                             let identifier = Interlocked.Increment(ref localIndex).ToString()
                             select TaskFactory.StartNew(
                                 () => ProcessAlert(identifier, alertId.AlertID, request, controlData, preferences), TaskCreationOptions.None)).ToList();

            Task.WaitAll(tasks.ToArray());
            var orderedTasks = tasks.OrderBy(task => Int32.Parse(task.Result.Identifier));
            return orderedTasks.Select(task => task.Result).ToList();
        }

        private AlertsNewsPageServicePartResult<AlertsPackage> ProcessAlert(string sortId, string alertId, NewsPageHeadlineModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var partResult = new AlertsNewsPageServicePartResult<AlertsPackage>();
            ProcessServicePartResult<AlertsPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                {
                    var alertsPackage = new AlertsPackage { AlertId = alertId };
                    int tempAlertId;

                    if (Int32.TryParse(alertId, out tempAlertId))
                    {
                        var getFolderHeadlinesRequest = new GetFolderHeadlinesRequest
                                                            {
                                                                folderID = tempAlertId,
                                                                searchQuery = new PerformContentSearchRequest()
                                                            };
                        getFolderHeadlinesRequest.searchQuery.StructuredSearch.Query.SearchCollectionCollection.AddRange(new[] { SearchCollection.WebSites, SearchCollection.Publications, });
                        getFolderHeadlinesRequest.searchQuery.MaxResults = request.MaxResultsToReturn;
                        getFolderHeadlinesRequest.searchQuery.FirstResult = request.FirstResultToReturn;
                        getFolderHeadlinesRequest.searchQuery.StructuredSearch.Formatting.DeduplicationMode = DeduplicationMode.Similar;
                        getFolderHeadlinesRequest.searchQuery.StructuredSearch.Formatting.SortOrder = ResultSortOrder.ArrivalTime;

                        GetFolderHeadlinesResponse getFolderHeadlinesResponse = null;

                        if (hasCacheBeenEnabled && request.FirstResultToReturn == 0)
                        {
                            var generator = new AlertCacheKeyGenerator(request.ModuleId, tempAlertId.ToString(), request.MaxResultsToReturn)
                                                {
                                                    CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                                };
                            controlData = generator.GetCacheControlData(controlData);
                        }

                        RecordTransaction(
                            typeof(GetFolderHeadlinesRequest).FullName,
                            MethodBase.GetCurrentMethod().Name,
                            manager =>
                                {
                                    getFolderHeadlinesResponse = manager.Invoke<GetFolderHeadlinesResponse>(getFolderHeadlinesRequest, controlData).ObjectResponse;
                                },
                            new ModuleDataRetrievalManager(controlData, preferences));

                        Folder alert;

                        if (getFolderHeadlinesResponse != null &&
                            getFolderHeadlinesResponse.folderHeadlinesResponse != null &&
                            getFolderHeadlinesResponse.folderHeadlinesResponse.folderHeadlinesResult != null &&
                            getFolderHeadlinesResponse.folderHeadlinesResponse.folderHeadlinesResult.folder != null &&
                            getFolderHeadlinesResponse.folderHeadlinesResponse.folderHeadlinesResult.folder[0] != null)
                        {
                            alert = getFolderHeadlinesResponse.folderHeadlinesResponse.folderHeadlinesResult.folder[0];
                        }
                        else
                        {
                            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.UnableToParseAlertId);
                        }

                        if (alert.status == 0)
                        {
                            alertsPackage.AlertName = alert.folderName;
                            alertsPackage.Result = conversionManager.Process(alert.PerformContentSearchResponse).Convert(request.TruncationType);
                            partResult.Package = alertsPackage;
                        }
                        else
                        {
                            partResult.ReturnCode = alert.status;
                            partResult.StatusMessage = ResourceTextManager.Instance.GetErrorMessage(partResult.ReturnCode.ToString());
                            partResult.Package = null;
                        }
                    }
                    else
                    {
                        partResult.ReturnCode = DowJonesUtilitiesException.UnableToParseAlertId;
                        partResult.StatusMessage = ResourceTextManager.Instance.GetErrorMessage(partResult.ReturnCode.ToString());
                    }
                },
                preferences);

            partResult.Identifier = sortId;
            return partResult;
        }
    }
}
