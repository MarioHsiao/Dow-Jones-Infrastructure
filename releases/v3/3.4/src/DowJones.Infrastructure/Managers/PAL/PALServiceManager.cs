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
    public class PALServiceManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(PALServiceManager));
        private const int SAVED_SEARCH_CLASS_ID = 58;
        private IPALPreferenceServiceProvider Provider { get; set; }
        public PALServiceManager(IPALPreferenceServiceProvider provider)
        {
            Provider = provider;
        }
        protected ILog Log
        {
            get { return _log; }
        }

        public void UpdateSavedSearchScope(IEnumerable<long> savedSearchIds, GWShareScope shareScope)
        {

            var itemList = new UpdateItemList();
            itemList.Id = savedSearchIds.ToArray();
            itemList.Scope = MapShareScope(shareScope);
            itemList.ScopeSpecified = true;
            itemList.ItemClassId = SAVED_SEARCH_CLASS_ID;
            itemList.ItemClassIdSpecified = true;
            var itemListArr = new UpdateItemList[] { itemList };
            Provider.UpdatePreferenceItemScope(itemListArr);

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
