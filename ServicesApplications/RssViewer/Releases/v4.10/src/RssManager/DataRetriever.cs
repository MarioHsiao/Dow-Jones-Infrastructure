using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Xsl;
using Data;
using EMG.Utility.Formatters.Globalization;
using EMG.Utility.Managers.Search.Requests;
using EMG.Utility.Managers.Search.Responses;
using EMG.Utility.Managers.CacheService;
using Factiva.BusinessLayerLogic.Exceptions;
using Factiva.BusinessLayerLogic.Managers.V2_0;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Messages.Assets.Newsletter.V1_0;
using Factiva.Gateway.Messages.Assets.Workspaces.V2_0;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Track.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;
using FactivaRssManager;
using FCMLib;
using Common_V2 = Factiva.Gateway.Messages.Assets.Common.V2_0;
using SearchManager=EMG.Utility.Managers.Search.SearchManager;
using EMG.Utility.Managers.Track;
using PerformContentSearchRequest = Factiva.Gateway.Messages.Search.V2_0.PerformContentSearchRequest;
using SortBy = EMG.Utility.Managers.Search.Requests.SortBy;

//using Factiva.Gateway

//using Factiva.Gateway.

//using Backend = Factiva.Gateway.Messages.Assets.Newsletter.V1_0;

namespace FactivaRssManager_2_0
{
    /// <summary>
    /// Summary description for Data Retriever.
    /// </summary>
    public class DataRetriever : FactivaRssManager.DataRetriever
    {
        //convert to get and set later
        public DataRetriever(string remoteHost)
            : base(remoteHost)
        {
        }

        public string getSearch2Headlines(InputData inputData, ConfigData configData)
        {
            return getSearch2Headlines(inputData, configData, false);
        }

        public string getSearch2Headlines(InputData inputData, ConfigData configData, bool getSearchCollectionsAssets)
        {
            Log(Logger.Level.INFO, "FCL::getSearch2Headlines inputData and ConfigData ");
            try
            {
                var cacheTrackFoldersRss = "";
                var strvalidate = configData.getItem("//validate/eid/params/param");
                string strHashkey = null;

                // [10.14.10] - avoid call to track if folder has been deleted
                var controlData = new ControlData
                {
                    UserID = configData.getItem("//transactionParams/lwUserID"),
                    UserPassword = configData.getItem("//transactionParams/lwPassword"),
                    ProductID = configData.getItem("//transactionParams/lwNameSpace")
                };

                var deletedFolders = new TrackDeletedFoldersCacheManager(controlData.Clone(), "en").GetDeletedFolders();
                if (deletedFolders != null &&
                    deletedFolders.Count > 0 &&
                    deletedFolders.ContainsKey(inputData.getItem(strvalidate)))
                {
                    return cacheTrackFoldersRss;
                }

                controlData.ClientType = configData.getItem("//transactionParams/ClientTypeCode");
                controlData.AccessPointCode = configData.getItem("//transactionParams/AccessPointCode");
                controlData.ProxyUserID = inputData.getItem("userid");
                controlData.ProxyUserNamespace = inputData.getItem("namespace");

                if (inputData.getItem("hkey") != null)
                {
                    strHashkey = inputData.getItem("hkey");
                }
                // check platform cache next for fault tolerance
                var strCacheID = string.Format("{0}_{1}_{2}{3}",
                                               inputData.getItem(strvalidate),
                                               inputData.getItem("userid"),
                                               inputData.getItem("namespace"),
                                               string.IsNullOrEmpty(strHashkey) ? "_E" : "_" + strHashkey.Substring(0, 1));
                cacheTrackFoldersRss = GetCacheData(strCacheID, 6); //check cache for xml data
                if (cacheTrackFoldersRss == "")
                {
                    // NN - 05/21/2012 switched to using Gateway instead of FCS
                    var request = new GetAlertHeadlinesForRss2Request
                    {
                        bResetSessionMark = false,
                        bPreviewHeadlines = true,
                        viewType = HeadlinesViewType.ViewAll,
                        
                        folderRequestItems = new[]
                        {
                            new FolderRequestItem
                            {
                                folderId = int.Parse(inputData.getItem(strvalidate)),
                                useFolderDedup = true,          
                            }
                        },
                        searchQuery = new PerformContentSearchRequest
                        {
                            FirstResult = 0,
                            MaxResults = int.Parse(configData.getItem("//transactionParams/numberOfHeadlines")),
                            StructuredSearch = new StructuredSearch
                            {
                                Formatting = new ResultFormatting
                                {
                                    SortOrder = ResultSortOrder.ArrivalTime
                                },
                                Query = new StructuredQuery
                                {                                    
                                    SearchCollectionCollection = new SearchCollectionCollection
                                    {
                                        SearchCollection.Publications,
                                        SearchCollection.WebSites,
                                        SearchCollection.Blogs
                                    }
                                },
                            }
                        }
                    };
                    if (getSearchCollectionsAssets)
                    {
                        request.searchQuery.StructuredSearch.Query.SearchCollectionCollection.Add(SearchCollection.Assets);
                    }
                    
                    if (!string.IsNullOrEmpty(strHashkey))
                    {
                        request.hashKeys = new [] {strHashkey};
                    }

                    var serviceResponse = FactivaServices.Invoke<GetAlertHeadlinesForRss2Response>(controlData, request);

                    if (serviceResponse == null)
                    {
                        throw new FactivaBusinessLogicException(new NullReferenceException("ServiceResponse is null"), -1);
                    }
                    if (serviceResponse.rc != 0)
                    {
                        throw new FactivaBusinessLogicException(serviceResponse.rc);
                    }

                    if (serviceResponse.ObjectResponse != null)
                    {
                        var response = serviceResponse.ObjectResponse;
                        if (response.folderHeadlinesResponse != null &&
                            response.folderHeadlinesResponse.folderHeadlinesResult != null &&
                            response.folderHeadlinesResponse.folderHeadlinesResult.folder != null &&
                            response.folderHeadlinesResponse.folderHeadlinesResult.folder.Length > 0)
                        {
                            var folder = response.folderHeadlinesResponse.folderHeadlinesResult.folder[0];
                            var headlineInfo = new HeadlineInfo
                                                   {
                                                       documentList = new documentCollection()
                                                   };
                            if (folder.status == 0)
                            {
                                if (folder.PerformContentSearchResponse != null)
                                {
                                    var searchResponse = folder.PerformContentSearchResponse;
                                    var duplicateIndices = GetDuplicateIndices(searchResponse);
                                    for (var index = 0; index < searchResponse.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection.Count; index++)
                                    {

                                        if (duplicateIndices.Contains(index))
                                            continue;
                                        var contentHeadline = searchResponse.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection[index];
                                        var document = new document();
                                        PopulateDocument(document, contentHeadline);
                                        headlineInfo.documentList.Add(document);
                                    }
                                    cacheTrackFoldersRss = AddCacheData(serialize(headlineInfo), strCacheID, 6);
                                }
                            }
                            else
                            {
                                var strErrorCode = ConfigurationSettings.AppSettings["OLHTrackErrorCodes"].Split(',');
                                //var intCacheTime = Convert.ToInt32(ConfigurationSettings.AppSettings["CacheTime"]);
                                if (strErrorCode.Any(errorCode => folder.status == Convert.ToInt32(errorCode)))
                                {
                                    AddCacheData(serialize(headlineInfo), strCacheID, 6);
                                }
                                throw new FactivaBusinessLogicException(string.Format("Folder status is not 0 ({0})", folder.status), folder.status);
                            }
                        }
                    }
                }
                return cacheTrackFoldersRss;
            }
            catch (Exception e)
            {
                Log(Logger.Level.ERROR, "FCL::getSearch2Headlines Error - " + e);
                throw;
            }
            #region Old code using fcs
            /*
            try
            {
                SortedList control = new SortedList();
                SortedList request = new SortedList();
                string _strHashkey = string.Empty;
                ;


                string strvalidate = configData.getItem("//validate/eid/params/param");


                control.Add("FCS_CD_NETWORK_PARTNER_ID", "");
                control.Add("FCS_CD_CLIENT_TYPE", configData.getItem("//transactionParams/ClientTypeCode"));
                control.Add("FCS_CD_ACCESS_POINT_CODE", configData.getItem("//transactionParams/AccessPointCode"));
                control.Add("FCS_CD_USER_ID", configData.getItem("//transactionParams/lwUserID"));
                control.Add("FCS_CD_USER_PASSWORD", configData.getItem("//transactionParams/lwPassword"));
                control.Add("FCS_CD_PRODUCT_ID", configData.getItem("//transactionParams/lwNameSpace"));
                control.Add("FCS_CD_IP_ADDRESS", _remoteHost);
                control.Add("FSS_PROXY_USER_ID", inputData.getItem("uid"));
                control.Add("FSS_PROXY_NAMESPACE", inputData.getItem("namespace"));
                if (inputData.getItem("hkey") != null)
                {
                    _strHashkey = inputData.getItem("hkey");
                    request.Add("hashKey", inputData.getItem("hkey"));
                }

                request.Add("FolderID", inputData.getItem(strvalidate));
                request.Add("HeadlineFormat", "headline");
                request.Add("HeadlineCount", configData.getItem("//transactionParams/numberOfHeadlines"));
                request.Add("BookMark", "");
                request.Add("SessionMark", "");
                request.Add("ViewType", "all");
                request.Add("ResetSessionMark", "no");
                request.Add("SortBy", "deliverydate");
                request.Add("SortOrder", "descending");

                request.Add("Preview", "yes");
                request.Add("ProgressDisclose", "false");
                request.Add("ChunkSize", "99");
                request.Add("ContentType", contentType);
                request.Add("PostStatus", "");
                request.Add("UseFolderDedup", "yes");

                string xmlRequest = "";
                string CacheTrackFoldersRss = "";
                
                // first check track deleted folders
                var cData = new ControlData
                {
                    UserID = configData.getItem("//transactionParams/lwUserID"),
                    UserPassword = configData.getItem("//transactionParams/lwPassword"),
                    ProductID = configData.getItem("//transactionParams/lwNameSpace")
                };
                var trackMgr = new TrackDeletedFoldersCacheManager(cData.Clone(), "en");
                var deletedFolders = trackMgr.GetDeletedFolders();

                // [10.14.10] - avoid call to track if folder has been deleted
                if (deletedFolders != null &&
                    deletedFolders.Count > 0 &&
                    deletedFolders.ContainsKey(inputData.getItem(strvalidate)))
                {
                    return CacheTrackFoldersRss;
                }
                else
                {
                    // check platform cache next for fault tolerance
                    string _strCacheID = string.Format("{0}_{1}_{2}{3}", inputData.getItem(strvalidate),
                                                       inputData.getItem("uid"), inputData.getItem("namespace"),
                                                       _strHashkey != "" ? "_" + _strHashkey.Substring(0, 1) : "_E");
                    CacheTrackFoldersRss = GetCacheData(_strCacheID, 6); //check cache for xml data
                    if (CacheTrackFoldersRss == "")
                    {
                        xmlRequest = createXMLRequest("CLIP_OLH_RSS_HEADLINE_2", "", control, request);
                        Log(Logger.Level.INFO, "getSearch2Headlines [XML REQUEST] " + xmlRequest);

                        // otherwise make track call
                        ContentManagerClass fcm = new ContentManagerClass();
                        string _xmlResponse = fcm.XMLQueryString(xmlRequest, "", "");
                        Log(Logger.Level.INFO, " getSearch2Headlines [XML RESPONSE]:  " + _xmlResponse);
                        int _statusLevel = 0;
                        if (ckeckFolderStatus(_xmlResponse, ref _statusLevel))
                            CacheTrackFoldersRss = AddCacheData(_xmlResponse, _strCacheID, 6);
                                // add cache to database
                        else
                        {
                            for (int i = 0; i < _strErrorCode.Length; i++)
                            {
                                if (_statusLevel == Convert.ToInt32(_strErrorCode[i]))
                                {
                                    CacheTrackFoldersRss = AddCacheData(_xmlResponse, _strCacheID, 6, _intCacheTime);
                                        // add cache to database
                                    break;
                                }
                            }
                        }
                    }
                    return CacheTrackFoldersRss;
                }
            }
            catch (Exception e)
            {
                Log(Logger.Level.ERROR, "FCL::getFolderHeadlines Error - " + e);
                throw e;
            }
             */
            #endregion
        }

