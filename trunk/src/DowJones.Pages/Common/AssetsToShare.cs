using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Managers.Alert;
using DowJones.Managers.PAL;
using DowJones.Managers.PAM;
using DowJones.Session;
using Factiva.Gateway.Messages.Assets.Item.V1_0;
using Factiva.Gateway.Messages.Assets.V1_0;
using GWShareScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope;
using Factiva.Gateway.Messages.Track.V1_0;

namespace DowJones.Pages.Common
{
    public sealed class  AssetsToShare
    {
        public SharingAssetType Type { get; private set; }
        public IEnumerable<object> Items { get; set; }
        public GWShareScope ShareScope { get; private set; }
        
        public AssetsToShare(SharingAssetType type, GWShareScope shareScope)
        {
            Type = type;
            ShareScope = shareScope;
        }

        internal void ShareAssets(IControlData controlData)
        {
            switch (Type)
            {
                case SharingAssetType.Alert:
                    {
                        MakePersonalAlertsPublic(controlData);
                        break;
                    }
                case SharingAssetType.SavedSearch:
                    {
                        MakePersonalSavedSearchesPublic(controlData);
                        break;
                    }
                case SharingAssetType.ChartItem:
                    {
                        SetChartItemsScope(controlData);
                        break;
                    }
            }
            
        }


        private void MakePersonalAlertsPublic(IControlData controlData)
        {
            var alertIds = GetTypedAssetList<int>();
            if (alertIds == null || !alertIds.Any())
                return;

            var alertManager = new AlertManager(controlData, null, null);

             var request = new SetFolderSharePropertiesRequest
                              {
                                  folderShareDetails = alertIds.Select(alertId => new FolderShareDetails
                                                                                      {
                                                                                          folderId = alertId,
                                                                                          SharingData = new Factiva.Gateway.Messages.Assets.V1_0.ShareProperties
                                                                                                            {
                                                                                                                accessControlScope = ShareAccessScope.Everyone,
                                                                                                                allowCopy = false,
                                                                                                                assignedScope = Factiva.Gateway.Messages.Assets.V1_0.ShareScope.Personal,
                                                                                                                externalAccess = ShareAccess.Allow,
                                                                                                                listingScope = Factiva.Gateway.Messages.Assets.V1_0.ShareScope.Personal,
                                                                                                                sharePromotion = Factiva.Gateway.Messages.Assets.V1_0.ShareScope.Personal
                                                                                                            }
                                                                                      }).ToArray()
                              };

            alertManager.SetFolderShareProperties(request);


        }

        private void MakePersonalSavedSearchesPublic(IControlData controlData)
        {
            var savedSearchIds = GetTypedAssetList<long>();
            if (savedSearchIds == null || !savedSearchIds.Any())
                return;

            var palSvcMgr = new PALServiceManager(controlData);
            
            palSvcMgr.UpdateSavedSearchScope(savedSearchIds, ShareScope);
        }


        private void SetChartItemsScope(IControlData controlData)
        {
            var chartIDs = GetTypedAssetList<long>();
            if (chartIDs == null || !chartIDs.Any())
                return;

            var itemManager = new ItemManager(controlData, null);

            itemManager.SetShareProperties(chartIDs, ShareScope);
        }

        private IEnumerable<T> GetTypedAssetList<T>()
        {
            var assetList = (from asset in Items select (T) asset);
            return assetList;
        }
    }

    public enum SharingAssetType
    {
        Alert,
        SavedSearch,
        ChartItem
        
    }
}
