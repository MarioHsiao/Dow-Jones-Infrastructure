using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DowJones.Exceptions;
using DowJones.Managers.Search.CodedNewsQueries;
using DowJones.Preferences;
using DowJones.Search.Core;
using DowJones.Session;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Track.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using TrackFilterItem = Factiva.Gateway.Messages.Track.V1_0.FilterItem;
using TrackFilterSourceItem = Factiva.Gateway.Messages.Track.V1_0.FilterSourceItem;
using TrackNewsFilters = Factiva.Gateway.Messages.Track.V1_0.NewsFilters;
using DowJones.Infrastructure.Alert;
using System.IO;

namespace DowJones.Managers.Alert
{
    public class SimpleAlertManager
    {
        private readonly IControlData _controlData;
        private readonly IPreferences _preferences;

        public SimpleAlertManager(IControlData controlData, IPreferences preferences)
        {
            _controlData = controlData;
            _preferences = preferences;
        }


        private TrackNewsFilters _newsFilters;

        public FolderIDResponse CreateFolder(SimpleAlertRequest request)
        {
            #region ---- Creat the request ----

            #region ---- Folder Request ----

            var fldrRequest = new CreateFolderRequest
                                  {
                                      productType = ProductType.Iff,
                                      folderName = request.AlertName,
                                      deliveryMethod = request.DeliveryMethod, 
                                      deliveryTimes = request.DeliveryTime, 
                                      deliveryTimesSpecified = true, 
                                      email = request.EmailAddress, 
                                      documentFormat = request.EmailFormat, 
                                      documentFormatSpecified = true, 
                                      documentType = DocumentType.FULL, 
                                      documentTypeSpecified = true, 
                                      deduplicationLevel = MapDedupLevel(request.RemoveDuplicate), 
                                      trackUserData = new TrackUserData()
                                  };
            #endregion

            #region ---- Transform NewsFilter ----
            _newsFilters = new TrackNewsFilters();
            if (request.NewsFilter != null && request.NewsFilter.Count > 0)
            {
                foreach (var filter in request.NewsFilter)
                {
                    switch (filter.FilterKey)
                    {
                        case "au":
                            _newsFilters.author = GetFilterItem(filter.Filter);
                            //_newsFilters.authorExclude = GetFilterItem(filter.ExcludeFilter);
                            break;
                        case "fds":
                            _newsFilters.company = GetFilterItem(filter.Filter);
                            //_newsFilters.companyExclude = GetFilterItem(filter.ExcludeFilter);
                            break;
                        case "pe":
                            _newsFilters.executive = GetFilterItem(filter.Filter);
                            //_newsFilters.executiveExclude = GetFilterItem(filter.ExcludeFilter);
                            break;
                        case "in":
                            _newsFilters.industry = GetFilterItem(filter.Filter);
                            //_newsFilters.industryExclude = GetFilterItem(filter.ExcludeFilter);
                            break;
                        case "ns":
                            _newsFilters.newsSubject = GetFilterItem(filter.Filter);
                            //_newsFilters.newsSubjectExclude = GetFilterItem(filter.ExcludeFilter);
                            break;
                        case "re":
                            _newsFilters.region = GetFilterItem(filter.Filter);
                            //_newsFilters.regionExclude = GetFilterItem(filter.ExcludeFilter);
                            break;
                        case "sc":
                            _newsFilters.source = GetFilterSourceItem(filter.Filter);
                            //_newsFilters.sourceExclude = GetFilterItem(filter.ExcludeFilter);
                            break;
                        case "key":
                            _newsFilters.keywords = GetKeywordFilterItem(filter.Filter);
                            
                            break;
                    }
                }
            }
            #endregion

            #region ---- User Data ---

            var userData = new UserData
                               {
                                   allWords = request.SearchText,
                                   newsFilters = _newsFilters,
                                   srchUIVer = "2", 
                                   rl = "1", 
                                   languageList = "custom", 
                                   searchType = "Simple", 
                                   sortBy = "date"
                               };
            if (request.DeliveryMethod == DeliveryMethod.Batch)
            {
                userData.maxNumber = "50";
            }

            if (_preferences.ContentLanguages != null && _preferences.ContentLanguages.Count > 0)
            {
                userData.searchLang = String.Join(",", _preferences.ContentLanguages.ToArray());
            }
            if (request.SelectedSources != null)
            {
                if (request.SelectedSources.SourceList != null)
                {
                    userData.sourceListDescStr = GetSourceListDescStr(request.SelectedSources.SourceList);
                    userData.sourceListStr = GetSourceListStr(request.SelectedSources.SourceList);
                }
                else
                {
                    if (request.SelectedSources.Source != null)
                    {
                        userData.sourceListDescStr = GetSourceListDescStr(request.SelectedSources.Source);
                        userData.sourceListStr = GetSourceListStr(request.SelectedSources.Source);
                    }
                }
            }

            #endregion

            fldrRequest.trackUserData.Data = userData;
            fldrRequest.userQuery = GetStructuredSearchQuery(request, _preferences);
            if (!string.IsNullOrEmpty(request.TimeZoneOffset))
            {
                fldrRequest.timeZone = string.Format("{0}|{1}", request.TimeZoneOffset, request.AdjustToDaylightSavingsTime ? "Y" : "N");
            }

            fldrRequest.langCode = _preferences.InterfaceLanguage;
            #endregion

            var serviceResponse = TrackService.CreateFolder(ControlDataManager.Convert(_controlData), fldrRequest);

            if (serviceResponse.rc == 0)
            {
                object responseObj;
                serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);
                var createFolderResponse = (CreateFolderResponse)responseObj;
                return createFolderResponse.folderIDResponse;
            }

