using System;
using System.Collections.Generic;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.Messages.Screening.V1_1;
using System.Linq;
using Factiva.Gateway.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.Messages.Assets.CompanyList.V1_0;
using Factiva.Gateway.Messages.Assets.Lists.V1_0;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Factiva.Gateway.Managers;
using DowJones.Exceptions;


namespace DowJones.Managers.QueryUtility
{
    public class FCPServiceWrapper 
    {
        

        private Factiva.Gateway.Utils.V1_0.ControlData controlData;
        
        #region Constructors

        static FCPServiceWrapper()
        {
         
        }

        public FCPServiceWrapper(Factiva.Gateway.Utils.V1_0.ControlData controlData) {
            this.controlData = controlData;
            
        }

           

        #endregion

        public bool LastOperationTruncated { get; private set; }


        /// <summary>
        /// Gets a list of FCodes for companies covered by the company screening criteria.
        /// </summary>
        /// <param name="criteriaId">Screening criteria ID</param>
        /// <returns>A list of FCodes for companies covered by the company screening criteria.</returns>
        public string[] GetCompanyListFromScreeningCriteria(string criteriaId)
        {
            //if (Logger.IsDebugEnabled) LogMethodStart();

            try
            {
                GetCompanyScreeningListExRequest getCompanyScreeningListExRequest = new GetCompanyScreeningListExRequest();

                getCompanyScreeningListExRequest.ScreeningContext = null;
                getCompanyScreeningListExRequest.ScreeningCriteriaID = criteriaId;

                getCompanyScreeningListExRequest.GetHitCounts = true;
                getCompanyScreeningListExRequest.SortBy = CompanyProfilesSortBy.CompanyName;
                getCompanyScreeningListExRequest.SortOrder = CompanyProfilesSortOrder.Ascending;
                getCompanyScreeningListExRequest.Language = "en";
                getCompanyScreeningListExRequest.DisplayOption = ScreeningListDisplayOptions.Standard;
                getCompanyScreeningListExRequest.ReturnErrorIfHitsGreaterThan = 0;
                getCompanyScreeningListExRequest.ResponseXMLElementNames = ResponseXMLElementNames.Verbose;
                getCompanyScreeningListExRequest.IncludeDescriptors = true;
                getCompanyScreeningListExRequest.CompanyListOptionalElements.AddRange(new CompanyListOptionalElements[] { CompanyListOptionalElements.URL, CompanyListOptionalElements.MarketIndices });

                ServiceResponse serviceResponse = ScreeningService.GetCompanyScreeningListEx(ControlDataManager.Clone(controlData), getCompanyScreeningListExRequest);

                object responseObj;
                CheckServiceResponse(serviceResponse, out responseObj);

                GetCompanyScreeningListExResponse getCompanyScreeningListExResponse = (GetCompanyScreeningListExResponse)responseObj;
                Companies companies = getCompanyScreeningListExResponse.CompanyScreeningListResult.Companies;

                string[] values = (from c in companies.CompanyCollection
                                   select c.Fcode).ToArray();

                return values;
            }
            finally
            {
                //if (Logger.IsDebugEnabled) LogMethodEnd();
            }
        }

        /// <summary>
        /// Gets a list of FCodes for executives covered by the executive screening criteria.
        /// </summary>
        /// <param name="criteriaId">Screening criteria ID</param>
        /// <returns>A list of FCodes for companies covered by the executive screening criteria.</returns>
        public string[] GetExecutiveListFromScreeningCriteria(string criteriaId)
        {
            //if (Logger.IsDebugEnabled) LogMethodStart();

            try
            {
                
                GetExecutiveScreeningListExRequest getExecutiveScreeningListExRequest = new GetExecutiveScreeningListExRequest();
                getExecutiveScreeningListExRequest.ScreeningContext = null;
                getExecutiveScreeningListExRequest.ScreeningCriteriaID = criteriaId;
                getExecutiveScreeningListExRequest.GetHitCounts = true;
                getExecutiveScreeningListExRequest.SortBy = ExecutiveProfilesSortBy.SortName;
                getExecutiveScreeningListExRequest.SortOrder = ExecutiveProfilesSortOrder.Ascending;
                getExecutiveScreeningListExRequest.Language = "en";
                getExecutiveScreeningListExRequest.DisplayOption = ScreeningListDisplayOptions.Standard;
                getExecutiveScreeningListExRequest.ReturnCriteriaInResponse = true;
                getExecutiveScreeningListExRequest.ReturnErrorIfHitsGreaterThan = 0;
                getExecutiveScreeningListExRequest.ResponseXMLElementNames = ResponseXMLElementNames.Verbose;
                getExecutiveScreeningListExRequest.IncludeDescriptors = true;
                getExecutiveScreeningListExRequest.UseConsolidatedExecs = true;

                getExecutiveScreeningListExRequest.ContentSets.ContentSetsCollection.Add(ContentSet.Core);

                ServiceResponse serviceResponse = ScreeningService.GetExecutiveScreeningListEx(ControlDataManager.Clone(controlData), getExecutiveScreeningListExRequest);
                object responseObj;
                CheckServiceResponse(serviceResponse, out responseObj);

                GetExecutiveScreeningListExResponse getExecutiveScreeningListExResponse = (GetExecutiveScreeningListExResponse)responseObj;

                string[] values = (from e in getExecutiveScreeningListExResponse.GetExecutiveScreeningListExResult.ExecutiveScreeningListResult.Executives.ExecutiveCollection
                                   select e.ExFcode).ToArray();

                return values;
            }
            finally
            {
                //if (Logger.IsDebugEnabled) LogMethodEnd();
            }
        }

