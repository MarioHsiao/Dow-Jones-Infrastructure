using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.DependencyInjection;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Infrastructure.Common;
using DowJones.Managers.Search;
using DowJones.Mapping;
using DowJones.Preferences;
using DowJones.Search;
using DowJones.Search.Filters;
using DowJones.Session;
using DowJones.Web.Mvc.Search.Requests.Filters;
using DowJones.Web.Mvc.Search.UI.Components.Builders;

namespace DowJones.Web.Mvc.Search.Requests.Mappers
{
    public class SimpleSearchRequestMapper : TypeMapper<SimpleSearchRequest, AbstractSearchQuery>
    {
        private readonly SearchRequestMapper _baseMapper;

        public SimpleSearchRequestMapper(SearchRequestMapper baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public override AbstractSearchQuery Map(SimpleSearchRequest source)
        {
            var query = new SimpleSearchQuery
                            {
                                Keywords = source.FreeText,
                                Source = source.Source,
                            };

            _baseMapper.Map(query, source);

            return query;
        }
    }

    public class SimpleSearchQueryToRequestMapper : TypeMapper<SimpleSearchQuery, SimpleSearchRequest>
    {
        #region --- Dependency Injection ----
        [Inject("Injecting Product")]
        public Product _product
        {
            get { return (Product)ServiceLocator.Resolve(typeof(Product)); }
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
        #endregion

        private Dictionary<NewsFilterCategory, List<string>> codesDict = new Dictionary<NewsFilterCategory, List<string>>();
        private Dictionary<NewsFilterCategory, Dictionary<string, string>> codeDescDict;

        public override SimpleSearchRequest Map(SimpleSearchQuery source)
        {
            var query = new SimpleSearchRequest { FreeText = source.Keywords };

            var searchRequestMapper = new SearchRequestMapper(_product, new DateTimeFormatter(_prefrences.InterfaceLanguage));
            searchRequestMapper.Map(query, source);

            #region ---- Get the description for filters ---
            if (query != null)
            {
                #region ---- Get all the codes ----
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
                    //News Filters
                    setNewsFilterDesc(query.Filters);
                    #endregion
                }
            }
            #endregion

            return query;
        }

        #region ---- Private Methods ----
        private void getNewsFilterCodes(QueryFilters newsFilters)
        {
            if (newsFilters != null && newsFilters.Count > 0)
            {
                foreach (var newsFilter in newsFilters.Where(newsFilter => newsFilter.Category != NewsFilterCategory.Keyword && newsFilter.Category != NewsFilterCategory.DateRange))
                {
                    if (!codesDict.ContainsKey(newsFilter.Category))
                    {
                        codesDict[newsFilter.Category] = new List<string>();
                    }
                    codesDict[newsFilter.Category].Add(newsFilter.Code);
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
        #endregion
    }
}