            throw new DowJonesUtilitiesException(serviceResponse.rc);
        }

        private DeduplicationLevel MapDedupLevel(RemoveDuplicate removeDuplicate)
        {
            switch (removeDuplicate)
            {
                case RemoveDuplicate.None:
                    return DeduplicationLevel.Off;
                case RemoveDuplicate.Medium:
                    return DeduplicationLevel.Similar;
                case RemoveDuplicate.High:
                    return DeduplicationLevel.VirtuallyIdentical;
                default:
                    return DeduplicationLevel.Off;
            }
        }

        private string GetStructuredSearchQuery(SimpleAlertRequest request, IPreferences preferences)
        {
            string strSerializedQuery;
            var query = BuildQuery(request, preferences);
            Stream objStream;
            var searchRequest = new PerformContentSearchRequest
                                    {
                                        StructuredSearch = new StructuredSearch {Query = query}
                                    };
            searchRequest.Serialize(out objStream);
            using (var reader = new StreamReader(objStream))
            {
                strSerializedQuery = reader.ReadToEnd();
            }
            strSerializedQuery = strSerializedQuery.Substring(strSerializedQuery.IndexOf("<structuredSearch"));
            strSerializedQuery = strSerializedQuery.Replace("</PerformContentSearch></soap-env:Body></soap-env:Envelope>", "");
            const string Ver = " version=\"2.3\"";
            strSerializedQuery = strSerializedQuery.Replace(Ver, "");
            return strSerializedQuery;
        }

        private StructuredQuery BuildQuery(SimpleAlertRequest request, IPreferences preferences)
        {
            var query = new StructuredQuery();

            var searchStringList = new List<SearchString>();


            if (!string.IsNullOrEmpty(request.SearchText))
                searchStringList.Add(SearchFilterUtility.GetFreeText(request.SearchText));

            //add the filters to the search string collection
            if (request.NewsFilter != null)
            {
                searchStringList.AddRange(SearchFilterUtility.ProcessNewsFilters(_newsFilters, _controlData));
            }

            query.SearchStringCollection.AddRange(searchStringList);

            SearchSourceGroupPreferenceItem ssgpItem = null;

            if (request.SelectedSources != null && request.SelectedSources.SourceList != null)
            {
                ssgpItem = request.SelectedSources.SourceList;
            }
            else if (request.SelectedSources != null && request.SelectedSources.Source != null)
            {
                ssgpItem = new SearchSourceGroupPreferenceItem { Value = new SourceGroup() };
                foreach (var source in (request.SelectedSources.Source))
                {
                    ssgpItem.Value.Add(source);
                }
            }

            /* if (ssgpItem != null)
            {
                query.SearchStringCollection.AddRange(SearchUtility.BuildSearchStringCollection(ssgpItem));

                if (ssgpItem.Value != null)
                {
                    var ssc = SearchUtility.GetAvailableContentCategories(ssgpItem);
                    var filteredSSC = ssc.Where(x => x == SearchCollection.Publications || x == SearchCollection.WebSites);
                    query.SearchCollectionCollection.AddRange(filteredSSC);

                    if ((filteredSSC.Count() > 0 && ssc[0] == SearchCollection.Publications)
                        && !query.SearchCollectionCollection.Contains(SearchCollection.Publications))
                        query.SearchCollectionCollection.Add(SearchCollection.Publications);
                    if ((filteredSSC.Count() > 0 && ssc[0] == SearchCollection.WebSites)
                        && !query.SearchCollectionCollection.Contains(SearchCollection.WebSites))
                        query.SearchCollectionCollection.Add(SearchCollection.WebSites);
                    if (filteredSSC.Count() == 0)
                    {
                        query.SearchCollectionCollection = new SearchCollectionCollection { SearchCollection.Publications, SearchCollection.WebSites };
                    }
                }
                else
                {
                    query.SearchCollectionCollection = new SearchCollectionCollection { SearchCollection.Publications, SearchCollection.WebSites };
                }
            }
            else
            {
                query.SearchCollectionCollection = new SearchCollectionCollection { SearchCollection.Publications, SearchCollection.WebSites };
            } */

            //Source browser cleanup - Always create alert for all valid categories.
            query.SearchStringCollection.AddRange(SearchUtility.BuildSourceSearchStringCollection(ssgpItem, true));
            query.SearchCollectionCollection = new SearchCollectionCollection { 
                                                                                SearchCollection.Publications, 
                                                                                SearchCollection.WebSites, 
                                                                                SearchCollection.Blogs
                                                                            };
            //Source browser cleanup - Always create alert for all valid categories.

            if (preferences.ContentLanguages != null && preferences.ContentLanguages.Count > 0)
            {
                query.SearchStringCollection.Add(SearchFilterUtility.GetLanguageFilter(String.Join(" ", preferences.ContentLanguages.ToArray())));
            }

            return query;

        }