        public companyNewsQueryCollection GetCompanyNewsQueryFromCodes(List<string> companyCodes)
        {
           // if (Logger.IsDebugEnabled) LogMethodStart();

            try
            {
                LastOperationTruncated = false;

                codeCollection c = new codeCollection();

                c.AddRange(companyCodes);
                

                return GetCompanyNewsQueryFromCodes(c);
            }
            finally
            {
                //if (Logger.IsDebugEnabled) LogMethodEnd();
            }
        }
        public List<string> GetAuthorCodesFromAuthorId(long authorId)
        {
            List<string> personCodes = new List<string> { "810519", "2176454", "1861760", "179666"};
            return personCodes;

        }
        public companyNewsQueryCollection GetCompanyNewsQueryFromCodes(codeCollection companyCodes)
        {
           // if (Logger.IsDebugEnabled) LogMethodStart();

            try
            {
                LastOperationTruncated = false;

                GetCompanyNewsCodingRequest objGetCompanyNewsCodingRequest = new GetCompanyNewsCodingRequest();

                NewsCodingRequest objNewsCodingRequest = new NewsCodingRequest();
                objNewsCodingRequest.codeScheme = ListCodeScheme.Code;

                
                objNewsCodingRequest.codeCollection.AddRange(companyCodes);
                

                objGetCompanyNewsCodingRequest.request = objNewsCodingRequest;

                ServiceResponse serviceResponse = ScreeningService.GetCompanyNewsCoding(ControlDataManager.Clone(controlData), objGetCompanyNewsCodingRequest);
                object responseObj;
                
                CheckServiceResponse(serviceResponse, out responseObj);

                GetCompanyNewsCodingResponse objGetCompanyNewsCodingResponse = (GetCompanyNewsCodingResponse)responseObj;
                return objGetCompanyNewsCodingResponse.GetCompanyNewsCodingResult.companyNewsQueryCollection;
            }
            finally
            {
                //if (Logger.IsDebugEnabled) LogMethodEnd();
            }
        }
        protected void CheckServiceResponse(ServiceResponse serviceResponse, out object responseObject)
        {
            if (serviceResponse.rc != 0)
            {
            //    //if (Logger.IsErrorEnabled) LogMethodError(serviceResponse.rc, "rc", 2);
                throw new DowJonesUtilitiesException(serviceResponse.rc);
            }
            else
            {
                long responseObjRC = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObject);
                if (responseObjRC != 0)
                {
                //  if (Logger.IsErrorEnabled) LogMethodError(responseObjRC, "responseObjRC", 2);
                 throw new DowJonesUtilitiesException(responseObjRC.ToString());
                }
            }
        }
        
