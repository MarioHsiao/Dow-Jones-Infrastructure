using Factiva.Gateway.Messages.Search.V2_0;
using System.Collections.Generic;
using Factiva.Gateway.V1_0;
using Factiva.Gateway.Utils.V1_0;
using DowJones.Exceptions;
using System.Linq;

namespace DowJones.Managers.Search.MetaDataSearch
{
    public class FIICodeInfoUtility
    {
        private const string DefaultLanguage = "en";

        public List<FIICodeInfo> GetFIICodesInfo(ControlData controlData, List<string> companyCodes, List<string> regionCodes, List<string> industryCodes, List<string> subjectCodes)
        {
            return GetFIICodesInfo(controlData, companyCodes, regionCodes, industryCodes, subjectCodes, DefaultLanguage);
        }

        public List<FIICodeInfo> GetFIICodesInfo(ControlData controlData, List<string> companyCodes, List<string> regionCodes, List<string> industryCodes, List<string> subjectCodes, string language)
        {            
            
            #region Separate Backend calls
            PerformMetadataSearchResponse CompanyResponse=null, RegionResponse =null, IndustryResponse=null, SubjectResponse=null;
            if (companyCodes != null && companyCodes.Count > 0)
                CompanyResponse = PerformMetadataSearch(controlData, companyCodes, new List<MetadataCollection> { MetadataCollection.Company }, language);
            if (regionCodes != null && regionCodes.Count >0)
                RegionResponse = PerformMetadataSearch(controlData, regionCodes, new List<MetadataCollection>{ MetadataCollection.Region}, language);
            if (industryCodes !=null && industryCodes.Count > 0)
                IndustryResponse = PerformMetadataSearch(controlData, industryCodes,new List<MetadataCollection>{ MetadataCollection.Industry}, language);
            if (subjectCodes !=null && subjectCodes.Count > 0)
                SubjectResponse = PerformMetadataSearch(controlData, subjectCodes, new List<MetadataCollection>{MetadataCollection.NewsSubject}, language);

            List<FIICodeInfo> CompanyList = MapMetadataSearchResponse(CompanyResponse, companyCodes, FIICodeType.Company);
            List<FIICodeInfo> RegionList = MapMetadataSearchResponse(RegionResponse, regionCodes,FIICodeType.Region);
            List<FIICodeInfo> IndustryList = MapMetadataSearchResponse(IndustryResponse, industryCodes,FIICodeType.Industry);
            List<FIICodeInfo> SubjectList = MapMetadataSearchResponse(SubjectResponse, subjectCodes,FIICodeType.Subject);

            return RegionList.Concat(IndustryList).Concat(SubjectList).Concat(CompanyList).ToList();
            #endregion
            
            //#region Combined Backend call
            //List<string> codes = (regionCodes == null ? new List<string>() : regionCodes).Concat(industryCodes == null ? new List<string>() : industryCodes).Concat(subjectCodes == null ? new List<string>() : subjectCodes).ToList();
            //PerformMetadataSearchResponse response = PerformMetadataSearch(controlData, codes, new List<MetadataCollection>{ MetadataCollection.Region,MetadataCollection.Industry,MetadataCollection.NewsSubject}, language);
            //return MapMetadataSearchResponse(response, regionCodes,industryCodes,subjectCodes);
            //#endregion
        }

        private PerformMetadataSearchRequest CreateRequest(List<string> codes, string language, List<MetadataCollection> types)
        {
            PerformMetadataSearchRequest request = new PerformMetadataSearchRequest
            {
                FirstResult = 0,
                MaxResults = codes.Count                
            };
            request.StructuredSearch.Query.QueryLanguage = language;
            request.StructuredSearch.Query.ResultLanguage = language;
            foreach(var type in types)
                request.StructuredSearch.Query.MetadataCollectionCollection.Add(type);
            request.StructuredSearch.Version = "2.7";
            request.StructuredSearch.Query.SearchStringCollection.Add(new MetadataSearchString
            {
                //Id = "SearchValue",
                Mode = MetadataSearchMode.Any,
                Type = SearchType.Free,
                Scope = "fcode",
                Value = string.Join(" ", codes.ToArray())
            });

            //request.StructuredSearch.Query.SearchStringCollection.Add(new MetadataSearchString
            //{
            //    Id = "SearchLanguage",
            //    Mode = MetadataSearchMode.Any,
            //    Type = SearchType.Free,
            //    Scope = "la",
            //    Value = language
            //});

            request.StructuredSearch.Formatting.ResponseType.Private = false;
            request.StructuredSearch.Formatting.ResponseType.Value =  ResponseDataSet.Small;
            request.StructuredSearch.Formatting.SortOrder =  MetadataResultSortOrder.Name;

            return request;
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

        private List<FIICodeInfo> MapMetadataSearchResponse(PerformMetadataSearchResponse objPerformMetadataSearchResponse,List<string> inputCodes, FIICodeType type)
        {
            List<FIICodeInfo> codes = new List<FIICodeInfo>();

            if (objPerformMetadataSearchResponse !=null && objPerformMetadataSearchResponse.MetadataSearchResult != null && objPerformMetadataSearchResponse.MetadataSearchResult.MetadataResultSet != null && objPerformMetadataSearchResponse.MetadataSearchResult.MetadataResultSet.MetadataInfoCollection.Count > 0)
            {
                foreach (var metadatainfo in objPerformMetadataSearchResponse.MetadataSearchResult.MetadataResultSet.MetadataInfoCollection)
                {
                    codes.Add(new FIICodeInfo { Code = metadatainfo.Id, Value = metadatainfo.Name });
                }
            }

            FillFIICodeType(codes, inputCodes, type);
            //FillFIICodeType(codes, industryCodes, FIICodeType.Industry);
            //FillFIICodeType(codes, subjectCodes, FIICodeType.Subject);
            //if (inputCodes != null)
            //{
            //    if (codes.Count < inputCodes.Count)
            //    {
            //        foreach (string code in inputCodes)
            //        {
            //            var s = (from fiicode in codes
            //                     where fiicode.Code == code
            //                     select fiicode).Distinct().ToList();
            //            if (s.Count <= 0)
            //            {
            //                codes.Add(new FIICodeInfo { Code = code, Value = "" });
            //            }
            //        }
            //    }
            //}
            return codes;
        }

        private void FillFIICodeType(List<FIICodeInfo> codes, List<string> inputCodes,FIICodeType type)
        {
            if (inputCodes != null)
            {
                foreach (var code in inputCodes)
                {
                    var s = (from fiicode in codes
                             where fiicode.Code.ToLower() == code.ToLower()
                             select fiicode).Distinct().ToList();
                    if (s.Count <= 0)
                    {
                        codes.Add(new FIICodeInfo { Code = code, Value = "", Type = type });
                    }
                    else
                    {
                        s.FirstOrDefault().Type = type;
                    }
                }
            }
        }
        private PerformMetadataSearchResponse PerformMetadataSearch(ControlData controlData,List<string> codes, List<MetadataCollection> types, string language)
        {
            ServiceResponse serviceResponse = Factiva.Gateway.Services.V2_0.SearchService.PerformMetadataSearch(controlData.Clone(), CreateRequest(codes, language, types));
            object responseObj;
            
            CheckServiceResponse(serviceResponse, out responseObj);

            PerformMetadataSearchResponse objPerformMetadataSearchResponse = (PerformMetadataSearchResponse)responseObj;
            return objPerformMetadataSearchResponse;
        }
            
    }
}