        private TrackFilterItem[] GetFilterItem(IEnumerable<Utilities.Search.Core.FilterItem> newsFilter)
        {
            var lstFilters = new List<TrackFilterItem>();
            foreach (var filter in newsFilter)
            {
                lstFilters.Add(new TrackFilterItem { code = filter.Code, name = filter.Desc });
            }
            return lstFilters.ToArray();
        }

        private TrackFilterSourceItem[] GetFilterSourceItem(IEnumerable<Utilities.Search.Core.FilterItem> newsFilter)
        {
            var lstFilters = new List<TrackFilterSourceItem>();
            foreach (var filter in newsFilter)
            {
                lstFilters.Add(new TrackFilterSourceItem { code = filter.Code, name = filter.Desc, type = filter.Type });
            }
            return lstFilters.ToArray();
        }

        private string[] GetKeywordFilterItem(IEnumerable<Utilities.Search.Core.FilterItem> newsFilter)
        {
            var lstFilters = new List<string>();
            foreach (var filter in newsFilter)
            {
                lstFilters.Add(filter.Code);
            }
            return lstFilters.ToArray();
        }

        public string GetSourceListStr(SearchSourceGroupPreferenceItem ssgPItem)
        {

            //For AllXXXXXXX category string should be -> P|;W|;I|
            if (ssgPItem == null)
                return null;

            var sourceGroup = ssgPItem.Value;

            if (sourceGroup == null || sourceGroup.IsAllSourceTypesSelected)
                return "";

            var sb = new StringBuilder();

            var itemId = GetItemId(ssgPItem);
            if (!string.IsNullOrEmpty(itemId))
            {
                AppendFiiListDescString(ref sb, "G|" + itemId);
            }
            else
            {
                if (sourceGroup.Count > 0)
                {

                    var allSourceCode = string.Empty;
                    foreach (var sourceList in sourceGroup)
                    {
                        if (IsNull(sourceList))
                        {
                            continue;
                        }

                        switch (sourceList.Type)
                        {
                            case SourceType.Publication:
                                allSourceCode = SearchUtility.ALL_PUBLICATIONS_CODE;
                                break;
                            case SourceType.Multimedia:
                                allSourceCode = SearchUtility.ALL_MULTIMEDIAS_CODE;
                                break;
                            case SourceType.Website:
                                allSourceCode = SearchUtility.ALL_WEBSITES_CODE;
                                break;
                            case SourceType.Picture:
                                allSourceCode = SearchUtility.ALL_PICTURES_CODE;
                                break;
                            case SourceType.Blogs:
                                allSourceCode = SearchUtility.ALL_BLOGS_CODE;
                                break;
                            case SourceType.Boards:
                                allSourceCode = SearchUtility.ALL_BOARDS_CODE;
                                break;
                            case SourceType.Internal:
                                allSourceCode = SearchUtility.ALL_INTERNAL_CODE;
                                break;
                        }
                        if (sourceList.IsAllSourcesSelected && !IsOnlyExcluded(sourceList))
                        {
                            AppendFiiListDescString(ref sb, allSourceCode);
                        }
                        else
                        {
                            AppendFiiListDescString(ref sb, GetSourceListString(sourceList));
                        }
                    }
                }

            }

            return sb.ToString();
        }