        private static void PopulateDocument(document inputDocument, ContentHeadline contentHeadline)
        {
            inputDocument.accessionNumber = contentHeadline.AccessionNo;
            inputDocument.publicationDate = contentHeadline.PublicationDate;
            inputDocument.words = contentHeadline.WordCount;

            if (contentHeadline.PublicationTime > DateTime.MinValue)
                inputDocument.publicationDate = DateTimeFormatter.Merge(contentHeadline.PublicationDate, contentHeadline.PublicationTime);

            if (contentHeadline.Headline != null && contentHeadline.Headline.Any != null)
                inputDocument.headline = contentHeadline.Headline.Any[0].InnerText;

            if (contentHeadline.SourceCode != null)
                inputDocument.sourceCode = contentHeadline.SourceCode;

            if (contentHeadline.SourceName != null)
                inputDocument.sourceName = contentHeadline.SourceName;

            if (contentHeadline.Snippet != null && contentHeadline.Snippet.Any != null)
                inputDocument.snippet = contentHeadline.Snippet.Any[0].InnerText;

            if (contentHeadline.ContentItems != null)
                inputDocument.category = contentHeadline.ContentItems.ContentType;

            if (contentHeadline.BaseLanguage != null)
                inputDocument.language = contentHeadline.BaseLanguage;

            if (contentHeadline.ContentItems != null)
                inputDocument.reference = contentHeadline.ContentItems.PrimaryRef;
        }

        private static List<int> GetDuplicateIndices(PerformContentSearchResponse searchResponse)
        {
            var duplicateIndices = new List<int>();
            if (searchResponse.ContentSearchResult.DeduplicatedHeadlineSet != null &&
                searchResponse.ContentSearchResult.DeduplicatedHeadlineSet.HeadlineRefCollection != null)
            {
                foreach (var headlineRef in searchResponse.ContentSearchResult.DeduplicatedHeadlineSet.HeadlineRefCollection)
                {
                    if (headlineRef.Duplicates != null && headlineRef.Duplicates.DuplicateRefCollection != null)
                    {
                        foreach (var duplicateRef in headlineRef.Duplicates.DuplicateRefCollection)
                            duplicateIndices.Add(duplicateRef.Index);
                    }
                }
            }
            return duplicateIndices;
        }

        //public string getFolderHeadlines(InputData inputData, ConfigData configData, string contentType)
        //{
        //    Log(Logger.Level.INFO, "FCL::getFolderHeadlines inputData and ConfigData ");
        //    XmlDocument xmlDoc = new XmlDocument();

        //    int _intCacheTime = Convert.ToInt32(ConfigurationSettings.AppSettings["CacheTime"]);
        //    string[] _strErrorCode = ConfigurationSettings.AppSettings["OLHTrackErrorCodes"].Split(',');
        //    try
        //    {
        //        SortedList control = new SortedList();
        //        SortedList request = new SortedList();
        //        string xmlRequest = "";
        //        string _xmlResponse = "";
        //        string _strHashkey = "";


        //        string strvalidate = configData.getItem("//validate/eid/params/param");


        //        control.Add("FCS_CD_NETWORK_PARTNER_ID", "");
        //        control.Add("FCS_CD_CLIENT_TYPE", configData.getItem("//transactionParams/ClientTypeCode"));
        //        control.Add("FCS_CD_ACCESS_POINT_CODE", configData.getItem("//transactionParams/AccessPointCode"));
        //        control.Add("FCS_CD_USER_ID", configData.getItem("//transactionParams/lwUserID"));
        //        control.Add("FCS_CD_USER_PASSWORD", configData.getItem("//transactionParams/lwPassword"));
        //        control.Add("FCS_CD_PRODUCT_ID", configData.getItem("//transactionParams/lwNameSpace"));
        //        control.Add("FCS_CD_IP_ADDRESS", base._remoteHost);
        //        control.Add("FSS_PROXY_USER_ID", inputData.getItem("userID"));
        //        control.Add("FSS_PROXY_NAMESPACE", inputData.getItem("namespace"));
        //        if (inputData.getItem("hkey") != null)
        //        {
        //            _strHashkey = inputData.getItem("hkey");
        //            request.Add("hashKey", inputData.getItem("hkey"));
        //        }

        //        request.Add("FolderID", inputData.getItem(strvalidate));
        //        request.Add("HeadlineFormat", "headline");
        //        request.Add("HeadlineCount", configData.getItem("//transactionParams/numberOfHeadlines"));
        //        request.Add("BookMark", "");
        //        request.Add("SessionMark", "");
        //        request.Add("ViewType", "all");
        //        request.Add("ResetSessionMark", "no");
        //        request.Add("SortBy", "deliverydate");
        //        request.Add("SortOrder", "descending");

        //        request.Add("Preview", "yes");
        //        request.Add("ProgressDisclose", "false");
        //        request.Add("ChunkSize", "99");
        //        request.Add("ContentType", contentType);
        //        request.Add("PostStatus", "");
        //        request.Add("UseFolderDedup", "yes");

        //        string CacheTrackFoldersRss = "";

        //        // first check track deleted folders
        //        var cData = new ControlData
        //        {
        //            UserID = configData.getItem("//transactionParams/lwUserID"),
        //            UserPassword = configData.getItem("//transactionParams/lwPassword"),
        //            ProductID = configData.getItem("//transactionParams/lwNameSpace")
        //        };
        //        var trackMgr = new TrackDeletedFoldersCacheManager(cData.Clone(), "en");
        //        var deletedFolders = trackMgr.GetDeletedFolders();

        //        // [10.14.10] - avoid call to track if folder has been deleted
        //        if (deletedFolders != null &&
        //            deletedFolders.Count > 0 &&
        //            deletedFolders.ContainsKey(inputData.getItem(strvalidate)))
        //        {
        //            return CacheTrackFoldersRss;
        //        }
        //        else
        //        {
        //            // check platform cache next for fault tolerance
        //            string _strCacheID = string.Format("{0}_{1}_{2}{3}", inputData.getItem(strvalidate),
        //                                               inputData.getItem("uid"), inputData.getItem("namespace"),
        //                                               _strHashkey != "" ? "_" + _strHashkey.Substring(0, 1) : "_E");
        //            CacheTrackFoldersRss = GetCacheData(_strCacheID, 6); //check cache for xml data
        //            if (CacheTrackFoldersRss == "")
        //            {
        //                xmlRequest = createXMLRequest("CLIP_OLH_RSS_HEADLINE_2", "", control, request);
        //                Log(Logger.Level.INFO, "getSearch2Headlines [XML REQUEST] " + xmlRequest);

        //                // otherwise make track call
        //                ContentManagerClass fcm = new ContentManagerClass();
        //                _xmlResponse = fcm.XMLQueryString(xmlRequest, "", "");
        //                Log(Logger.Level.INFO, " getSearch2Headlines [XML RESPONSE]:  " + _xmlResponse);
        //                int _statusLevel = 0;
        //                if (ckeckFolderStatus(_xmlResponse, ref _statusLevel))
        //                    CacheTrackFoldersRss = AddCacheData(_xmlResponse, _strCacheID, 6);
        //                // add cache to database
        //                else
        //                {
        //                    for (int i = 0; i < _strErrorCode.Length; i++)
        //                    {
        //                        if (_statusLevel == Convert.ToInt32(_strErrorCode[i]))
        //                        {
        //                            CacheTrackFoldersRss = AddCacheData(_xmlResponse, _strCacheID, 6, _intCacheTime);
        //                            // add cache to database
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            return CacheTrackFoldersRss;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Log(Logger.Level.ERROR, "FCL::getFolderHeadlines Error - " + e.Message);
        //        throw e;
        //    }
        //}


