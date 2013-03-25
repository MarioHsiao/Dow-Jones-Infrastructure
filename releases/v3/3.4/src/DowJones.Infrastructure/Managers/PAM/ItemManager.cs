using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Loggers;
using DowJones.Managers.Abstract;
using DowJones.Session;
using Factiva.Gateway.Messages.Assets.Item.V1_0;
using GWShareScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope;
using log4net;

namespace DowJones.Managers.PAM
{
    public class ItemManager : AbstractAggregationManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ItemManager));
        public ItemManager(IControlData controlData, ITransactionTimer transactionTimer)
            : base(controlData, transactionTimer)
        {

        }
        protected override ILog Log
        {
            get { return _log; }
        }

        public void SetShareProperties(IEnumerable<long> itemIds, GWShareScope shareScope)
        {
            var sharePeoperties = new ShareProperties();
            var request = new SetSharePropertiesRequest();
            sharePeoperties.AccessControlScope = MapShareScope(shareScope);
            sharePeoperties.AssignedScope = ShareScope.Personal;
            sharePeoperties.ListingScope = ShareScope.Personal;
            sharePeoperties.SharePromotion = ShareScope.Personal;
            request.ShareProperties = sharePeoperties;
            foreach (var itemId in itemIds)
            {
                try
                {
                    request.Id = itemId;
                    SetShareProperties(request);
                }
                catch (Exception ex)
                {
                    Log.Warn("Error setting Item share properties for Item " + itemId, ex);
                }

            }

        }

        public SetSharePropertiesResponse SetShareProperties(SetSharePropertiesRequest request)
        {
            return Process<SetSharePropertiesResponse>(request);
        }


        private AccessControlScope MapShareScope(GWShareScope gatewayShareScope)
        {
            switch (gatewayShareScope)
            {
                case GWShareScope.Personal:
                    return AccessControlScope.Personal;

                case GWShareScope.Everyone:
                    return AccessControlScope.Everyone;

                case GWShareScope.AccountAdmin:
                    return AccessControlScope.AccountAdmin;

                case GWShareScope.Account:
                    return AccessControlScope.Account;

                //case GWShareScope.Group:
                //    return AccessControlScope.Account;

                default:
                    return AccessControlScope.Personal;
            }
        }
    }
}