        public string GetSourceListStr(List<Source> lstSource)
        {
            if (lstSource == null)
                return null;

            var sb = new StringBuilder();
            foreach (var source in lstSource)
            {
                AppendFiiListDescString(ref sb, GetSourceListString(source));
            }

            return sb.ToString();
        }

        public string GetSourceListDescStr(SearchSourceGroupPreferenceItem ssgPItem)
        {
            if (ssgPItem == null)
                return null;

            var sourceGroup = ssgPItem.Value;

            if (sourceGroup == null || sourceGroup.IsAllSourceTypesSelected)
                return "";

            var sb = new StringBuilder();
            var itemId = GetItemId(ssgPItem);
            if (itemId != null)
            {
                AppendFiiListDescString(ref sb, String.Format("sc_g_{0},{1}", itemId, ssgPItem.ItemName));
            }
            else
            {
                if (sourceGroup.Count > 0)
                {
                    foreach (var sourceList in sourceGroup)
                    {
                        AppendFiiListDescString(ref sb, GetSourceListDescString(sourceList));
                    }
                }
            }
            return sb.ToString();
        }

        public string GetSourceListDescStr(List<Source> lstSource)
        {
            if (lstSource == null)
                return null;

            var sb = new StringBuilder();
            foreach (var source in lstSource)
            {
                AppendFiiListDescString(ref sb, GetSourceListDescString(source));
            }

            return sb.ToString();
        }

        private bool IsOnlyExcluded(SourceList sourceList)
        {
            return (sourceList != null && (sourceList.CodeIncluded == null || sourceList.CodeIncluded.Count == 0) && (sourceList.CodeExcluded != null && sourceList.CodeExcluded.Count > 0));
        }

        private bool IsNull(SourceList sourceList)
        {
            return (sourceList == null
                || ((sourceList.CodeExcluded == null || sourceList.CodeExcluded.Count == 0)
                    && (sourceList.CodeIncluded == null || sourceList.CodeIncluded.Count == 0)));
        }

        private void AppendFiiListDescString(ref StringBuilder sb, string str2)
        {
            if (!string.IsNullOrEmpty(str2))
            {
                if (sb.Length > 0)
                {
                    sb.Append(";");
                }
                sb.Append(str2);
            }
        }

        private string GetItemId(PreferenceItem preferenceItem)
        {
            if (preferenceItem != null && !string.IsNullOrEmpty(preferenceItem.ItemID))
            {
                if (preferenceItem.ItemInfo != null && preferenceItem.ItemInfo.ItemType == PreferenceScope.GROUP)
                    return "G" + preferenceItem.ItemID;
                return preferenceItem.ItemID;
            }
            return null;
        }