        //public string getFDKFolderHeadlines(InputData inputData, ConfigData configData, string contentType)
        //{
        //    Log(Logger.Level.INFO, "FCL::getFDKFolderHeadlines inputData and ConfigData ");
        //    try
        //    {
        //        string reponseXmlQuery = "";
        //        string _xmlResponse = "";
        //        string _strHashkey = "";
        //        SortedList control = new SortedList();
        //        SortedList request = new SortedList();
        //        string strvalidate = configData.getItem("//validate/eid/params/param");
        //        int _intCacheTime = Convert.ToInt32(ConfigurationSettings.AppSettings["CacheTime"]);
        //        string[] _strErrorCode = ConfigurationSettings.AppSettings["OLHTrackErrorCodes"].Split(',');


        //        control.Add("FCS_CD_NETWORK_PARTNER_ID", "");
        //        control.Add("FCS_CD_CLIENT_TYPE", configData.getItem("//transactionParams/ClientTypeCode"));
        //        control.Add("FCS_CD_ACCESS_POINT_CODE", configData.getItem("//transactionParams/AccessPointCode"));
        //        control.Add("FCS_CD_USER_ID", configData.getItem("//transactionParams/lwUserID"));
        //        control.Add("FCS_CD_USER_PASSWORD", configData.getItem("//transactionParams/lwPassword"));
        //        control.Add("FCS_CD_PRODUCT_ID", configData.getItem("//transactionParams/lwNameSpace"));
        //        control.Add("FCS_CD_IP_ADDRESS", base._remoteHost);
        //        control.Add("FSS_PROXY_USER_ID", inputData.getItem("userID"));
        //        control.Add("FSS_PROXY_NAMESPACE", inputData.getItem("namespace"));
        //        if (inputData.getItem("hkey") != null)
        //        {
        //            _strHashkey = inputData.getItem("hkey");
        //            request.Add("hashKey", inputData.getItem("hkey"));
        //        }

        //        request.Add("FolderID", inputData.getItem(strvalidate));
        //        request.Add("HeadlineFormat", "headline");
        //        request.Add("HeadlineCount", configData.getItem("//transactionParams/numberOfHeadlines"));
        //        request.Add("BookMark", "");
        //        request.Add("SessionMark", "");
        //        request.Add("ViewType", "all");
        //        request.Add("ResetSessionMark", "no");
        //        request.Add("SortBy", "deliverydate");
        //        request.Add("SortOrder", "descending");

        //        request.Add("Preview", "yes");
        //        request.Add("ProgressDisclose", "false");
        //        request.Add("ChunkSize", "99");
        //        request.Add("ContentType", contentType);
        //        request.Add("PostStatus", "");
        //        request.Add("UseFolderDedup", "yes");

        //        string xmlRequest = "";
        //        string CacheTrackFoldersRss = "";

        //        // first check track deleted folders
        //        var cData = new ControlData
        //        {
        //            UserID = configData.getItem("//transactionParams/lwUserID"),
        //            UserPassword = configData.getItem("//transactionParams/lwPassword"),
        //            ProductID = configData.getItem("//transactionParams/lwNameSpace")
        //        };
        //        var trackMgr = new TrackDeletedFoldersCacheManager(cData.Clone(), "en");
        //        var deletedFolders = trackMgr.GetDeletedFolders();

        //        // [10.14.10] - avoid call to track if folder has been deleted
        //        if (deletedFolders != null &&
        //            deletedFolders.Count > 0 &&
        //            deletedFolders.ContainsKey(inputData.getItem(strvalidate)))
        //        {
        //            return CacheTrackFoldersRss;
        //        }
        //        else
        //        {
        //            // check platform cache next for fault tolerance
        //            string _strCacheID = string.Format("{0}_{1}_{2}{3}", inputData.getItem(strvalidate),
        //                                               inputData.getItem("uid"), inputData.getItem("namespace"),
        //                                               _strHashkey != "" ? "_" + _strHashkey.Substring(0, 1) : "_E");
        //            CacheTrackFoldersRss = GetCacheData(_strCacheID, 6); //check cache for xml data
        //            if (CacheTrackFoldersRss == "")
        //            {
        //                xmlRequest = createXMLRequest("CLIP_OLH_RSS_HEADLINE_2", "", control, request);
        //                Log(Logger.Level.INFO, "getSearch2Headlines [XML REQUEST] " + xmlRequest);

        //                // otherwise make track call
        //                ContentManagerClass fcm = new ContentManagerClass();
        //                _xmlResponse = fcm.XMLQueryString(xmlRequest, "", "");
        //                Log(Logger.Level.INFO, " getSearch2Headlines [XML RESPONSE]:  " + _xmlResponse);
        //                int _statusLevel = 0;
        //                if (ckeckFolderStatus(_xmlResponse, ref _statusLevel))
        //                    CacheTrackFoldersRss = AddCacheData(_xmlResponse, _strCacheID, 6);
        //                // add cache to database
        //                else
        //                {
        //                    for (int i = 0; i < _strErrorCode.Length; i++)
        //                    {
        //                        if (_statusLevel == Convert.ToInt32(_strErrorCode[i]))
        //                        {
        //                            CacheTrackFoldersRss = AddCacheData(_xmlResponse, _strCacheID, 6, _intCacheTime);
        //                            // add cache to database
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            return CacheTrackFoldersRss;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Log(Logger.Level.ERROR, "FCL::getFDKFolderHeadlines Error - " + e.Message);
        //        throw e;
        //    }
        //}

        public string getSearchHeadlines(InputData inputData, ConfigData configData)
        {
            //Search for editors choice information.
            Log(Logger.Level.INFO, "FCL::getSearchHeadlines InputData and ConfigData");

            try
            {
                var control = new SortedList();
                var request = new SortedList();


                //control.Add("FCS_CD_NETWORK_PARTNER_ID","");
                control.Add("FCS_CD_CLIENT_TYPE", configData.getItem("//transactionParams/ClientTypeCode"));
                control.Add("FCS_CD_ACCESS_POINT_CODE", configData.getItem("//transactionParams/AccessPointCode"));

                control.Add("FCS_CD_USER_ID", configData.getItem("//transactionParams/lwUserID"));
                control.Add("FCS_CD_USER_PASSWORD", configData.getItem("//transactionParams/lwPassword"));
                control.Add("FCS_CD_PRODUCT_ID", configData.getItem("//transactionParams/lwNameSpace"));
                control.Add("FCS_CD_IP_ADDRESS", _remoteHost);

                request.Add("bss", configData.getItem("//EditorsChoices/EditorsChoice[@urlParam='" + inputData.getItem("feed") + "']/bss"));
                request.Add("nhr", configData.getItem("//EditorsChoices/EditorsChoice[@urlParam='" + inputData.getItem("feed") + "']/@maxheadlines"));
                request.Add("so", "y");
                request.Add("is", "y");
                request.Add("qdf", "mdy");
                request.Add("rrs", "");
                request.Add("hrd", "");
                request.Add("crd", "");
                request.Add("rh", "");
                request.Add("qcs", "dj");
                request.Add("hpc", Int32.Parse(configData.getItem("//EditorsChoices/EditorsChoice[@urlParam='" + inputData.getItem("feed") + "']/@maxheadlines")));
                request.Add("ics", "");
                request.Add("irl", "");
                inputData.data.Add("Title", configData.getItem("//EditorsChoices/EditorsChoice[@urlParam='" + inputData.getItem("feed") + "']/title"));

                //Commented since no one is using this value
                //inputData.data.Add("tu","RSS_feed_" + configData.getItem("//EditorsChoices/EditorsChoice[@urlParam='" + inputData.getItem("feed") + "']/title").Replace(" ", "_"));

                var CacheEditorChoiceRss = GetCacheData(inputData.getItem("feed") + "_en", 3); //check cache for xml data
                if (CacheEditorChoiceRss == "")
                {
                    var xmlRequest = createXMLRequest("INDEX_Search_NOCACHE", "", control, request);
                    Log(Logger.Level.INFO, "getSearchHeadlines " + xmlRequest);
                    var fcm = new ContentManagerClass();
                    var xmlResponse = fcm.XMLQueryString(xmlRequest, "", "");
                    Log(Logger.Level.INFO, "getSearchHeadlines [XML RESPONSE] :: " + xmlResponse);
                    return AddCacheData(xmlResponse, inputData.getItem("feed") + "_en", 3); // add cache to database
                }
                else
                {
                    return CacheEditorChoiceRss; // return data from cache database
                }
            }
            catch (Exception e)
            {
                Log(Logger.Level.ERROR, "FCL::getSearchHeadlines Error - " + e.Message);
                throw;
            }
        }

