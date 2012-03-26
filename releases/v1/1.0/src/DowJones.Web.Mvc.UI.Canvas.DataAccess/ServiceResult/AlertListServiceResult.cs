using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Handlers.DJInsider;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.Assets.V1_0;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Factiva.Gateway.Messages.Track.V1_0;
using Factiva.Gateway.Services.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using Factiva.Gateway.Managers;
using Alert = DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.Alert;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    [DataContract(Name = "alertListServiceResult", Namespace = "")]
    public class AlertListServiceResult : AbstractServiceResult, IPopulate<BaseModuleRequest>
    {
        [DataMember(Name = "package")]
        public AlertListPackage Package { get; set; }

        public void Populate(ControlData controlData, BaseModuleRequest request, IPreferences preferences)
        {
            ProcessServiceResult(
               MethodBase.GetCurrentMethod(),
               () =>
               {
                   // page and module ids are not required
                   if (request == null)
                   {
                       throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidGetRequest);
                   }

                   // Never use proxy for this
                   controlData.ProxyUserID = null;
                   controlData.ProxyUserNamespace = null;

                   GetData(request, controlData, preferences);
               },
               preferences);
        }

        private void GetData(IModuleRequest request, ControlData controlData, IPreferences preferences)
        {
            Package = new AlertListPackage();
            
            var availableAlerts = GetFolderListCollection(
                                                          GetAvailableAlertIds(controlData),
                                                          new[] { ProductType.Global, ProductType.Iff },
                                                          new[] { RequestAssetType.Personal, RequestAssetType.Subscribed },
                                                          controlData,
                                                          preferences);

            if (availableAlerts == null)
            {
                return;
            }

            Dictionary<int, Alert> currentAlertsDictionary = null;
            if (request.PageId.IsNotEmpty() && request.ModuleId.IsNotEmpty())
            {
                currentAlertsDictionary = GetCurrentAlertIds(GetModule(request, controlData, preferences));
            }

            Package.Alerts = new List<Alert>();

            foreach (var folderList in availableAlerts.OrderBy(folder => folder.folderName))
            {
                if (folderList.folderSharing == null)
                {
                    continue;
                }

                var alert = new Alert
                                {
                                    Id = folderList.folderID,
                                    Name = folderList.folderName,
                                    AssetType = ConvertToAlertAssetType(folderList.folderSharing.assetType),
                                    IsActive = folderList.folderSharing.status == FolderAssetStatus.Active,
                                    IsOwner = folderList.folderSharing.isOwner,
                                    IsGroupFolder = folderList.isGroupFolder,
                                    DeliveryMethod = ConvertToAlertDeliveryMethod(folderList.deliveryMethod),
                                    DeliveryTimes = ConvertToAlertDeliveryTimes(folderList.deliveryTimes),
                                    Email = folderList.email,
                                    NewsHits = folderList.newHits,
                                    ProductType = ConvertToAlertProductType(folderList.productType),
                                    DocumentFormat = folderList.documentFormat,
                                    PublishScope = ConvertToAlertShareAccessScope(folderList.folderSharing.sharingData.accessControlScope)
                                };

                if (currentAlertsDictionary != null && currentAlertsDictionary.ContainsKey(folderList.folderID))
                {
                    alert.IsInModule = true;
                    currentAlertsDictionary[folderList.folderID] = alert;
                }

                Package.Alerts.Add(alert);
            }

            if (currentAlertsDictionary != null)
                Package.ModuleAlerts = currentAlertsDictionary.Values.ToList();
        }

        private static AlertShareAccessScope ConvertToAlertShareAccessScope(ShareAccessScope accessControlScope)
        {
            switch (accessControlScope)
            {
                case ShareAccessScope.Account:
                    return AlertShareAccessScope.Account;
                case ShareAccessScope.Everyone:
                    return AlertShareAccessScope.Everyone;
                case ShareAccessScope.Personal:
                    return AlertShareAccessScope.Personal;
                case ShareAccessScope.PreviousScope:
                    return AlertShareAccessScope.PreviousScope;
            }
            throw new DowJonesInsiderException(DowJonesUtilitiesException.InvalidShareAccessScope);
        }

        private static AlertDeliveryTimes ConvertToAlertDeliveryTimes(DeliveryTimes deliveryTimes)
        {
            switch (deliveryTimes)
            {
                case DeliveryTimes.Afternoon:
                    return AlertDeliveryTimes.Afternoon;
                case DeliveryTimes.Both:
                    return AlertDeliveryTimes.Both;
                case DeliveryTimes.Continuous:
                    return AlertDeliveryTimes.Continuous;
                case DeliveryTimes.Morning:
                    return AlertDeliveryTimes.Morning;
                //case DeliveryTimes.None:
                default:
                    return AlertDeliveryTimes.None;
            }
        }

        private static AlertProductType ConvertToAlertProductType(ProductType productType)
        {
            switch (productType)
            {
                case ProductType.Global:
                    return AlertProductType.Global;
                case ProductType.FastTrack:
                    return AlertProductType.FastTrack;
                case ProductType.FCPCompany:
                    return AlertProductType.FCPCompany;
                case ProductType.FCPExecutive:
                    return AlertProductType.FCPExecutive;
                case ProductType.FCPIndustry:
                    return AlertProductType.FCPIndustry;
                case ProductType.IWE:
                    return AlertProductType.IWE;
                case ProductType.Lexis:
                    return AlertProductType.Lexis;
                case ProductType.Iff:
                    return AlertProductType.Iff;
                case ProductType.SelectFullText:
                    return AlertProductType.SelectFullText;
                case ProductType.SelectHeadlines:
                    return AlertProductType.SelectHeadlines;
                case ProductType.WealthManagementAlerts:
                    return AlertProductType.WealthManagementAlerts;
                case ProductType.InvestmentBankingAlerts:
                    return AlertProductType.InvestmentBankingAlerts;
                case ProductType.WealthManagementTriggers:
                    return AlertProductType.WealthManagementTriggers;
                case ProductType.InvestmentBankingTriggers:
                    return AlertProductType.InvestmentBankingTriggers;
                case ProductType.BRITriggers:
                    return AlertProductType.BRITriggers;
                case ProductType.BRI:
                    return AlertProductType.BRI;
                case ProductType.GlobalTrigger:
                    return AlertProductType.GlobalTrigger;
                case ProductType.WsjProfessional:
                    return AlertProductType.WsjProfessional;
                case ProductType.DjConsultant:
                    return AlertProductType.DjConsultant;
                case ProductType.DirectToClient:
                    return AlertProductType.DirectToClient;
                case ProductType.Author:
                    return AlertProductType.Author;
                case ProductType.NewAuthor:
                    return AlertProductType.NewAuthor;
            }
            throw new DowJonesInsiderException(DowJonesUtilitiesException.InvalidProductType);
        }

        private static AlertDeliveryMethod ConvertToAlertDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            switch (deliveryMethod)
            {
                case DeliveryMethod.Batch:
                    return AlertDeliveryMethod.Batch;
                case DeliveryMethod.Continuous:
                    return AlertDeliveryMethod.Continuous;
                case DeliveryMethod.Feed:
                    return AlertDeliveryMethod.Feed;
                case DeliveryMethod.Online:
                    return AlertDeliveryMethod.Online;
            }
            throw new DowJonesInsiderException(DowJonesUtilitiesException.InvalidDeliveryMethod);
        }

        private IEnumerable<FolderList> GetFolderListCollection(int[] groupFolderIds, ProductType[] productTypes, RequestAssetType[] requestAssetTypes, ControlData controlData, IPreferences preferences)
        {
            var getFolderListRequest = new GetFolderListRequest
            {
                groupFolderIds = groupFolderIds,
                excludeInactiveFolders = true,
                returnGroupFolderHitCounts = false,
                productType = productTypes,
                folderAssetType = requestAssetTypes
            };

            GetFolderListResponse getFolderListResponse = null;

            RecordTransaction(
                typeof(GetFolderHeadlinesRequest).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    getFolderListResponse = manager.Invoke<GetFolderListResponse>(getFolderListRequest, controlData).ObjectResponse;
                },
                new ModuleDataRetrievalManager(controlData, preferences));

            if (getFolderListResponse.folderListResponse != null &&
                getFolderListResponse.folderListResponse.folderListResultSet != null &&
                getFolderListResponse.folderListResponse.folderListResultSet.folderList != null)
            {
                return getFolderListResponse.folderListResponse.folderListResultSet.folderList;
            }
            return null;
        }

        private static AlertAssetType ConvertToAlertAssetType(ResponseAssetType assetType)
        {
            switch (assetType)
            {
                case ResponseAssetType.Assigned:
                    return AlertAssetType.Assigned;
                case ResponseAssetType.Personal:
                    return AlertAssetType.Personal;
                case ResponseAssetType.Subscribed:
                    return AlertAssetType.Subscribed;
                //case ResponseAssetType.Unknown:
                default:
                    return AlertAssetType.Unknown;
            }
        }

        private static Dictionary<int, Alert> GetCurrentAlertIds(AlertsNewspageModule module)
        {
            if (module == null || module.AlertCollection == null)
                return null;

            var currentAlerts = new Dictionary<int, Alert>();
            foreach (var id in module.AlertCollection.Select(alert => int.Parse(alert.AlertID)).Where(id => !currentAlerts.ContainsKey(id)))
            {
                currentAlerts.Add(id, new Alert{Id = id});
            }
            return currentAlerts;
        }

        private int[] GetAvailableAlertIds(ControlData controlData)
        {
            var getItemsByClassIDRequest = new GetItemsByClassIDRequest
            {
                ReturnUsersCategorizedItems = true,
                ClassID = new[] { PreferenceClassID.GroupFolder }
            };

            var groupFolderIds = new List<int>();

            PreferenceResponse preferenceResponse = null;
            var tempControlData = ControlDataManager.Clone(controlData);
            RecordTransaction(
                "AlertListServiceResult.Preference",
                "",
                () =>
                {
                    preferenceResponse = PreferenceService.GetItemsByClassID(tempControlData, getItemsByClassIDRequest);
                    if (preferenceResponse.rc != 0)
                    {
                        // Throw the new exception
                        throw new DowJonesUtilitiesException(preferenceResponse.rc);
                    }
                },
                tempControlData);

            if (preferenceResponse.GroupFolder != null)
            {
                groupFolderIds.AddRange(from groupFolderPreferenceItem in preferenceResponse.GroupFolder
                                        where !string.IsNullOrEmpty(groupFolderPreferenceItem.ItemValue)
                                        select int.Parse(groupFolderPreferenceItem.ItemValue));
            }
            if (preferenceResponse.CategorizedItems != null)
            {
                groupFolderIds.AddRange(from categorizedItem in preferenceResponse.CategorizedItems
                                        where !string.IsNullOrEmpty(categorizedItem.ItemName)
                                        select int.Parse(categorizedItem.ItemName));
            }
            return groupFolderIds.ToArray();
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
    }
}