        private string GetSourceListString(SourceList sourceList)
        {
            if (sourceList == null)
                return null;

            var prefix = ((XmlEnumAttribute)Attribute.GetCustomAttribute(typeof(SourceType).GetField(sourceList.Type.ToString()), typeof(XmlEnumAttribute))).Name;

            var sb = new StringBuilder();
            sb.Append(prefix);
            sb.Append("|");

            if (sourceList.CodeIncluded != null)
            {
                foreach (var code in sourceList.CodeIncluded)
                {
                    if (IsValidSourceCode(code))
                    {
                        sb.Append(code);
                        sb.Append(",");
                    }
                }
            }
            if (sourceList.CodeExcluded != null)
            {
                foreach (var code in sourceList.CodeExcluded)
                {
                    if (IsValidSourceCode(code))
                    {
                        sb.Append("~");
                        sb.Append(code);
                        sb.Append(",");
                    }
                }
            }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        private string GetSourceListDescString(SourceList sourceList)
        {
            if (sourceList == null)
                return null;

            var prefix = ((XmlEnumAttribute)Attribute.GetCustomAttribute(typeof(SourceType).GetField(sourceList.Type.ToString()), typeof(XmlEnumAttribute))).Name;

            var sb = new StringBuilder();
            string sourceName = null;
            if (sourceList.GetType() == typeof(Source))
            {
                sourceName = ((Source)sourceList).Name;
            }
            AppendFiiListDescString(ref sb, GetSourceListDescString(sourceList.CodeIncluded, prefix, sourceName));
            AppendFiiListDescString(ref sb, GetSourceListDescString(sourceList.CodeExcluded, prefix, sourceName));
            return sb.ToString();
        }

        private string GetSourceListDescString(CodeList codeList, string prefix, string sourceName)
        {
            if (codeList == null)
                return null;
            var sb = new StringBuilder();
            foreach (var code in codeList)
            {
                if (IsValidSourceCode(code))
                {
                    sb.Append("sc_");
                    sb.Append(prefix.ToLower());
                    sb.Append("_");
                    sb.Append(code);
                    sb.Append(",");
                    sb.Append(sourceName);
                }
                sb.Append(";");
            }
            if (sb.Length > 0)
            {
                if (sb[sb.Length - 1] == ';')
                {
                    sb.Remove(sb.Length - 1, 1);
                }
            }
            return sb.ToString();
        }

        private bool IsValidSourceCode(string code)
        {
            return !(code == null || SearchUtility.IsUiSourceCode(code));
        }

        /*
        private string GetFiiList(SearchCriteria criteria)
        {
            StringBuilder sb = new StringBuilder();
            AppendFiiListString(ref sb, GetFIIListString(criteria.AuthorCriteria, "au"));
            AppendFiiListString(ref sb, GetFIIListString(criteria.CompanyCriteria, "fds"));
            AppendFiiListString(ref sb, GetFIIListString(criteria.IndustryCriteria, "in"));
            AppendFiiListString(ref sb, GetFIIListString(criteria.RegionCriteria, "re"));
            AppendFiiListString(ref sb, GetFIIListString(criteria.NewsSubjectCriteria, "ns"));
            return sb.ToString();
        }

        private string GetFiiListName(SearchCriteria criteria)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetFIIListDescString(criteria.AuthorCriteria, "au"));
            AppendFiiListDescString(ref sb, GetFIIListDescString(criteria.CompanyCriteria, "co"));
            AppendFiiListDescString(ref sb, GetFIIListDescString(criteria.IndustryCriteria, "in"));
            AppendFiiListDescString(ref sb, GetFIIListDescString(criteria.RegionCriteria, "re"));
            AppendFiiListDescString(ref sb, GetFIIListDescString(criteria.NewsSubjectCriteria, "ns"));
            return sb.ToString();
        }

        private void AppendFiiListString(ref StringBuilder sb, string str2)
        {
            if (str2 != null && str2.Length > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(";");
                }
                if (str2[0] == '~')
                {
                    //If all are not code then make main operator to not and rest to or
                    str2 = str2.Substring(1);
                    str2 = str2.Replace("~", "|");
                    sb.Append("~");
                }
                else
                {
                    sb.Append("&");
                }
                sb.Append("!");
                sb.Append(str2);
            }
        }

        private string GetFIIListString(FIICriteria criteria, string prefix)
        {
            if (criteria == null)
                return null;
            StringBuilder sb = new StringBuilder();
            string ope = (criteria.IncludedCodeOperator == FIICodeOperator.And) ? "&" : "|";
            sb.Append(StringJoin(criteria.Included, prefix, ope));
            string str = StringJoin(criteria.Excluded, prefix, "~");
            if (str != null && str.Length > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append(str);
            }
            if (sb.Length > 0 && (sb[0] == '&' || sb[0] == '|'))
            {
                sb.Remove(0, 1);
            }
            return sb.ToString();
        }

        private string GetFIIListDescString(FIICriteria criteria, string prefix)
        {
            if (criteria == null)
                return null;
            StringBuilder sb = new StringBuilder();
            AppendFiiListDescString(ref sb, GetFIIListDescString(criteria.Included, prefix));
            AppendFiiListDescString(ref sb, GetFIIListDescString(criteria.Excluded, prefix));
            return sb.ToString();
        }

        private string GetFIIListDescString(CriteriaInfo[] criteria, string prefix)
        {
            if (criteria == null)
                return null;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < criteria.Length; i++)
            {
                CriteriaInfo criteriaInfo = criteria[i];
                if (criteriaInfo.Code != null)
                {
                    sb.Append(prefix);
                    sb.Append("_");
                    sb.Append(criteriaInfo.Code);
                    sb.Append(",");
                    sb.Append(criteriaInfo.Name);
                }
                else if (criteriaInfo.PreferenceItem != null)
                {
                    string itemId = GetItemId(criteriaInfo.PreferenceItem);
                    if (itemId != null)
                    {
                        sb.Append(itemId);
                        sb.Append(",");
                        sb.Append(criteriaInfo.PreferenceItem.InstanceName);
                    }
                }
                if (i != criteria.Length - 1)
                {
                    sb.Append(";");
                }
            }
            return sb.ToString();
        }
         * */
    }
}
