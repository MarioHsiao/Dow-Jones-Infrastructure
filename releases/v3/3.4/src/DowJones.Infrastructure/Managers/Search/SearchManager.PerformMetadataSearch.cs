using System;
using System.Linq;
using System.Collections.Generic;
using DowJones.Exceptions;
using DowJones.Search;
using DowJones.Search.Core;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.Search
{
    /// <summary>
    /// Service to get the code descriptor.
    /// </summary>
    public partial class SearchManager
    {
        public Dictionary<NewsFilterCategory, Dictionary<string, string>> GetDescriptorsByCode(
            Dictionary<NewsFilterCategory, List<string>> CodesDict)
        {
            if (CodesDict != null && CodesDict.Count > 0)
            {
                #region ---- Build the request ----

                var request = new PerformMetadataSearchRequest
                                  {
                                      FirstResult = 0,
                                      MaxResults = 999,
                                      StructuredSearch = new MetadataStructuredSearch
                                                             {
                                                                 Query = new MetadataStructuredQuery
                                                                             {
                                                                                 SearchStringCollection =
                                                                                     new MetadataSearchStringCollection()
                                                                             }
                                                             }
                                  };

                request.StructuredSearch.Query.ResultLanguage = _preferences.InterfaceLanguage;
                var fcodes = new List<string>();
                foreach (var keyValuePair in CodesDict)
                {
                    request.StructuredSearch.Query.MetadataCollectionCollection.Add(MapNewsFilterCategoryToMetadataType(keyValuePair.Key));
                    if (keyValuePair.Key == NewsFilterCategory.Source)
                    {
                        request.StructuredSearch.Query.MetadataCollectionCollection.Add(MetadataCollection.Group);
                    }
                    fcodes.AddRange(keyValuePair.Value);
                }
                request.StructuredSearch.Query.SearchStringCollection.Add(new MetadataSearchString
                                                                              {
                                                                                  Mode = MetadataSearchMode.Any,
                                                                                  Scope = "fcode",
                                                                                  Value = String.Join(" ", fcodes)
                                                                              });
                #endregion

                #region ---- Build the response ----

                PerformMetadataSearchResponse response = GetPerformMetadataSearchResponse(request);

                if (response != null
                    && response.MetadataSearchResult != null
                    && response.MetadataSearchResult.MetadataResultSet != null
                    && response.MetadataSearchResult.MetadataResultSet.MetadataInfoCollection != null
                    && response.MetadataSearchResult.MetadataResultSet.MetadataInfoCollection.Count > 0)
                {
                    var dict = new Dictionary<NewsFilterCategory, Dictionary<string, string>>();
                    foreach (var keyValuePair in CodesDict)
                    {
                        foreach (var code in keyValuePair.Value)
                        {
                            var metadataInfo = GetMetadataInfoByType(response.MetadataSearchResult.MetadataResultSet.MetadataInfoCollection, keyValuePair.Key, code);
                            if (!dict.ContainsKey(keyValuePair.Key))
                            {
                                dict[keyValuePair.Key] = new Dictionary<string, string>();
                            }

                            if (metadataInfo != null)
                            {
                                dict[keyValuePair.Key].Add(metadataInfo.Id.ToLower(), metadataInfo.Name);
                            }
                        }
                    }
                    return dict;
                }

                #endregion

                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptyFCodeCollection);
            }
            return null;
        }

        private MetadataInfo GetMetadataInfoByType(MetadataInfoCollection metadataInfoCollection, NewsFilterCategory category, string code)
        {
            return metadataInfoCollection.Where(m => m.Collection == MapNewsFilterCategoryToMetadataType(category) && m.Id.ToLower() == code.ToLower()).FirstOrDefault();
        }

        private MetadataCollection MapNewsFilterCategoryToMetadataType(NewsFilterCategory key)
        {
            switch (key)
            {
                case NewsFilterCategory.Author:
                    return MetadataCollection.Author;
                case NewsFilterCategory.Company:
                    return MetadataCollection.Company;
                case NewsFilterCategory.Executive:
                    return MetadataCollection.People;
                case NewsFilterCategory.Industry:
                    return MetadataCollection.Industry;
                case NewsFilterCategory.Region:
                    return MetadataCollection.Region;
                case NewsFilterCategory.Source:
                    return MetadataCollection.Source;
                case NewsFilterCategory.Subject:
                    return MetadataCollection.NewsSubject;
            }
            throw new NotSupportedException("Not supported key:" + key);
        }
    }
}