using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using EMG.Utility.Exceptions;
using EMG.Utility.Managers.CacheService;
using EMG.Utility.Search.SearchBuilder;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Utils.V1_0;
using log4net;
using SearchCollection=EMG.Utility.Search.SearchBuilder.SearchCollection;

namespace EMG.Utility.Managers.AccessionSearch
{

    public class AccessionSearchManager
    {
        private static readonly ILog m_Log = LogManager.GetLogger(typeof(CacheManager));
        private static readonly AccessionSearchManager m_Instance = new AccessionSearchManager();

        public static AccessionSearchManager Instance
        {
            get { return m_Instance; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessionSearchManager"/> class.
        /// </summary>
        private AccessionSearchManager()
        {
        }

        public PerformContentSearchResponse PerformSearch(ControlData controlData, AccessionNumberSearchRequest accNumSearchRequest)
        {
            try
            {
                //Always go to search 2.0 1st
                //Build the Content Search Request
                AccessionNumberContentSearch _accessionNumberSearch = new AccessionNumberContentSearch();
                ArrayList contentTypeList = new ArrayList();

                _accessionNumberSearch.AccessionNumbers = new string[accNumSearchRequest.Articles.Length];
                for (int i = 0; i < accNumSearchRequest.Articles.Length; i++)
                {
                    _accessionNumberSearch.AccessionNumbers[i] = accNumSearchRequest.Articles[i].AccessionNumber;
                    //add content type to the content type list
                    if (!contentTypeList.Contains(accNumSearchRequest.Articles[i].ContentType))
                        contentTypeList.Add(accNumSearchRequest.Articles[i].ContentType);
                }
                _accessionNumberSearch.Formatting = new FormattingController();
                _accessionNumberSearch.Formatting.SortOrder = ResultSortOrder.PublicationDateReverseChronological;  //default
                SearchBuilderManager sbManager = new SearchBuilderManager(_accessionNumberSearch);
                SearchBuilderResponse _sbResponse = sbManager.PerformSearch(controlData);
                //If not all articles returns
                if (_sbResponse.SearchResponse.ContentSearchResult.HitCount != accNumSearchRequest.Articles.Length)
                {
                    //Build the Index Search Request
                    //If any remain articles's content type is publication
                    //Build Index Search request with those publication articles with the content type set to "publication"
                    //If any remain articles's content type is picture
                    //Build Index Search request with those picture articles with the content type set to "picture"
                    //Not done yet!!!!!!!!!
                }
                //Combine the Content Search Response with the Index Search Response(s)
                //Sort the response as the way client defined before.
                //Not done yet!!!!!!!!!
            }
            catch (Exception ex)
            {
                throw new EMGUtilitiesException("Exception while doing accession number search.", ex.InnerException);
            }
            PerformContentSearchResponse searchResponse = new PerformContentSearchResponse();
            return searchResponse;
        }
    }
}
