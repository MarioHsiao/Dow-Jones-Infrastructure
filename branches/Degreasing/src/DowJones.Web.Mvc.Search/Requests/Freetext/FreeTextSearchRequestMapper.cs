using System.Collections.Generic;
using System.Linq;
using DowJones.DependencyInjection;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Infrastructure.Common;
using DowJones.Managers.Search;
using DowJones.Mapping;
using DowJones.Preferences;
using DowJones.Search;
using DowJones.Session;
using DowJones.Web.Mvc.Search.Requests.Filters;
using DowJones.Web.Mvc.Search.Requests.Mappers;
using DowJones.Web.Mvc.Search.UI.Components.Builders;
using Factiva.Gateway.Messages.Assets.Lists.V1_0;

namespace DowJones.Web.Mvc.Search.Requests.Freetext
{
    public class FreeTextSearchRequestMapper : TypeMapper<FreeTextSearchRequest, AbstractSearchQuery>
    {
        private readonly AdvancedSearchRequestMapper _baseMapper;

        public FreeTextSearchRequestMapper(AdvancedSearchRequestMapper baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public override AbstractSearchQuery Map(FreeTextSearchRequest source)
        {
            var query = new FreeTextSearchQuery {FreeText = source.FreeText, FreeTextIn = source.FreeTextIn};
            _baseMapper.Map(query, source);

            return query;
        }
    }

    public class FreeTextSearchQueryToRequestMapper : TypeMapper<FreeTextSearchQuery, FreeTextSearchRequest>
    {
        #region --- Dependency Injection ----
        [Inject("Injecting Product")]
        public Product _product
        {
            get { return (Product)ServiceLocator.Resolve(typeof (Product)); }
        }

        [Inject("Injecting Preferences")]
        public IPreferences _prefrences
        {
            get { return (IPreferences)ServiceLocator.Resolve(typeof(IPreferences)); }
        }

        [Inject("Injecting ControlData")]
        public IControlData _controlData
        {
            get { return (IControlData)ServiceLocator.Resolve(typeof(IControlData)); }
        }

        [Inject("Injecting Source list service")]
        public ISourceListService SourceLstService { get; set; }

        [Inject("ProductSourceGroupConfigurationManager")]
        public ProductSourceGroupConfigurationManager PSGCM
        {
            get { return (ProductSourceGroupConfigurationManager)ServiceLocator.Resolve(typeof(ProductSourceGroupConfigurationManager)); }
        }
        #endregion

        private Dictionary<NewsFilterCategory, List<string>> codesDict = new Dictionary<NewsFilterCategory, List<string>>();
        private Dictionary<NewsFilterCategory, Dictionary<string, string>> codeDescDict;

        public override FreeTextSearchRequest Map(FreeTextSearchQuery source)
        {
            var query = new FreeTextSearchRequest { FreeText = source.FreeText, FreeTextIn = source.FreeTextIn };

            var advancedMapper = new AdvancedSearchRequestMapper(_product, new DateTimeFormatter(_prefrences.InterfaceLanguage));
            advancedMapper.Map(query, source);

            #region ---- Get the description for filters ---
            if(query != null)
            {
                #region ---- Get all the codes ----
                //Channel Filters
                getFilterCodes(query.Company, NewsFilterCategory.Company);
                getFilterCodes(query.Author, NewsFilterCategory.Author);
                getFilterCodes(query.Executive, NewsFilterCategory.Executive);
                getFilterCodes(query.Subject, NewsFilterCategory.Subject);
                getFilterCodes(query.Industry, NewsFilterCategory.Industry);
                getFilterCodes(query.Region, NewsFilterCategory.Region);
                getFilterCodes(query.Source);

                //News Filters
                getNewsFilterCodes(query.Filters);
                #endregion


                if (codesDict.Count > 0)
                {
                    #region ---- Make a PerformMetaDataSearch service call to get the description ----
                    var searchManager = new SearchManager(_controlData, _prefrences);
                    codeDescDict = searchManager.GetDescriptorsByCode(codesDict);

                    #endregion

                    #region ---- Set the description for each code in the filter ----

                    //Channel Filters
                    setFilterDesc(query.Company, NewsFilterCategory.Company);
                    setFilterDesc(query.Author, NewsFilterCategory.Author);
                    setFilterDesc(query.Executive, NewsFilterCategory.Executive);
                    setFilterDesc(query.Subject, NewsFilterCategory.Subject);
                    setFilterDesc(query.Industry, NewsFilterCategory.Industry);
                    setFilterDesc(query.Region, NewsFilterCategory.Region);
                    setFilterDesc(query.Source);

                    //News Filters
                    setNewsFilterDesc(query.Filters);

                    #endregion
                }
            }
            #endregion

            return query;
        }

        #region ---- Private Methods ----
        private void getFilterCodes(SearchFilter searchFilter, NewsFilterCategory filterCategory)
        {
            if (searchFilter != null && searchFilter.Include != null)
            {
                codesDict[filterCategory] = searchFilter.Include.Select(filter => filter.Code).ToList();
            }
            if (searchFilter != null && searchFilter.Exclude != null)
            {
                if(codesDict.ContainsKey(filterCategory))
                    codesDict[filterCategory].AddRange(searchFilter.Exclude.Select(filter => filter.Code));
                else
                    codesDict[filterCategory] = searchFilter.Exclude.Select(filter => filter.Code).ToList();
            }
        }