        public Query GetQueryById(long queryId)
        {
            GetQueryByIDRequest request = new GetQueryByIDRequest();
            request.Id = queryId;

            ServiceResponse serviceResponse = QueryService.GetQueryByID(ControlDataManager.Clone(controlData), request);
            object responseObj;

            CheckServiceResponse(serviceResponse, out responseObj);

            GetQueryByIDResponse objGetQueryByIdResponse = (GetQueryByIDResponse)responseObj;
            return objGetQueryByIdResponse.Query;
        }
        #region Get Lists
        public string[] GetCompanyListByID(string companyListId)
        {
//            if (Logger.IsDebugEnabled) LogMethodStart();

            try
            {
                GetCompanyListsByIDRequest request = new GetCompanyListsByIDRequest();
                request.AddId(companyListId);

                ServiceResponse serviceResponse = CompanyListService.GetCompanyListsByID(ControlDataManager.Clone(controlData), request);

                object responseObj;
                CheckServiceResponse(serviceResponse, out responseObj);

                GetCompanyListsByIDResponse getCompanyListsByIDResponse = (GetCompanyListsByIDResponse)responseObj;
                if (getCompanyListsByIDResponse.companyListsByIDResultCollection[0].status == 0)
                {
                    // return a list of fcodes
                    return (from c in getCompanyListsByIDResponse.companyListsByIDResultCollection[0].companyList.CompanyCodeCollection
                            select c).ToArray();
                }
                else
                {
                    string err = string.Format("Failed to retrieve company list id {0}, status: {1}", companyListId, getCompanyListsByIDResponse.companyListsByIDResultCollection[0].status.ToString());

                    //if (Logger.IsErrorEnabled) LogMethodError(err);

                    // throw exception
                    throw new Exception(err);
                }
            }
            finally
            {
                //if (Logger.IsDebugEnabled) LogMethodEnd();
            }
        }

        private static ExecutiveListItem convertToExecutiveListItem(ListItem item)
        {
            return (ExecutiveListItem)item;
        }
        //private static SourceGroupListItem convertToSourceGroupListItem(ListItem item)
        //{
        //    return (SourceGroupListItem)item;
        //}
        //public string[] GetSourceListItemsByID(long sourceListId)
        //{
        //    //if (Logger.IsDebugEnabled) LogMethodStart();

        //    try
        //    {
        //        GetListByIDRequest request = new GetListByIDRequest();
        //        request.Id = sourceListId;

        //        ServiceResponse serviceResponse = ListService.GetListByID(ControlDataManager.Clone(controlData), request);
        //        object responseObj;
        //        CheckServiceResponse(serviceResponse, out responseObj);

        //        GetListByIDResponse getListByIDResponse = (GetListByIDResponse)responseObj;

        //        SourceGroupList sourceGroupList = (SourceGroupList)getListByIDResponse.List;

        //        string temp = GeneralUtils.serialize(sourceGroupList, typeof(SourceGroupList));
        //        Console.WriteLine(temp);
        //        var listItems =
        //            sourceGroupList.Items.ConvertAll(new Converter<ListItem, SourceGroupListItem>(convertToSourceGroupListItem));
        //        //temp = GeneralUtils.serialize(listItems.FirstOrDefault().SourceGroupDefnition);
        //        //Console.WriteLine(temp);
        //        return (from li in listItems
        //                select li.SourceName).ToArray();
        //    }
        //    finally
        //    {
        //        //if (Logger.IsDebugEnabled) LogMethodEnd();
        //    }
        //}

        public string[] GetExecutiveListItemsByID(long executiveListId)
        {
            //if (Logger.IsDebugEnabled) LogMethodStart();

            try
            {
                GetListByIDRequest request = new GetListByIDRequest();
                request.Id = executiveListId;

                ServiceResponse serviceResponse = ListService.GetListByID(ControlDataManager.Clone(controlData), request);
                object responseObj;
                CheckServiceResponse(serviceResponse, out responseObj);

                GetListByIDResponse getListByIDResponse = (GetListByIDResponse)responseObj;
                

                ExecutiveList executiveList = (ExecutiveList)getListByIDResponse.List;

                var listItems =
                    executiveList.Items.ConvertAll(new Converter<ListItem, ExecutiveListItem>(convertToExecutiveListItem));
                
                return (from li in listItems
                        select li.ItemCode).ToArray();
            }
            finally
            {
                //if (Logger.IsDebugEnabled) LogMethodEnd();
            }
        }
        #endregion
        public companyNewsQueryCollection GetCompanyNewsQueryFromListId(string companyListId)
        {
            //if (Logger.IsDebugEnabled) LogMethodStart();

            try
            {
                

                string[] codes = GetCompanyListByID(companyListId);

                codeCollection companyCodes = new codeCollection();

                foreach (string s in codes) companyCodes.Add(s);

                return GetCompanyNewsQueryFromCodes(companyCodes);
            }
            finally
            {
                //if (Logger.IsDebugEnabled) LogMethodEnd();
            }
        }

