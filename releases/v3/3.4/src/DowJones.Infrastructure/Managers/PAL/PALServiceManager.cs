using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Loggers;
using DowJones.Managers.Abstract;
using DowJones.PALService;
using DowJones.Session;
using DowJones.Utilities;
using log4net;
using ServiceObjects = DowJones.PALService;
using GWShareScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope;


namespace DowJones.Managers.PAL
{
    public class PALServiceManager : AbstractAggregationManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(PALServiceManager));
        public PALServiceManager(IControlData controlData, ITransactionTimer transactionTimer = null)
            : base(controlData, transactionTimer)
        {

        }
        protected override ILog Log
        {
            get { return _log; }
        }

        public void UpdateSavedSearchScope(IEnumerable<long> savedSearchIds, GWShareScope shareScope)
        {
            using (var service = ServiceFactory<ServiceObjects.PreferenceItemScopeService>.Create(ControlData))
            {
                var itemList = new UpdateItemList();
                itemList.Id = savedSearchIds.ToArray();
                itemList.Scope = MapShareScope(shareScope);
                itemList.ScopeSpecified = true;
                itemList.IncludeChildAsset = true;
                itemList.IncludeChildAssetSpecified = true;
                itemList.ItemClassId = 58;
                itemList.ItemClassIdSpecified = true;
                var itemListArr = new UpdateItemList[] {itemList};
                service.UpdatePreferenceItemScope(itemListArr);
            }
        }


        private ItemScope MapShareScope(GWShareScope gatewayShareScope)
        {
            switch (gatewayShareScope)
            {
                case GWShareScope.Personal:
                    return ItemScope.Personal;

                case GWShareScope.Everyone:
                    return ItemScope.Everyone;

                //case GWShareScope.AccountAdmin:
                //    return ItemScope.Account;

                case GWShareScope.Account:
                    return ItemScope.Account;

                //case GWShareScope.Group:
                //    return ItemScope.Account;

                default:
                    return ItemScope.Personal;
            }
        }
    }


    
    
}