        public string getPrefItemByID(InputData inputData, ConfigData configData)
        {
            Log(Logger.Level.INFO, "FCL::getPrefItemByID InputData NLID= : " + inputData.getItem("NLID") + "  and ConfigData");
            var xmlDoc = new XmlDocument();
            var editionID = "";
            try
            {
                var control = new SortedList();
                var request = new SortedList();
                control.Add("FCS_CD_NETWORK_PARTNER_ID", "");
                control.Add("FCS_CD_IP_ADDRESS", _remoteHost);
                control.Add("FCS_CD_ENCRYPTED_LOGIN", inputData.getItem("tk"));
                control.Add("referringURL", configData.getItem("//transactionParams/RefferingURL"));
                control.Add("callingURL", configData.getItem("//transactionParams/CallingURL"));
                control.Add("accessPointCode", configData.getItem("//transactionParams/AccessPointCode"));
                request.Add("ITEM_ID_LIST fcstype='list'", "<ITEM_ID>" + inputData.getItem("NLID") + "</ITEM_ID>");

                var xmlRequest = createXMLRequest("MSRVS_PGF_GET_ITEM_BY_ID", "", control, request);
                var fcm = new ContentManagerClass();
                var response = fcm.XMLQueryString(xmlRequest, "", "");
                Log(Logger.Level.INFO, "getPrefItemByID " + response);
                xmlDoc.LoadXml(response);
                if (xmlDoc.SelectSingleNode("//MEMBER_GET_ITEM_BY_ID_RESP/ResultSet/Result/ERROR_CODE").InnerText == "0")
                {
                    var xmlnsManager = new XmlNamespaceManager(xmlDoc.NameTable);
                    xmlnsManager.AddNamespace("n", "http://global.factiva.com/fvs/1.0");
                    if (xmlDoc.SelectSingleNode("//MEMBER_GET_ITEM_BY_ID_RESP/ResultSet/Result/RESPONSE_LIST/ITEM_BLOB/parentNewsLetter/LatestPublishedEdition/n:EditionID", xmlnsManager) != null)
                        editionID = xmlDoc.SelectSingleNode("//MEMBER_GET_ITEM_BY_ID_RESP/ResultSet/Result/RESPONSE_LIST/ITEM_BLOB/parentNewsLetter/LatestPublishedEdition/n:EditionID", xmlnsManager).InnerText;
                }
            }
            catch (COMException ce)
            {
                Log(Logger.Level.ERROR, "FCL::getPreference Error:COMException - " + ce.Message + "(" + ce.ErrorCode + ")");
            }
            catch (Exception e)
            {
                Log(Logger.Level.ERROR, "FCL::getPreference Error - " + e.Message);
                throw;
            }
            return editionID;
        }

        public bool validatetoken(string strToken, InputData inputData, ConfigData configData)
        {
            Log(Logger.Level.INFO, "FCL::validatetoken InputData : " + strToken + " ConfigData");
            var rtnErrorCode = false;
            var xmlDoc = new XmlDocument();
            try
            {
                var control = new SortedList();
                var request = new SortedList
                                  {
                                      {"token", strToken},
                                      {"referringUrl", configData.getItem("//transactionParams/RefferingURL")},
                                      {"callingUrl", configData.getItem("//transactionParams/CallingURL")},
                                      {"accessPointCode", configData.getItem("//transactionParams/AccessPointCode")}
                                  };
                var xmlRequest = createXMLRequest("MSRVS_AUTH_VALIDATE_TOKEN", "", control, request);
                var fcm = new ContentManagerClass();
                var response = fcm.XMLQueryString(xmlRequest, "", "");
                Log(Logger.Level.INFO, "validatetoken " + response);
                xmlDoc.LoadXml(response);
                if (xmlDoc.SelectSingleNode("//MEMBER_VALIDATE_AUTH_TOKEN_RESP/ResultSet/Result/ERROR_CODE").InnerText == "0")
                    rtnErrorCode = true;
            }
            catch (COMException ce)
            {
                Log(Logger.Level.ERROR, "FCL::validatetoken Error:COMException - " + ce.Message + "(" + ce.ErrorCode + ")");
            }
            catch (Exception e)
            {
                Log(Logger.Level.ERROR, "FCL::validatetoken Error - " + e.Message);
                throw;
            }
            return rtnErrorCode;
        }

        public string getPocast(InputData inputData, ConfigData configData)
        {
            /*Log(Logger.Level.INFO, "FCL::getPocast ");

            string _cachePodCastRss = "";
            string response = "";
            ServiceResponse serviceResponse;
            string strwebPages = "";
            string strPublication = "";
            string strPictures = "";
            try
            {
                string strPodCastCacheId = inputData.getItem("contid");
                _cachePodCastRss = GetCacheData(strPodCastCacheId, 8); //check cache for xml data
                if (_cachePodCastRss != "")
                {
                    response = _cachePodCastRss; // return data from cache database
                }
                else
                {
                    ControlData _controlData = ControlDataManager.GetLightWeightUserControlData(configData.getItem("//transactionParams/lwUserID"),
                                                                                                configData.getItem("//transactionParams/lwPassword"),
                                                                                                configData.getItem("//transactionParams/lwNameSpace"));
                    _controlData = ControlDataManager.AddProxyCredentialsToControlData(_controlData, inputData.getItem("userID"), inputData.getItem("namespace"));

                    GetContainerByIdRequest _getContainerByIdRequest = new GetContainerByIdRequest();
                    _getContainerByIdRequest.Id = long.Parse(inputData.getItem("contid"));
                    Log(Logger.Level.INFO, string.Format("FCL::getPocast {0} ", long.Parse(inputData.getItem("contid"))));

                    serviceResponse = DataContainersService.GetContainerById(_controlData, _getContainerByIdRequest);
                    object _getContainerByIdResp;
                    serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out _getContainerByIdResp);


                    foreach (ContentItem itemsInContainer in ((GetContainerByIdResponse) _getContainerByIdResp).ItemsCollection)
                    {
                        switch (itemsInContainer.ContentType)
                        {
                            case ContentType.WebSite:
                                strwebPages += "AN=" + itemsInContainer.Value + " or ";
                                break;
                            case ContentType.Publication:
                                strPublication += "AN=" + itemsInContainer.Value + " or ";
                                break;
                            case ContentType.Picture:
                                strPictures += "AN=" + itemsInContainer.Value + " or ";
                                break;
                        }
                    }
                    try
                    {
                        ControlData _controlData1 = ControlDataManager.GetLightWeightUserControlData(configData.getItem("//transactionParams/lwUserID"),
                                                                                                     configData.getItem("//transactionParams/lwPassword"),
                                                                                                     configData.getItem("//transactionParams/lwNameSpace"));
                        _controlData1 = ControlDataManager.AddProxyCredentialsToControlData(_controlData1, inputData.getItem("userID"), inputData.getItem("namespace"));
                        UpdateAssestStatisticsRequest _updateAssestStatisticsRequest = new UpdateAssestStatisticsRequest();
                        _updateAssestStatisticsRequest.AssetRequestType = AssetRequestType.PodcastRss;

                        serviceResponse = MetricsService.UpdateAssestStatistics(_controlData1, _updateAssestStatisticsRequest);
                    }
                    catch (Exception ex)
                    {
                        Log(Logger.Level.ERROR, string.Format("UpdateAssestStatistics Error - [50089] {0}", ex));
                    }

                    Log(Logger.Level.INFO, string.Format(" Pictures :-{0} |  Publication :-{1}   WebSite :-{2} ", strPictures, strPublication, strwebPages));

                    if (strwebPages != "")
                    {
                        strwebPages = strwebPages.Substring(0, strwebPages.LastIndexOf("or"));
                        strwebPages = "(" + strwebPages + ") and (fmt=webpage)";
                        response += getNoIndexCache(strwebPages, configData);
                    }
                    if (strPublication != "")
                    {
                        strPublication = strPublication.Substring(0, strPublication.LastIndexOf("or"));
                        strPublication = "(" + strPublication + ") and (fmt=article)";
                        response += getNoIndexCache(strPublication, configData);
                    }
                    if (strPictures != "")
                    {
                        strPictures = strPictures.Substring(0, strPictures.LastIndexOf("or"));
                        strPictures = "(" + strPictures + ") and (fmt=picture)";
                        response += getNoIndexCache(strPictures, configData);
                    }
                    response = AddCacheData(MyxmlParser(configData, "<IndexSearchResponse>" + response + "</IndexSearchResponse>"), inputData.getItem("contid"), 8);
                    Log(Logger.Level.INFO, string.Format("getNoIndexCache response (500034) :::::: {0}", response));
                }
            }
            catch (Exception ex)
            {
                Log(Logger.Level.ERROR, string.Format("getPodcast Error - [50016] {0}", ex));
            }

            return response;
            */
            return null;
        }

        //private string getNoIndexCache(string strSearchResults, ConfigData configData)
        //{
        //    Log(Logger.Level.INFO, string.Format("getNoIndexCache {0}", strSearchResults));

        //    SortedList control = new SortedList();
        //    SortedList request = new SortedList();
        //    string response = "";
        //    try
        //    {
        //        control.Add("FCS_CD_CLIENT_TYPE", configData.getItem("//transactionParams/ClientTypeCode"));
        //        control.Add("FCS_CD_ACCESS_POINT_CODE", configData.getItem("//transactionParams/AccessPointCode"));

        //        control.Add("FCS_CD_USER_ID", configData.getItem("//transactionParams/lwUserID"));
        //        control.Add("FCS_CD_USER_PASSWORD", configData.getItem("//transactionParams/lwPassword"));
        //        control.Add("FCS_CD_PRODUCT_ID", configData.getItem("//transactionParams/lwNameSpace"));
        //        control.Add("FCS_CD_IP_ADDRESS", base._remoteHost);

        //        request.Add("bss", strSearchResults);
        //        request.Add("nhr", "099");
        //        request.Add("hpc", 99);
        //        request.Add("so", "y");
        //        request.Add("is", "y");
        //        request.Add("qdf", "mdy");
        //        request.Add("rrs", "");
        //        request.Add("hrd", "");
        //        request.Add("crd", "");
        //        request.Add("rh", "");
        //        request.Add("qcs", "dj");
        //        request.Add("ics", "");
        //        request.Add("irl", "");
        //        string xmlRequest = createXMLRequest("INDEX_Search_NOCACHE", "1.0", control, request);
        //        Log(Logger.Level.INFO, string.Format("getNoIndexCache Request (500034) ::::::: {0}", xmlRequest));
        //        ContentManagerClass fcm = new ContentManagerClass();
        //        response = fcm.XMLQueryString(xmlRequest, "", "");
        //        Log(Logger.Level.INFO, string.Format("getNoIndexCache response (500034) :::::: {0}", response));

        //        xmlDoc.LoadXml(response);
        //        response = xmlDoc.SelectSingleNode("//IndexSearchResponse/ResultSet").OuterXml; // All results in search
        //    }
        //    catch (Exception e)
        //    {
        //        Log(Logger.Level.ERROR, "FCL::getNoIndexCache - Pod Cast Error (500035) - " + e.Message);
        //        throw e;
        //    }


        //    return response;
        //}

