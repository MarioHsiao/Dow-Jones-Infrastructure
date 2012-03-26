using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using Factiva.Gateway.Messages.Assets.Item.V1_0;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using IdCollection = Factiva.Gateway.Messages.Assets.Item.V1_0.IdCollection;
using SyndicationItem = DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.SyndicationItem;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    [DataContract(Name = "syndicationItemsServiceResult", Namespace = "")]
    public class SyndicationItemsServiceResult : AbstractServiceResult, IPopulate<BaseModuleRequest>
    {
        [DataMember(Name = "package")]
        public SyndicationItemsPackage Package { get; set; }

        public void Populate(ControlData controlData, BaseModuleRequest request, IPreferences preferences)
        {
            ProcessServiceResult(
               MethodBase.GetCurrentMethod(),
               () =>
               {
                   if (request == null || !request.IsValid())
                   {
                       throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidGetRequest);
                   }

                   GetData(request, controlData, preferences);
               },
               preferences);
        }

        private void GetData(BaseModuleRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule(request, controlData, preferences);
            var getItemsByIdsRequest = new GetItemsByIDsRequest {IdCollection = new IdCollection()};

            foreach (var syndicationFeedId in module.SyndicationFeedIDCollection)
            {
                long feedId;
                if (long.TryParse(syndicationFeedId, out feedId))
                {
                    getItemsByIdsRequest.IdCollection.Add(feedId);
                }
            }

            GetItemsByIDsResponse getItemsByIDsResponse = null;

            RecordTransaction(
                "SyndicationItemsService.GetItemsByIDs",
                "",
                manager =>
                {
                    getItemsByIDsResponse = manager.Invoke<GetItemsByIDsResponse>(getItemsByIdsRequest).ObjectResponse;
                },
                new ModuleDataRetrievalManager(controlData, preferences));

            Package = new SyndicationItemsPackage{SyndicationItems = new List<SyndicationItem>()};
            foreach (var syndicationFeedId in getItemsByIdsRequest.IdCollection)
            {
                foreach (var syndicationItem in getItemsByIDsResponse.ItemCollection.Select(item => item as Factiva.Gateway.Messages.Assets.Item.V1_0.SyndicationItem))
                {
                    if (syndicationItem == null)
                    {
                        throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidSyndicationItem);
                    }

                    if (syndicationItem.Id == syndicationFeedId)
                    {
                        Package.SyndicationItems.Add(new SyndicationItem
                                                            {
                                                                Id = syndicationItem.Id,
                                                                Url = syndicationItem.Properties.Value
                                                            });
                    }
                }
            }
        }

        protected internal SyndicationNewspageModule GetModule(IModuleRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule<SyndicationNewspageModule>(request, controlData, preferences);
            if (module.SyndicationFeedIDCollection.Count <= 0)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptySyndicationFeedIDCollection);
            }

            return module;
        }
    }
}