        public companyNewsQueryCollection GetCompanyNewsQueryFromCompanyListId(string listId,RuleManager ruleManager)
        {
            //if (Logger.IsDebugEnabled) LogMethodStart();

            try
            {
                LastOperationTruncated = false;

                
                GetCompanyNewsCodingRequest objGetCompanyNewsCodingRequest = new GetCompanyNewsCodingRequest();
                ListRule rule = (ListRule)ruleManager.GetRuleByFilterType(FilterType.CompanyListFilter,typeof(ListRule));

                if (rule != null)
                    objGetCompanyNewsCodingRequest.maxResultsToReturn = rule.MaxValueToProcess;

                NewsCodingRequest objNewsCodingRequest = new NewsCodingRequest
                {
                    codeScheme = ListCodeScheme.ListID
                };
                objNewsCodingRequest.codeCollection = new codeCollection();
                objNewsCodingRequest.codeCollection.Add(listId);

                objGetCompanyNewsCodingRequest.request = objNewsCodingRequest;

                ServiceResponse serviceResponse = ScreeningService.GetCompanyNewsCoding(ControlDataManager.Clone(controlData), objGetCompanyNewsCodingRequest);
                object responseObj;
                CheckServiceResponse(serviceResponse, out responseObj);

                GetCompanyNewsCodingResponse objGetCompanyNewsCodingResponse = (GetCompanyNewsCodingResponse)responseObj;

                companyNewsQueryCollection rv = objGetCompanyNewsCodingResponse.GetCompanyNewsCodingResult.companyNewsQueryCollection;

                //if (MaximumCompaniesFromCriteriaToProcess > 0 && rv != null && rv.Count >= MaximumCompaniesFromCriteriaToProcess)
                //{
                //    LastOperationTruncated = true;

                //    companyNewsQueryCollection temp = new companyNewsQueryCollection();
                //    temp.AddRange(rv.Take(MaximumCompaniesFromCriteriaToProcess));
                //    rv = temp;
                //}

                return rv;
            }
            finally
            {
                //if (Logger.IsDebugEnabled) LogMethodEnd();
            }
        }

        public companyNewsQueryCollection GetCompanyNewsQueryFromScreeningCriteriaId(string criteriaId,RuleManager ruleManager)
        {
            //if (Logger.IsDebugEnabled) LogMethodStart();

            try
            {
                LastOperationTruncated = false;

                GetCompanyNewsCodingRequest objGetCompanyNewsCodingRequest = new GetCompanyNewsCodingRequest();
                ScreeningRule rule = (ScreeningRule)ruleManager.GetRuleByFilterType(FilterType.CompanyScreeningFilter, typeof(ScreeningRule));

                if (rule != null)
                    objGetCompanyNewsCodingRequest.maxResultsToReturn = rule.MaxValueToProcess;

                
                NewsCodingRequest objNewsCodingRequest = new NewsCodingRequest
                {
                    codeScheme = ListCodeScheme.ScreeningCriteriaID
                };
                objNewsCodingRequest.codeCollection = new codeCollection();
                objNewsCodingRequest.codeCollection.Add(criteriaId);

                objGetCompanyNewsCodingRequest.request = objNewsCodingRequest;

                ServiceResponse serviceResponse = ScreeningService.GetCompanyNewsCoding(ControlDataManager.Clone(controlData), objGetCompanyNewsCodingRequest);
                object responseObj;
                CheckServiceResponse(serviceResponse, out responseObj);

                GetCompanyNewsCodingResponse objGetCompanyNewsCodingResponse = (GetCompanyNewsCodingResponse)responseObj;

                companyNewsQueryCollection rv = objGetCompanyNewsCodingResponse.GetCompanyNewsCodingResult.companyNewsQueryCollection;

                               

                return rv;
            }
            finally
            {
                //if (Logger.IsDebugEnabled) LogMethodEnd();
            }
        }

        public PersonNewsQueryCollection GetPersonNewsQueryFromCodes(List<string> personCodes)
        {
           // if (Logger.IsDebugEnabled) LogMethodStart();

            try
            {
                LastOperationTruncated = false;

                codeCollection c = new codeCollection();

               
                    c.AddRange(personCodes);
               

                return GetPersonNewsQueryFromCodes(c);
            }
            finally
            {
               // if (Logger.IsDebugEnabled) LogMethodEnd();
            }
        }

