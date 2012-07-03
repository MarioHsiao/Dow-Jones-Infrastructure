using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Session;
using DowJones.Token;
using Factiva.Gateway.Messages.Assets.Lists.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using log4net;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using DowJones.Extensions;

namespace DowJones.Managers.Search
{
    public class ProductSourceGroupConfigurationManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (ProductSourceGroupConfigurationManager));
        private static readonly object SyncLock = new object();

        private IControlData _controlData;
        private static GetListsDetailsListResponse _response;
        private ITokenRegistry _tokenRegistry;


        public ProductSourceGroupConfigurationManager(IControlData controlData, ITokenRegistry tokenRegistry)
        {
            _controlData = controlData;
            _tokenRegistry = tokenRegistry;
        }

        /// <summary>
        /// Get list of primary source types (PST) for a product
        /// </summary>
        /// <param name="productId">Product ID</param>i
        /// <returns></returns>
        public IEnumerable<string> PrimarySourceTypes(string productId)
        {
            Logger.Debug("PrimarySourceTypes::ProductId=" + productId);
            var item = GetProductItem(productId);

            if (item == null)
                return Enumerable.Empty<string>();

            return GetPrimarySourceTypes(item.SourceGroupCollection);
        }

        /// <summary>
        /// Get list of primary souce types (PST) for a given product secondary source group 
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="sourceGoupId">Secondary source group ID - PDF(Product Define Code)</param>
        /// <returns></returns>
        public IEnumerable<string> PrimarySourceTypes(string productId, string sourceGoupId)
        {
            Logger.Debug("PrimarySourceTypes::ProductId/GroupId=" + productId + "/" + sourceGoupId);
            var sourceGroups = GetProductItem(productId);
            SourceGroup sourceGroup = FindSourceGroup(sourceGroups.SourceGroupCollection, sourceGoupId);
            if (sourceGroup != null)
            {
                return GetPrimarySourceTypes(new List<SourceGroup> {sourceGroup});
            }
            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Get top level source hierarchy for a product
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="sourceGoupId">Filter source groups for a specific parent source group</param>
        /// <returns></returns>
        public IEnumerable<SourceGroup> SourceGroups(string productId, string sourceGoupId = null)
        {
            Logger.Debug("SourceGroups::ProductId=" + productId);
            var item = GetProductItem(productId);

            if (item == null)
                return Enumerable.Empty<SourceGroup>();

            var list = GetSourceGroups(item.SourceGroupCollection);
            
            if (String.IsNullOrEmpty(sourceGoupId))
            {
                return list;
            }

            var parentGroup = FindSourceGroup(list, sourceGoupId);

            if (parentGroup == null)
                return Enumerable.Empty<SourceGroup>();

            return parentGroup.SourceGroupCollection;
        }

        private IEnumerable<SourceGroup> GetSourceGroups(IEnumerable<SourceGroup> sourceGroups)
        {
            var list = new List<SourceGroup>();
            foreach (SourceGroup sourceGroup in sourceGroups)
            {
                var group = CloneSourceGroup(sourceGroup);
                list.Add(group);
                if (sourceGroup.SourceGroupCollection != null && sourceGroup.SourceGroupCollection.Count > 0)
                {
                    group.SourceGroupCollection = new SourceGroupCollection();
                    group.SourceGroupCollection.AddRange(GetSourceGroups(sourceGroup.SourceGroupCollection));
                }
            }
            return list;
        }

        private SourceGroup CloneSourceGroup(SourceGroup sourceGroup)
        {
            var obj = new SourceGroup {PdfCode = sourceGroup.PdfCode, Descriptor = _tokenRegistry.Get(sourceGroup.Descriptor), PrimarySourceCollection = new PrimarySourceTypeCollection()};
            sourceGroup.PrimarySourceCollection.ForEach(a => obj.PrimarySourceCollection.Add(new PrimarySourceType {PstCode = a.PstCode}));
            return obj;
        }

        private static IEnumerable<string> GetPrimarySourceTypes(IEnumerable<SourceGroup> sg)
        {
            var list = new List<string>();
            foreach (SourceGroup sourceGroup in sg)
            {
                if (sourceGroup.SourceGroupCollection != null && sourceGroup.SourceGroupCollection.Count > 0)
                {
                    IEnumerable<string> subList = GetPrimarySourceTypes(sourceGroup.SourceGroupCollection);
                    list.AddRange(subList);
                }
                else if (sourceGroup.PrimarySourceCollection != null && sourceGroup.PrimarySourceCollection.Count > 0)
                {
                    list.AddRange(sourceGroup.PrimarySourceCollection.Select(primarySourceType => primarySourceType.PstCode));
                }
            }
            return list;
        }


        private static SourceGroup FindSourceGroup(IEnumerable<SourceGroup> sg, string groupId)
        {
            foreach (SourceGroup sourceGroup in sg)
            {
                if (string.Compare(sourceGroup.PdfCode, groupId, true) == 0)
                {
                    return sourceGroup;
                }
                if (sourceGroup.SourceGroupCollection != null && sourceGroup.SourceGroupCollection.Count > 0)
                {
                    SourceGroup subGroup = FindSourceGroup(sourceGroup.SourceGroupCollection, groupId);
                    if (subGroup != null)
                    {
                        return subGroup;
                    }
                }
            }
            return null;
        }


        private SourceGroupConfigurationListItem GetProductItem(string productId)
        {
            Load();

            if (_response == null)
                return new SourceGroupConfigurationListItem();

            return (from listDetailsItem in _response.ListDetailsItems
                    from item in listDetailsItem.List.Items
                    where item.ItemCode.Equals(productId, StringComparison.InvariantCultureIgnoreCase)
                    select item as SourceGroupConfigurationListItem).FirstOrDefault();
        }

        private void Load()
        {
            if (_response != null)
            {
                return;
            }
            Logger.Debug("Load products source groupping configuration");
            var request = new GetListsDetailsListRequest();
            request.ListTypes.Add(ListType.SourceGroupConfigurationList);
            //request.FilterGroups.Add(new FilterGroup { Filters = { new ItemCodeSearchFilter { ItemCodes = { "MADE" } } } });
            lock (SyncLock)
            {
                if (_response == null)
                {
                    ControlData controlData = ControlDataManager.Convert(_controlData);
                    ServiceResponse response = ListService.GetListsDetailsList(controlData, request);
                    _response = response.GetObject<GetListsDetailsListResponse>();
                }
            }
        }
    }
}