        private void getFilterCodes(SourceSearchFilter sourceFilter)
        {
            if (sourceFilter != null)
            {
                if(sourceFilter.Include != null && sourceFilter.Include.Count() > 0)
                {
                    #region ---- Get Source Group Name ----

                    var dictSourceGroupNames = GetSourceGroupNames(_product.SourceGroupConfigurationId);

                    #endregion

                    string type;
                    int listId = 0;
                    foreach (var source in sourceFilter.Include)
                    {
                        foreach (var item in source)
                        {
                            type = item.Type.ToUpper();
                            if (type != "PDF" && type != "SN" && type != "BY")
                            {
                                if (!codesDict.ContainsKey(NewsFilterCategory.Source))
                                {
                                    codesDict[NewsFilterCategory.Source] = new List<string>();
                                }
                                codesDict[NewsFilterCategory.Source].Add(item.Code);
                            }
                            else
                            {
                                switch (type)
                                {
                                    case "PDF":
                                        item.Name = dictSourceGroupNames[item.Code.ToLower()];
                                        break;
                                    default:
                                        item.Name = item.Code;
                                        break;
                                }
                            }
                        }
                    }
                }
                else if(!string.IsNullOrEmpty(sourceFilter.ListId))
                {
                    #region ---- Set Source List Name ----
                    var sourceList = SourceLstService.GetSourceLists();
                    if (sourceList != null && sourceList.Count > 0)
                    {
                        foreach (var list in sourceList)
                        {
                            if (list.Key == sourceFilter.ListId)
                            {
                                sourceFilter.ListName = list.Value;
                                break;
                            }
                        }
                    }
                    #endregion
                }
            }
            
        }

        private void getNewsFilterCodes(QueryFilters newsFilters)
        {
            if (newsFilters != null && newsFilters.Count > 0)
            {
                foreach (var newsFilter in newsFilters.Where(newsFilter => newsFilter.Category != NewsFilterCategory.Keyword && newsFilter.Category != NewsFilterCategory.DateRange))
                {
                    if(!codesDict.ContainsKey(newsFilter.Category))
                    {
                        codesDict[newsFilter.Category] = new List<string>();
                    }
                    codesDict[newsFilter.Category].Add(newsFilter.Code);
                }
            }
        }

        private void setFilterDesc(SearchFilter searchFilter, NewsFilterCategory filterCategory)
        {
            if (searchFilter != null && searchFilter.Include != null)
            {
                foreach(var filter in searchFilter.Include)
                {
                    filter.Name = codeDescDict[filterCategory][filter.Code.ToLower()];
                }
            }
            if (searchFilter != null && searchFilter.Exclude != null)
            {
                foreach (var filter in searchFilter.Exclude)
                {
                    filter.Name = codeDescDict[filterCategory][filter.Code.ToLower()];
                }
            }
        }

        private void setFilterDesc(SourceSearchFilter sourceFilter)
        {
            if (sourceFilter == null)
            {
                return;
            }

            if (sourceFilter.Include != null)
            {
                string type;
                foreach (var source in sourceFilter.Include)
                {
                    foreach (var item in source)
                    {
                        type = item.Type.ToUpper();
                        if (type != "PDF" && type != "SN" && type != "BY")
                        {
                            item.Name = codeDescDict[NewsFilterCategory.Source][item.Code.ToLower()];
                        }
                    }
                }
            }
        }

        private void setNewsFilterDesc(QueryFilters newsFilters)
        {
            if (newsFilters != null && newsFilters.Count > 0)
            {
                foreach (var queryFilter in newsFilters.Where(newsFilter => newsFilter.Category != NewsFilterCategory.Keyword && newsFilter.Category != NewsFilterCategory.DateRange))
                {
                    queryFilter.Name = codeDescDict[queryFilter.Category][queryFilter.Code.ToLower()];
                }
            }
        }

        private Dictionary<string, string> GetSourceGroupNames(string productId)
        {
            var dictSrcGroupItems = new Dictionary<string, string>();
            var sourceGroups = PSGCM.SourceGroups(productId);
            foreach (var sourceGroup in sourceGroups)
            {
                var dsgi = dictSrcGroupItems.Concat(GetSourceGroupItem(sourceGroup))
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
                dictSrcGroupItems = dsgi;
            }
            return dictSrcGroupItems;
        }

        private Dictionary<string, string> GetSourceGroupItem(SourceGroup sourceGroup)
        {
            var dictSrcGroupItems = new Dictionary<string, string>();
            if (sourceGroup != null)
            {
                dictSrcGroupItems.Add(sourceGroup.PdfCode.ToLower(), sourceGroup.Descriptor);
                if (sourceGroup.SourceGroupCollection != null && sourceGroup.SourceGroupCollection.Count > 0)
                {
                    foreach (var group in sourceGroup.SourceGroupCollection)
                    {
                        var d = dictSrcGroupItems.Concat(GetSourceGroupItem(group))
                            .ToDictionary(pair => pair.Key, pair => pair.Value);
                        dictSrcGroupItems = d;
                    }
                }
            }
            return dictSrcGroupItems;
        }
        #endregion
    }
}