        public string getNewsLetter(InputData inputData, ConfigData configData)
        {
            Log(Logger.Level.INFO, "FCL::getPreference InputData and ConfigData");

            var response = "";
            var _controlData = new ControlData();

            try
            {
                if (validatetoken(inputData.getItem("tk"), inputData, configData))
                {
                    var cacheNewsLetterRss = GetCacheData("NLID_" + inputData.getItem("NLID") + "_en", 4);
                    if (cacheNewsLetterRss != "")
                    {
                        inputData.data.Add("MI", "NL:" + inputData.getItem("NLID") + "~NT:RSS");
                        response = cacheNewsLetterRss; // return data from cache database
                    }
                    else
                    {
                        _controlData.IPAddress = _remoteHost;
                        _controlData.EncryptedLogin = inputData.getItem("tk");
                        _controlData.ReferringUrl = configData.getItem("//transactionParams/RefferingURL");
                        _controlData.CallingUrl = configData.getItem("//transactionParams/CallingURL");
                        _controlData.AccessPointCode = configData.getItem("//transactionParams/AccessPointCode");


                        var getNewsletterByIdRequest = new GetNewsletterByIdRequest
                                                           {
                                                               id = int.Parse(inputData.getItem("NLID")),
                                                               returnLatestEditionOnly = false
                                                           };
                        var serviceResponse = NewsletterContentService.GetNewsletterById(cloneControlData(_controlData), getNewsletterByIdRequest);
                        object getNewsletterByIdResp;
                        serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out getNewsletterByIdResp);
                        var getNewsLetterIdResponse = (GetNewsletterByIdResponse) getNewsletterByIdResp;

                        Log(Logger.Level.INFO, string.Format("_getEditionByIdRequest Response [9934993] {0}", getNewsLetterIdResponse.newsletter.id));
                        var strEditionID = getNewsLetterIdResponse.newsletter.properties.latestPublishedEditionForRSS.editionId.ToString();
                        Log(Logger.Level.INFO, string.Format("Edition ID [9934993-2939399] {0}", strEditionID));


                        var getEditionRequest = new Factiva.Gateway.Messages.Assets.Editions.V2_0.GetEditionByIdRequest
                                                    {
                                                        Id = long.Parse(strEditionID),
                                                        ReturnEditionSections = true
                                                    };
                        serviceResponse = Factiva.Gateway.Services.V2_0.EditionService.GetEditionById(cloneControlData(_controlData), getEditionRequest);
                        object getEditionByIdResp;
                        serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out getEditionByIdResp);
                        var getEditionIdResponse = (Factiva.Gateway.Messages.Assets.Editions.V2_0.GetEditionByIdResponse) getEditionByIdResp;
                        var workspaceID = getEditionIdResponse.Edition.Properties.ParentWorkspaceId.ToString();

                        var workspaceManager = new WorkspaceManager(cloneControlData(_controlData), "en");
                        var manualWorkspace = workspaceManager.GetManualWorkspaceById(long.Parse(workspaceID));
                       
                        var workspaceAudience = manualWorkspace.Properties.Audience;
                        var audienceObject = serialize(workspaceAudience);
                        inputData.data.Add("audience", audienceObject);
                        inputData.data.Add("newsletterID", manualWorkspace.Id.ToString());
                        inputData.data.Add("newsletterName", manualWorkspace.Properties.Name);


                        var factivaItems = GetEditionItems(cloneControlData(_controlData), getEditionIdResponse.Edition);

                        var headlineInfo = new HeadlineInfo();
                        var documentCollection = new documentCollection();

                        // Article Items Only
                        foreach(var item in factivaItems.Values)
                        {
                            if ((item.HasBeenFound))
                            {
                                if (item.ContentHeadline != null)
                                {
                                    var document = new document
                                                       {
                                                           position = getItemPositionFromAN(item.AccessionNumber, manualWorkspace)
                                                       };
                                    PopulateDocument(document, item.ContentHeadline);
                                    documentCollection.Add(document);
                                }
                                    
                            }
                        }


                        foreach (var section in getEditionIdResponse.Edition.SectionCollection)
                        {
                            foreach (var contentItem in section.ItemCollection)
                            {
                                object objGetItem = contentItem;

                                if (objGetItem.GetType().ToString().Contains("LinkItem"))
                                {
                                    var linkItem = (Factiva.Gateway.Messages.Assets.Common.V2_0.LinkItem) contentItem;

                                    var document = new document
                                                        {
                                                            headline = linkItem.Title,
                                                            snippet = (linkItem.Type == LinkType.RssHeadlineUrl) ? string.Empty : linkItem.Description,
                                                            uri = linkItem.Uri
                                                        };

                                    documentCollection.Add(document);
                                }
                                else if (objGetItem.GetType().ToString().Contains("ImageItem"))
                                {
                                    var imageItem = (Factiva.Gateway.Messages.Assets.Common.V2_0.ImageItem) contentItem;
                                    documentCollection.Add(new document
                                                               {
                                                                   headline = imageItem.Title,
                                                                   uri = imageItem.PostbackUri,
                                                                   position = imageItem.Position
                                                               });
                                }
                            }
                        }


                        headlineInfo.documentList = documentCollection;


                        response = serialize(headlineInfo);

                        response =
                            AddCacheData(
                                MyxmlParser(configData, response),
                                "NLID_" + strEditionID + "_en", 4); // cache to database
                        Log(Logger.Level.INFO,
                            "getNewsLetter [XML RESPONSE](828832-399495) :: " + response);
                        

                        }
                }
            }
            catch (COMException ce)
            {
                Log(Logger.Level.ERROR, "FCL::getNewsLetter Error:COMException - " + ce + "(" + ce.ErrorCode + ")");
            }
            catch (Exception e)
            {
                Log(Logger.Level.ERROR, "FCL::getNewsLetter Error - " + e);
                throw;
            }
            return response;
        }

        public string MyxmlParser(ConfigData configData, string xmlString)
        {
            var rtnValue = xmlString;

            try
            {
                var xmlDoc = new XmlDocument();
                var resultString = new StringBuilder();
                using (var xmlWriter = new XmlTextWriter(new StringWriter(resultString)))
                {
                    using (var xmlReader = new XmlTextReader(new StringReader(rtnValue)))
                    {
                        var xslTransform = new XslCompiledTransform();
                        xslTransform.Load(HttpContext.Current.Request.MapPath(configData.getItem("//StyleSheet/name")));
                        xslTransform.Transform(xmlReader, xmlWriter);
                    }
                    xmlDoc.LoadXml(resultString.ToString());
                }

                rtnValue = xmlDoc.OuterXml;
            }
            catch (Exception ex)
            {
                Log(Logger.Level.ERROR, "FCL::MyxmlParser Error - " + ex.Message);
            }
            return rtnValue;
        }

        public string _getIndexHeadlines(string strSearchResults, string strNLID, InputData inputData, ConfigData configData)
        {
            Log(Logger.Level.INFO, "FCL::_getIndexHeadlines strSearchResults and strNLID " + strNLID + " and inputData and configData ");

            var control = new SortedList();
            var request = new SortedList();
            var xmlDoc = new XmlDocument();
            string response;
            try
            {
                control.Add("FCS_CD_NETWORK_PARTNER_ID", "");

                //Modified to support both existing Newsletters and Manual Workspace Newsletters
                if (inputData.getItem("tk")!=null)
                    control.Add("FCS_CD_ENCRYPTED_LOGIN", inputData.getItem("tk"));
                else
                {
                    control.Add("FCS_CD_USER_ID", configData.getItem("//transactionParams/lwUserID"));
                    control.Add("FCS_CD_USER_PASSWORD", configData.getItem("//transactionParams/lwPassword"));
                    control.Add("FCS_CD_PRODUCT_ID", configData.getItem("//transactionParams/lwNameSpace"));
                    control.Add("FSS_PROXY_USER_ID", inputData.getItem("userid"));
                    control.Add("FSS_PROXY_NAMESPACE", inputData.getItem("ns"));                    
                }
                control.Add("FCS_CD_IP_ADDRESS", _remoteHost);
                control.Add("referringURL", configData.getItem("//transactionParams/RefferingURL"));
                control.Add("callingURL", configData.getItem("//transactionParams/CallingURL"));
                control.Add("accessPointCode", configData.getItem("//transactionParams/AccessPointCode"));
                request.Add("bss", strSearchResults);
                request.Add("nhr", "099");
                request.Add("hpc", 99);
                request.Add("so", "y");
                request.Add("is", "y");
                request.Add("qdf", "mdy");
                request.Add("rrs", "");
                request.Add("hrd", "");
                request.Add("crd", "");
                request.Add("rh", "");
                request.Add("qcs", "dj");
                request.Add("ics", "");
                request.Add("irl", "");
                var xmlRequest = createXMLRequest("INDEX_Search_NOCACHE", "1.0", control, request);
                var fcm = new ContentManagerClass();
                response = fcm.XMLQueryString(xmlRequest, "", "");
                xmlDoc.LoadXml(response);
                response = xmlDoc.SelectSingleNode("//IndexSearchResponse/ResultSet").OuterXml; // All results in search
            }
            catch (Exception e)
            {
                Log(Logger.Level.ERROR, "FCL::getNewsLetterHeadlines Error - " + e.Message);
                throw;
            }

            return response;
        }

        protected string createXMLRequest(string tranName, string version, SortedList control, SortedList request)
        {
            Log(Logger.Level.INFO, "FCL::createXMLRequest tranName - " + tranName + " version - " + version);

            var requestXML = new StringBuilder();
            try
            {
                if (version == "")
                {
                    requestXML.Append(@"<" + tranName + @" xml:lang='en'>");
                }
                else
                {
                    requestXML.Append(@"<" + tranName + @" version='1.0' xml:lang='en'>");
                }

                requestXML.Append(@"<Control>");
                requestXML.Append(@"<FCS_CD>");
                for (var i = 0; i < control.Count; i++)
                {
                    requestXML.Append(@"<" + control.GetKey(i) + ">");
                    requestXML.Append(control.GetByIndex(i));
                    requestXML.Append(@"</" + control.GetKey(i) + ">");
                }
                requestXML.Append(@"</FCS_CD>");
                requestXML.Append(@"</Control>");
                requestXML.Append(@"<Request>");
                for (var i = 0; i < request.Count; i++)
                {
                    requestXML.Append(@"<" + request.GetKey(i) + ">");
                    requestXML.Append(request.GetByIndex(i));
                    if (request.GetKey(i).ToString().IndexOf(" ") > 0)
                    {
                        var tag = request.GetKey(i).ToString();
                        var endTag = tag.Substring(0, tag.IndexOf(" "));
                        requestXML.Append(@"</" + endTag + ">");
                    }
                    else
                    {
                        requestXML.Append(@"</" + request.GetKey(i) + ">");
                    }
                }
                requestXML.Append(@"</Request>");
                requestXML.Append(@"</" + tranName + ">");
                return requestXML.ToString();
            }
            catch (Exception)
            {
                Log(Logger.Level.ERROR, "FCL::createXMLRequest tranName - " + tranName + " version - " + version);
                throw;
            }
        }
        
        //private bool ckeckFolderStatus(string xmlData, ref int StatusCode)
        //{
        //    bool boolStatusCode = true;
        //    StatusCode = 0;
        //    xmlDoc.LoadXml(xmlData);
        //    if (xmlDoc.SelectSingleNode("//OLHRSSHeadlineResponse/ResultSet/FolderInfo").Attributes.GetNamedItem("status") != null)
        //        StatusCode = Convert.ToInt32(xmlDoc.SelectSingleNode("//OLHRSSHeadlineResponse/ResultSet/FolderInfo").Attributes.GetNamedItem("status").Value);
        //    if (StatusCode != 0)
        //        boolStatusCode = false;


        //    return boolStatusCode;
        //}

        //Gets the Cahed value passes the Namespace and Foldername 

        public string GetCacheData(string itemID, int nameSpace)
        {
            Log(Logger.Level.INFO, "FCL::GetCacheData itemID " + itemID);
            try
            {
                //GenericCache gc = new GenericCache();
                //return gc.GetItem(nameSpace, foldername).Value;
                var cacheManager = new CacheManager();
                return cacheManager.GetItem(itemID, getCacheNameSpaceFromId(nameSpace));
            }
            catch (Exception e)
            {
                Log(Logger.Level.ERROR, "FCL::GetCacheData" + e);
                return "";
            }
        }

        public string AddCacheData(string itemValue, string itemID, int nameSpace)
        {
            Log(Logger.Level.INFO, "FCL::AddCacheData XmlData and foldername " + itemID);

            try
            {
                //GenericCache gc = new GenericCache();
                //GenericCacheItem gci = new GenericCacheItem();
                //gci.ID = foldername;
                //gci.NameSpace = nameSpace;
                //gci.Value = XmlData;
                //gc.AddItem(gci);
                //return XmlData;

                var cacheManager = new CacheManager();
                cacheManager.AddItem(itemID, itemValue, getCacheNameSpaceFromId(nameSpace), CacheItemDuration.FifthteenMinutes);
                return itemValue;
            }
            catch (Exception e)
            {
                Log(Logger.Level.ERROR, "FCL::AddCacheData" + e);
                return itemValue;
            }
        }

        public CacheNameSpace getCacheNameSpaceFromId(int nameSpace)
        {
            Log(Logger.Level.INFO, "FCL::getCacheNameSpaceFromId");
            
            var cacheNamespace = CacheNameSpace.PodCast;
            try
            {
                
                switch (nameSpace)
                {
                    case 2:
                        cacheNamespace = CacheNameSpace.Search;
                        break;
                    case 3:
                        cacheNamespace = CacheNameSpace.EditorChoice;
                        break;
                    case 4:
                        cacheNamespace = CacheNameSpace.NewsLetter;
                        break;
                    case 5:
                        cacheNamespace = CacheNameSpace.Widgets;
                        break;
                    case 6:
                        cacheNamespace = CacheNameSpace.TrackFolder;
                        break;
                    case 7:
                        cacheNamespace = CacheNameSpace.ProjectVisible;
                        break;
                    case 8:
                        cacheNamespace = CacheNameSpace.PodCast;
                        break;
                }
            }
            catch (Exception e)
            {
                Log(Logger.Level.ERROR, "FCL::getCacheNameSpaceFromId" + e);

            }
            return cacheNamespace;                

        }

        //private string GetOLHRequest(int maxHeadlines, string folderID)
        //{
        //    Log(Logger.Level.INFO, "FCL::GetOLHRequest maxHeadlines - " + maxHeadlines + " folderID - " + folderID);

        //    try
        //    {
        //        PerformContentSearch searchRequest = new PerformContentSearch();


        //        searchRequest.firstResultToReturn = 0;

        //        searchRequest.maxResultsToReturn = maxHeadlines;
        //        StructuredSearch strSearch = new StructuredSearch();
        //        // Structured Search - formatting
        //        ResultFormatting format = new ResultFormatting();
        //        format.snippetType = SnippetType.Fixed;
        //        format.snippetTypeSpecified = true;
        //        format.sortOrder = ResultSortOrder.ArrivalTime;
        //        format.sortOrderSpecified = true;
        //        format.clusterMode = ClusterMode.Off;
        //        format.clusterModeSpecified = true;
        //        strSearch.formatting = format;

        //        // Structured Search - query


        //        StructuredQuery query = new StructuredQuery();

        //        query.collection = new SearchCollection[3];
        //        query.collection[0] = SearchCollection.Publications;
        //        query.collection[1] = SearchCollection.Pictures;
        //        query.collection[2] = SearchCollection.WebSites;
        //        query.searchString = new SearchString[1];
        //        SearchString qString = new SearchString();

        //        //qString.type =  Factiva.Services.Olh.V2_0.SearchType.controlled;

        //        //qString.mode = Factiva.Services.Olh.V2_0.SearchMode.simple ;
        //        //qString.typeSpecified = true;

        //        qString.scope = "flp";
        //        qString.Value = folderID;
        //        query.searchString[0] = qString;


        //        //				Structured Search - query ( language ) 

        //        //				SearchString langString = new SearchString();
        //        //
        //        //				langString .mode = SearchMode.all;
        //        //
        //        //				langString .modeSpecified = true;
        //        //
        //        //				langString .scope = "la";
        //        //
        //        //				langString .Value = "en";
        //        //
        //        //				langString.validate = false;
        //        //
        //        //				langString.validateSpecified = true;
        //        //
        //        //				query.searchString[1] = langString ;

        //        // Structured Search language


        //        strSearch.query = query;


        //        //Region Navigation control

        //        //				searchRequest.navigationControl = new NavigationControl();
        //        //
        //        //				searchRequest.navigationControl.codeNavigatorControl = new CodeNavigatorControl();
        //        //
        //        //				searchRequest.navigationControl.codeNavigatorControl.mode = CodeNavigatorMode.All;
        //        //
        //        //				searchRequest.navigationControl.codeNavigatorControl.minBucketValue = 0;
        //        //
        //        //				searchRequest.navigationControl.codeNavigatorControl.minBucketValueSpecified = true;
        //        //
        //        //				searchRequest.navigationControl.codeNavigatorControl.maxBuckets = 5;
        //        //
        //        //				searchRequest.navigationControl.codeNavigatorControl.maxBucketsSpecified = true;
        //        //
        //        //				searchRequest.navigationControl.returnCollectionCounts = true;
        //        //
        //        //				searchRequest.navigationControl.returnCollectionCountsSpecified = true;
        //        //
        //        //				searchRequest.navigationControl.customNavigatorControl = null;
        //        //
        //        //				searchRequest.navigationControl.descriptorControl = new DescriptorControl();
        //        //
        //        //				searchRequest.navigationControl.descriptorControl.mode = DescriptorControlMode.All;

        //        // region


        //        searchRequest.structuredSearch = strSearch;
        //        StringWriter sw = new StringWriter();
        //        XmlSerializer ser = new XmlSerializer(typeof (PerformContentSearch));
        //        ser.Serialize(sw, searchRequest);
        //        return sw.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log(Logger.Level.ERROR, "FCL::GetOLHRequest Error - " + ex.Message);
        //        throw ex;
        //    }
        //}
        
        private static AccessionNumberSearchResponse GetAutomaticWorkspaceItems(ControlData userControlData, AutomaticWorkspace autoWorkspace)
        {
            
            var searchManager = new SearchManager(userControlData, "en");

            var requestDTO = new AccessionNumberSearchRequestDTO
                                 {
                                     AccessionNumbers = GetAccessionNumbers(autoWorkspace).ToArray(),
                                     SortBy = SortBy.LIFO
                                 };

            SearchCollectionCollection searchCollections = new SearchCollectionCollection
                                                               {
                                                                   SearchCollection.Blogs,
                                                                   SearchCollection.Boards,
                                                                   SearchCollection.CustomerDoc,
                                                                   SearchCollection.Multimedia,
                                                                   SearchCollection.Pictures,
                                                                   SearchCollection.Publications,
                                                                   SearchCollection.WebSites
                                                               };
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.AlistBlogs);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.Audio);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.Internal);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.Magazines);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.Newspapers);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.NewsSites);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.Summary);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.Video);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.Wires);


            requestDTO.SearchCollectionCollection = searchCollections;
            

            if (requestDTO.IsValid())
            {
                return searchManager.PerformAccessionNumberSearch(requestDTO);
            }
            return null;
        }

        private Dictionary<string, AccessionNumberBasedContentItem> GetManualNewsletterWorkspaceItems(ControlData userControlData, ManualWorkspace manualWorkspace)
        {
            var dictOfAccessionBasedContentItems = new Dictionary<string, AccessionNumberBasedContentItem>();
            var searchManager = new SearchManager(userControlData, "en");
            var requestDTO = new AccessionNumberSearchRequestDTO
                                 {
                                     AccessionNumbers = GetAccessionNumbers(manualWorkspace).ToArray(),
                                     SortBy = SortBy.LIFO
                                 };

            var searchCollections = new SearchCollectionCollection
                                        {
                                            SearchCollection.Blogs,
                                            SearchCollection.Boards,
                                            SearchCollection.CustomerDoc,
                                            SearchCollection.Multimedia,
                                            SearchCollection.Pictures,
                                            SearchCollection.Publications,
                                            SearchCollection.WebSites
                                        };
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.AlistBlogs);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.Audio);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.Internal);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.Magazines);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.Newspapers);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.NewsSites);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.Summary);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.Video);
            //searchCollections.Add(Factiva.Gateway.Messages.Search.V2_0.SearchCollection.Wires);


            requestDTO.SearchCollectionCollection = searchCollections;



            if (requestDTO.IsValid())
            {
                var searchResponse = searchManager.PerformAccessionNumberSearch(requestDTO);
                if (searchResponse != null && searchResponse.AccessionNumberBasedContentItemSet != null &&
                    searchResponse.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection != null &&
                    searchResponse.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection.Count > 0)
                {
                    // add items
                    foreach (var item in searchResponse.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection)
                    {
                        if (!dictOfAccessionBasedContentItems.ContainsKey(item.AccessionNumber))
                        {
                            dictOfAccessionBasedContentItems.Add(item.AccessionNumber, item);
                        }
                    }
                }
            }

            return dictOfAccessionBasedContentItems;
        }

        private Dictionary<string, AccessionNumberBasedContentItem> GetEditionItems(ControlData userControlData, Factiva.Gateway.Messages.Assets.Editions.V2_0.Edition edition)
        {
            var dictOfAccessionBasedContentItems = new Dictionary<string, AccessionNumberBasedContentItem>();
            var searchManager = new SearchManager(userControlData, "en");
            var requestDTO = new AccessionNumberSearchRequestDTO
                                 {
                                     AccessionNumbers = GetAccessionNumbers(edition).ToArray(),
                                     SortBy = SortBy.LIFO
                                 };
            if (requestDTO.IsValid())
            {
                var searchResponse = searchManager.PerformAccessionNumberSearch(requestDTO);

                if (searchResponse != null && searchResponse.AccessionNumberBasedContentItemSet != null &&
                    searchResponse.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection != null &&
                    searchResponse.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection.Count > 0)
                {
                    // add items
                    foreach (var item in searchResponse.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection)
                    {
                        if (!dictOfAccessionBasedContentItems.ContainsKey(item.AccessionNumber))
                        {
                            dictOfAccessionBasedContentItems.Add(item.AccessionNumber, item);
                        }
                    }
                }
            }

            return dictOfAccessionBasedContentItems;
        }

        private static List<string> GetAccessionNumbers(AutomaticWorkspace autoWorkspace)
        {
            var accessionNos = new List<string>();
            foreach (var item in autoWorkspace.ItemsCollection)
            {
                var articleItem = item as Common_V2.ArticleItem;
                if (articleItem != null)
                {
                    if (!accessionNos.Contains(articleItem.AccessionNumber))
                    {
                        accessionNos.Add(articleItem.AccessionNumber);
                    }
                }
            }
            return accessionNos;
        }

        private static List<string> GetAccessionNumbers(ManualWorkspace manaualWorkspace)
        {
            var accessionNos = new List<string>();
            foreach (var section in manaualWorkspace.SectionCollection)
            {
                foreach (var item in section.ItemCollection)
                {
                    var articleItem = item as Common_V2.ArticleItem;
                    if (articleItem != null && !accessionNos.Contains(articleItem.AccessionNumber))
                    {
                        accessionNos.Add(articleItem.AccessionNumber);
                    }
                }
            }
            return accessionNos;
        }

        private static List<string> GetAccessionNumbers(Factiva.Gateway.Messages.Assets.Editions.V2_0.Edition edition)
        {
            var accessionNos = new List<string>();
            foreach (var section in edition.SectionCollection)
            {
                foreach (var item in section.ItemCollection)
                {
                    var articleItem = item as Common_V2.ArticleItem;
                    if (articleItem != null && !accessionNos.Contains(articleItem.AccessionNumber))
                    {
                        accessionNos.Add(articleItem.AccessionNumber);
                    }
                }
            }
            return accessionNos;
        }
        
        public string getManualWorkpaceForNewsLetter(InputData inputData, ConfigData configData)
        {
            Log(Logger.Level.INFO, "FCL::getManualWorkpaceForNewsLetter");

            var response = "";

            var _controlData = new ControlData();

            try
            {
                _controlData.IPAddress = _remoteHost;
                _controlData.ProxyUserID = inputData.getItem("userid");
                _controlData.ProxyUserNamespace = inputData.getItem("ns");
                _controlData.UserID = configData.getItem("//transactionParams/lwUserID");
                _controlData.UserPassword = configData.getItem("//transactionParams/lwPassword");
                _controlData.ProductID = configData.getItem("//transactionParams/lwNameSpace");
                _controlData.AccessPointCode = configData.getItem("//transactionParams/AccessPointCode");

                var workspaceManager = new WorkspaceManager(_controlData, "en");
                var manualWorkspace = workspaceManager.GetManualWorkspaceById(long.Parse(inputData.getItem("WSID")));

                var workspaceAudience = manualWorkspace.Properties.Audience;
                var audienceObject = serialize(workspaceAudience);
                inputData.data.Add("audience", audienceObject);

                inputData.data.Add("newsletterID", manualWorkspace.Id.ToString());
                inputData.data.Add("newsletterName", manualWorkspace.Properties.Name);

                if (manualWorkspace.Properties.AreFeedsActive == false)
                {
                    return "<HeadlineInfo></HeadlineInfo>";
                }
                
                var cacheManualWorkspaceForNewsLetterRss = GetCacheData("WSID_" + inputData.getItem("WSID") + "_en", 4);
                if (cacheManualWorkspaceForNewsLetterRss != "")
                {
                    response = cacheManualWorkspaceForNewsLetterRss; // return data from cache database
                }
                else
                {
                    var factivaItems = GetManualNewsletterWorkspaceItems(_controlData,manualWorkspace);
                    var headlineInfo = new HeadlineInfo();
                    var documentCollection = new documentCollection();

                    // Article Items Only
                    foreach(var item in factivaItems.Values)
                    {
                        if ((item.HasBeenFound) && (IsArticlePodcastable(item, inputData.getItem("from").ToLower()) == "1"))
                        {
                            if (item.ContentHeadline != null)
                            {
                                var document = new document
                                                   {
                                                        position = getItemPositionFromAN(item.AccessionNumber, manualWorkspace)
                                                   };
                                PopulateDocument(document, item.ContentHeadline);
                                documentCollection.Add(document);
                            }
                        }
                    }


                    // Non Article Items - Link Item , Insight Chart Item
                    if (inputData.getItem("from").ToLower() != "nl2pcast")
                    {
                        foreach (var section in manualWorkspace.SectionCollection)
                        {
                            foreach (var contentItem in section.ItemCollection)
                            {
                                object objGetItem = contentItem;

                                if (objGetItem.GetType().ToString().Contains("LinkItem"))
                                {
                                    var linkItem = (Factiva.Gateway.Messages.Assets.Common.V2_0.LinkItem) contentItem;
                                    documentCollection.Add(new document
                                                           {
                                                               headline = linkItem.Title,
                                                               snippet = (linkItem.Type == LinkType.RssHeadlineUrl) ? string.Empty : linkItem.Description,
                                                               uri = linkItem.Uri,
                                                               position = linkItem.Position
                                                           });
                                }
                                else if (objGetItem.GetType().ToString().Contains("ImageItem"))
                                {
                                    var imageItem = (Factiva.Gateway.Messages.Assets.Common.V2_0.ImageItem) contentItem;
                                    documentCollection.Add(new document
                                                               {
                                                                   headline = imageItem.Title,
                                                                   uri = imageItem.PostbackUri,
                                                                   position = imageItem.Position
                                                               });
                                }
                            }
                        }
                    }
                    documentCollection.Sort((d1, d2) => d1.position.CompareTo(d2.position)); ;
                    headlineInfo.documentList = documentCollection;
                    response = serialize(headlineInfo);
                    response = AddCacheData(MyxmlParser(configData, response),
                                            "WSID_" + inputData.getItem("WSID") + "_en",
                                            4); // cache to database
                    Log(Logger.Level.INFO, "getManualWorkpaceForNewsLetter: " + response);
                }
            }
            catch (COMException ce)
            {
                Log(Logger.Level.ERROR, "FCL::getManualWorkpaceForNewsLetter Error:COMException - " + ce + "(" + ce.ErrorCode + ")");
            }
            catch (Exception e)
            {
                Log(Logger.Level.ERROR, "FCL::getManualWorkpaceForNewsLetter Error - " + e);
                throw;
            }
            return response;
        }

        public string getAutomaticWorkpaceForWorkspaces(InputData inputData, ConfigData configData)
        {
            Log(Logger.Level.INFO, "FCL::getAutomaticWorkpaceForWorkspaces");

            var response = "";
            var controlData = new ControlData();

            try
            {
                controlData.IPAddress = _remoteHost;
                controlData.ProxyUserID = inputData.getItem("userid");
                controlData.ProxyUserNamespace = inputData.getItem("ns");
                controlData.UserID = configData.getItem("//transactionParams/lwUserID");
                controlData.UserPassword = configData.getItem("//transactionParams/lwPassword");
                controlData.ProductID = configData.getItem("//transactionParams/lwNameSpace");
                controlData.AccessPointCode = configData.getItem("//transactionParams/AccessPointCode");

                var workspaceManager = new WorkspaceManager(controlData, "en");
                var automaticWorkspace = workspaceManager.GetAutomaticWorkspaceById(long.Parse(inputData.getItem("wsid")));

                if (automaticWorkspace.Properties.AreFeedsActive == false)
                    return "<HeadlineInfo></HeadlineInfo>";

                var workspaceAudience = automaticWorkspace.Properties.Audience;
                var audienceObject = serialize(workspaceAudience);
                inputData.data.Add("audience", audienceObject);

                inputData.data.Add("workspaceID",automaticWorkspace.Id.ToString());
                inputData.data.Add("workspaceName", automaticWorkspace.Properties.Name);

                var cacheAutomaticWorkspaceRss = GetCacheData("WSID_" + inputData.getItem("WSID") + "_en", 4);
                if (cacheAutomaticWorkspaceRss != "")
                {
                    response = cacheAutomaticWorkspaceRss; // return data from cache database
                }
                else
                {
                    var searchResponse = GetAutomaticWorkspaceItems(controlData, automaticWorkspace);

                    var headlineInfo = new HeadlineInfo();
                    var documentCollection = new documentCollection();

                    if( (searchResponse!=null) && (searchResponse.AccessionNumberBasedContentItemSet != null))
                    {
                        foreach (var item in searchResponse.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection)
                        {
                            if ((item.HasBeenFound) && (IsArticlePodcastable(item, inputData.getItem("from").ToLower()) == "1"))
                            {
                                if (item.ContentHeadline != null)
                                {
                                    var document = new document
                                                       {
                                                           position = getItemPositionFromAN(item.AccessionNumber, automaticWorkspace)
                                                       };
                                    PopulateDocument(document, item.ContentHeadline);
                                    documentCollection.Add(document);
                                }
                            }
                        }
                    }

                    //_documentCollection.Sort(delegate(document d1, document d2)
                    //                                   {
                    //                                       return d1.position.CompareTo(d2.position);
                    //                                   }); ;

                    headlineInfo.documentList = documentCollection;
                    response = serialize(headlineInfo);

                    response =
                        AddCacheData(
                            MyxmlParser(configData,  response ),
                            "WSID_" + inputData.getItem("WSID") + "_en", 4); // cache to database
                }
                Log(Logger.Level.INFO,
                    "getAutomaticWorkpaceForWorkspaces [XML RESPONSE](828832-399495) :: " + response);
            }
            catch (COMException ce)
            {
                Log(Logger.Level.ERROR, "FCL::getAutomaticWorkpaceForWorkspaces Error:COMException - " + ce + "(" + ce.ErrorCode + ")");
            }
            catch (Exception e)
            {
                Log(Logger.Level.ERROR, "FCL::getAutomaticWorkpaceForWorkspaces Error - " + e);
                throw;
            }
            return response;
        }

        #region << Helper Functions >>
        public void Log(Logger.Level level, string logMsg)
        {
            if (ConfigurationSettings.AppSettings["logging"] == "On" || level >= Logger.Level.ERROR)
                Logger.Log(level, logMsg);
        }

        public string serialize(object obj)
        {
            // Serialization
            String XmlizedString;
            var memoryStream = new MemoryStream();
            var xs = new XmlSerializer(obj.GetType());
            var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            xs.Serialize(xmlTextWriter, obj);
            memoryStream = (MemoryStream) xmlTextWriter.BaseStream;
            XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
            var stringToRemove = "?<?xml version='1.0' encoding='utf-8'?>";
            return XmlizedString.Remove(0, stringToRemove.Length);
        }

        private static String UTF8ByteArrayToString(Byte[] characters)
        {
            var encoding = new UTF8Encoding();
            var constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        public static string IsArticlePodcastable(AccessionNumberBasedContentItem contentItem,string product)
        {
            if (( product ==  "nl2pcast") || ( product ==  "ws1pcast"))
            {
                const string supportedLangs = "en,de,fr,es,it";
                const int wordCountSupported = 5000;
                var language = contentItem.ContentHeadline.BaseLanguage;
                var wordcount = contentItem.ContentHeadline.WordCount;

                if (IsLanguageOK(language, supportedLangs) && IsWordCountOK(wordcount, wordCountSupported) &&
                    IsContentTypeOK(contentItem))
                {
                    return "1";
                }
                return "0";
            }
            return "1";
        }

        public static bool IsLanguageOK(string articleLanguage, string langsSupported)
        {
            var ret = false;
            try
            {
                if (articleLanguage != null)
                {
                    char[] delimiterChars = { ',' };
                    var langs = langsSupported.Split(delimiterChars);
                    ret = (Array.IndexOf(langs, articleLanguage.Trim().ToLower()) != -1);
                }
            }
            catch (Exception)
            {
            }
            return ret;
        }

        public static bool IsWordCountOK(int wordCountArticle,int wordCountSupported)
        {
            var ret = false;
            try
            {
                if (wordCountArticle > 0 && wordCountArticle <= wordCountSupported)
                {
                    ret = true;
                }
            }
            catch (Exception)
            {
            }
            return ret;
        }

        public static bool IsContentTypeOK(AccessionNumberBasedContentItem contentItem)
        {
            try
            {
                if (contentItem != null &&
                    contentItem.ContentHeadline != null &&
                    contentItem.ContentHeadline.ContentItems !=null &&
                    contentItem.ContentHeadline.ContentItems.ContentType !=null &&
                    (contentItem.ContentHeadline.ContentItems.ContentType.ToLower() == "article" ||
                    contentItem.ContentHeadline.ContentItems.ContentType.ToLower() == "articlewithgraphics"))
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        public ControlData cloneControlData(ControlData controlData)
        {
            var _controlData = new ControlData();
            try
            {
                if (controlData != null)
                {
                    _controlData.AccessPointCode = controlData.AccessPointCode;
                    _controlData.EncryptedLogin = controlData.EncryptedLogin;
                    _controlData.ReferringUrl= controlData.ReferringUrl;
                    _controlData.CallingUrl=    controlData.CallingUrl;
                }
            }
            catch (Exception)
            {
            }
            return _controlData;
        }

        public int getItemPositionFromAN(string accessionNumber,ManualWorkspace manualWorkspace)
        {
            var position = -1;
            var itemFound = false;
            try
            {
                foreach (var section in manualWorkspace.SectionCollection)
                {
                    foreach (var item in section.ItemCollection)
                    {
                        object objGetItem = item;

                        if (objGetItem.GetType().ToString().Contains("ArticleItem"))
                        {
                            var articleItem = (Factiva.Gateway.Messages.Assets.Common.V2_0.ArticleItem)item;

                            if (articleItem.AccessionNumber == accessionNumber)
                            {
                                position = item.Position;
                                itemFound = true;
                                break;
                            }
                        }
                    }
                    if (itemFound) break;
                }

            }
            catch (Exception)
            {
            }
            return position;
        }
        public int getItemPositionFromAN(string accessionNumber, AutomaticWorkspace automaticWorkspace)
        {
            var position = -1;
            try
            {
                foreach (Factiva.Gateway.Messages.Assets.Common.V2_0.Item item in automaticWorkspace.ItemsCollection)
                {
                    object objGetItem = item;

                    if (objGetItem.GetType().ToString().Contains("ArticleItem"))
                    {
                        var articleItem = (Factiva.Gateway.Messages.Assets.Common.V2_0.ArticleItem)item;

                        if (articleItem.AccessionNumber == accessionNumber)
                        {
                            position = item.Position;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return position;
        }

        
        #endregion
    }
    #region << HeadlineInfo >>
    [XmlRoot(ElementName = "headlineInfo", Namespace = "", IsNullable = false), Serializable]
    public class HeadlineInfo
    {

        [XmlElement(Type = typeof(documentCollection), ElementName = "documentList", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = "")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public documentCollection __documentList;

        [XmlIgnore]
        public documentCollection documentList
        {
            get { return __documentList ?? (__documentList = new documentCollection()); }
            set { __documentList = value; }
        }
    }
    [Serializable]
    public class documentCollection : List<document>
    {
    }

    [XmlType(TypeName = "document", Namespace = ""), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class document
    {

        [XmlElement(ElementName = "accessionNumber", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = "")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __accessionNumber;

        [XmlIgnore]
        public string accessionNumber
        {
            get { return __accessionNumber; }
            set { __accessionNumber = value; }
        }

        [XmlElement(ElementName = "headline", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = "")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __headline;

        [XmlIgnore]
        public string headline
        {
            get { return __headline; }
            set { __headline = value; }
        }

        [XmlElement(ElementName = "snippet", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = "")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __snippet;

        [XmlIgnore]
        public string snippet
        {
            get { return __snippet; }
            set { __snippet = value; }
        }

        [XmlElement(ElementName = "sourceCode", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = "")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __sourceCode;

        [XmlIgnore]
        public string sourceCode
        {
            get { return __sourceCode; }
            set { __sourceCode = value; }
        }


        [XmlElement(ElementName = "sourceName", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = "")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __sourceName;

        [XmlIgnore]
        public string sourceName
        {
            get { return __sourceName; }
            set { __sourceName = value; }
        }

        [XmlElement(ElementName = "publicationDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = "")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DateTime __publicationDate;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __publicationDateSpecified;

        [XmlIgnore]
        public DateTime publicationDate
        {
            get { return __publicationDate; }
            set { __publicationDate = value; __publicationDateSpecified = true; }
        }

        [XmlElement(ElementName = "category", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = "")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __category;

        [XmlIgnore]
        public string category
        {
            get { return __category; }
            set { __category = value; }
        }


        [XmlElement(ElementName = "language", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = "")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __language;

        [XmlIgnore]
        public string language
        {
            get { return __language; }
            set { __language = value; }
        }


        [XmlElement(ElementName = "words", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = "")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __words;

        [XmlIgnore]
        public int words
        {
            get { return __words; }
            set { __words = value; }
        }


        [XmlElement(ElementName = "uri", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = "")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __uri;

        [XmlIgnore]
        public string uri
        {
            get { return __uri; }
            set { __uri = value; }
        }

        [XmlElement(ElementName = "position", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = "")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __position;

        [XmlIgnore]
        public int position
        {
            get { return __position; }
            set { __position = value; }
        }

        [XmlElement(ElementName = "reference", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = "")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __reference;

        [XmlIgnore]
        public string reference
        {
            get { return __reference; }
            set { __reference = value; }
        }

    }
#endregion 
}
