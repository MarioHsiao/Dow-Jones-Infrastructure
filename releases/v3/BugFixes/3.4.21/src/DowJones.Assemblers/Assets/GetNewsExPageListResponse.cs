using System.Collections.Generic;
using DowJones.Ajax.TabStrip;
using Factiva.Gateway.Messages.Assets.PNP.V1_0;
using Factiva.Gateway.Messages.Assets.V1_0;

namespace DowJones.Assemblers.Assets
{
    public class GetNewsExPageListResponse
    {
        private GenerateTabOptions _generateTabOptions;

        public TabStripDataResult Process(GetNewsPageListResponse getNewsPageListResponse, GenerateTabOptions generateTabOptions)
        {
            _generateTabOptions = generateTabOptions;

            var result = new TabStripDataResult { resultSet = new TabStripDataResultSet() };

            foreach (var page in getNewsPageListResponse.pageListResult.pageList)
            {
                var tabInfo = new Tab
                                {
                                    Id =  page.pageID,
                                    Title = page.name,
                                    TabPosition = page.position
                                };
                
                //setting tab status : unlock, lock or personal
                if (page.type == ResponseAssetType.Personal && page.shareProperties.assignedScope == ShareScope.Account && page.shareProperties.accessControlScope == ShareAccessScope.Account)
                    tabInfo.Status = TabStatus.UnLocked;
                else if (page.type == ResponseAssetType.Assigned)
                    tabInfo.Status = TabStatus.Locked;
                else 
                    tabInfo.Status = TabStatus.Personal;

                tabInfo.Options = AddTabOptions(tabInfo);

                result.resultSet.tab.Add(tabInfo);
	            }

            var tAsc = new TabStripComparer();
            result.resultSet.tab.Sort(tAsc);
            //result.resultSet.tab.Sort(delegate(Tab tab0, Tab tab1) { return tab0.TabPosition.CompareTo(tab1.TabPosition); });
            return result;
        }

        private List<OptionTypes> AddTabOptions(Tab tab)
        {
            if (tab == null) return null;

            return _generateTabOptions != null ? _generateTabOptions(tab) : null;
        }
    }

    public class TabStripComparer : IComparer<Tab>
    {
        public int Compare(Tab x, Tab y)
        {
            if (x != null && y != null)
            {
                if (x.TabPosition > 0 && y.TabPosition > 0)
                {
                    if (x.TabPosition > y.TabPosition) return 1;
                    if (x.TabPosition < y.TabPosition) return -1;
                    return 0;
                }

                if (x.TabPosition == 0 || y.TabPosition == 0)
                    return string.Compare(x.Title, y.Title);
            }
            return 0;
        }

    }
}