        public PersonNewsQueryCollection GetPersonNewsQueryFromCodes(codeCollection personCodes)
        {
            //if (Logger.IsDebugEnabled) LogMethodStart();

            try
            {
                LastOperationTruncated = false;

                GetPersonNewsCodingRequest objGetPersonNewsCodingRequest = new GetPersonNewsCodingRequest();

                NewsCodingRequest objNewsCodingRequest = new NewsCodingRequest();
                objNewsCodingRequest.codeScheme = ListCodeScheme.Code;


                
                    objNewsCodingRequest.codeCollection.AddRange(personCodes);
                

                objGetPersonNewsCodingRequest.request = objNewsCodingRequest;

                ServiceResponse serviceResponse = ScreeningService.GetPersonNewsCoding(ControlDataManager.Clone(controlData), objGetPersonNewsCodingRequest);
                object responseObj;
                CheckServiceResponse(serviceResponse, out responseObj);

                GetPersonNewsCodingResponse objGetPersonNewsCodingResponse = (GetPersonNewsCodingResponse)responseObj;
                return objGetPersonNewsCodingResponse.GetPersonNewsCodingResult.PersonNewsQueryCollection;
            }
            finally
            {
                //if (Logger.IsDebugEnabled) LogMethodEnd();
            }
        }

        public PersonNewsQueryCollection GetPersonNewsQueryFromListId(long executiveListId)
        {
            // if (Logger.IsDebugEnabled) LogMethodStart();

            try
            {
                //ListAssetWrapper wrapper = new ListAssetWrapper() { ControlData = PAMControlData };

                string[] codes = GetExecutiveListItemsByID(executiveListId);

                codeCollection personCodes = new codeCollection();

                foreach (string s in codes) personCodes.Add(s);

                return GetPersonNewsQueryFromCodes(personCodes);
            }
            finally
            {
                //if (Logger.IsDebugEnabled) LogMethodEnd();
            }
        }
        /*public List<sourceNewsQuery> GetSourceNewsQueriesFromSourceGroup(SourceGroup sg)
        {
            List<sourceNewsQuery> sourceNewsQueries = new List<sourceNewsQuery>();

            try {
                if (sg.SourceListId > 0)
                {
                    // get the sourcenames from backend and fill sourcenewsqueries
                    sourceNewsQueries.Add(new sourceNewsQuery { searchFormatCategory = SearchFormatCategory.Publications, sourceName = new List<string> { "The Wall Street Journal", "The New York Post" } });
                    sourceNewsQueries.Add(new sourceNewsQuery { searchFormatCategory = SearchFormatCategory.WebNews, sourceName = new List<string> { "Accommodation Times" } });
                }
                else { 
                    sourceNewsQueries.Add(new sourceNewsQuery { searchFormatCategory = sg.SearchFormatCategory, sourceId = sg.SourceCodeCollection});
                }
            }
            finally { 
            
            }
            return sourceNewsQueries;
        }
         */
        public PersonNewsQueryCollection GetPersonNewsQueryFromScreeningCriteriaId(string criteriaId,RuleManager ruleManager)
        {
            //if (Logger.IsDebugEnabled) LogMethodStart();

            try
            {
                LastOperationTruncated = false;

                GetPersonNewsCodingRequest objGetPersonNewsCodingRequest = new GetPersonNewsCodingRequest();
                ScreeningRule rule = (ScreeningRule)ruleManager.GetRuleByFilterType(FilterType.ExecutiveScreeningFilter, typeof(ScreeningRule));

                if (rule != null)
                    objGetPersonNewsCodingRequest.maxResultsToReturn = rule.MaxValueToProcess;

                NewsCodingRequest objNewsCodingRequest = new NewsCodingRequest
                {
                    codeScheme = ListCodeScheme.ScreeningCriteriaID
                };

                objNewsCodingRequest.codeCollection = new codeCollection();
                objNewsCodingRequest.codeCollection.Add(criteriaId);

                objGetPersonNewsCodingRequest.request = objNewsCodingRequest;

                ServiceResponse serviceResponse = ScreeningService.GetPersonNewsCoding(ControlDataManager.Clone(controlData), objGetPersonNewsCodingRequest);
                object responseObj;
                CheckServiceResponse(serviceResponse, out responseObj);

                GetPersonNewsCodingResponse objGetPersonNewsCodingResponse = (GetPersonNewsCodingResponse)responseObj;

                PersonNewsQueryCollection rv = objGetPersonNewsCodingResponse.GetPersonNewsCodingResult.PersonNewsQueryCollection;

                

                return rv;
            }
            finally
            {
                //if (Logger.IsDebugEnabled) LogMethodEnd();
            }
        }
    }
}
