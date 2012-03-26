//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml.Linq;
//using DowJones.Utilities.Managers.Core;
//using Factiva.Gateway.Messages.Assets.Pages.V1_0;
//using Factiva.Gateway.Messages.Assets.Queries.V1_0;
//using Factiva.Gateway.Messages.Preferences.V1_0;
//using Factiva.Gateway.Messages.Screening.V1_1;
//using Factiva.Gateway.Services.V1_0;
//using Factiva.Gateway.Utils.V1_0;
//using Factiva.Gateway.V1_0;
//using log4net;

//namespace DowJones.Utilities.Managers.DJConsultant
//{
//    public class NameValuePair
//    {
//        public string Name;
//        public string Value;
//    }

//    public class DJConsultantManager : AbstractAggregationManager
//    {
//        private static readonly ILog _log = LogManager.GetLogger(typeof (DJConsultantManager));
//        private static readonly ResourceTextManager RT = ResourceTextManager.Instance;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="DJConsultantManager"/> class.
//        /// </summary>
//        /// <param name="controlData">The control data.</param>
//        /// <param name="interfaceLanguage">The interface language.</param>
//        public DJConsultantManager(ControlData controlData, string interfaceLanguage)
//            : base(controlData)
//        {
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="DJConsultantManager"/> class.
//        /// </summary>
//        /// <param name="sessionID">The session ID.</param>
//        /// <param name="clientTypeCode">The client type code.</param>
//        /// <param name="accessPointCode">The access point code.</param>
//        /// <param name="interfaceLanguage">The interface language.</param>
//        public DJConsultantManager(string sessionID, string clientTypeCode, string accessPointCode,
//                                   string interfaceLanguage)
//            : base(sessionID, clientTypeCode, accessPointCode)
//        {
//        }

//        /// <summary>
//        /// Gets the log.
//        /// </summary>
//        /// <value>The log.</value>
//        protected override ILog Log
//        {
//            get { return _log; }
//        }

//        /// <summary>
//        /// Returns consultant practice area list, sorted in the order of the translated name.
//        /// Value All will be on the top
//        /// </summary>
//        public static List<NameValuePair> GetSortedPracticeAreaList()
//        {
//            var practiceAreas = new SortedDictionary<string, ConsultantPracticeArea>();
//            foreach (ConsultantPracticeArea pa in Enum.GetValues(typeof (ConsultantPracticeArea)))
//                practiceAreas.Add(RT.GetString(string.Format("{0}_token", pa)), pa);

//            var l = new List<NameValuePair>();
//            foreach (var pa in practiceAreas)
//                if (pa.Value == ConsultantPracticeArea.PA_ALL)
//                    l.Insert(0, new NameValuePair {Name = pa.Key, Value = pa.Value.ToString()});
//                else
//                    l.Add(new NameValuePair {Name = pa.Key, Value = pa.Value.ToString()});
//            return l;
//        }

//        public static ModuleCollection GetDefaultModuleCollectionByPageModuleType(PreferenceDJCProfile djcProfile, IEnumerable<XElement> dashboardDefaultModules, string pageType)
//        {
//            var moduleCollection = new ModuleCollection();

//            var _pos = 0;
//            foreach (var aModuleType in dashboardDefaultModules)
//            {
//                var modId = aModuleType.Attribute("modId");
//                if ( modId != null && modId.Value != null)
//                {
//                    // ReSharper disable PossibleNullReferenceException
//                    switch (aModuleType.Attribute("modId").Value)
//                        // ReSharper restore PossibleNullReferenceException
//                    {
//                        case "NewsOpinionAnalysisModule":
//                            var _noaModType = NOAModuleType.Search; //SM_TODO: waht is right thing to do?
//                            var _code = GetNOAModuleTypeAndIDBasedOnIndustry(aModuleType,
//                                                                                djcProfile.SelectedIndustries.Industries[0],
//                                                                                ref _noaModType);
//                            var analysisModule = new NewsOpinionAnalysisModule
//                                                     {
//                                                         RetrievalType = _noaModType,
//                                                         HeadlineCount = DJConsultantConfiguration.NUMBER_OF_DEFAULT_HEADLINES,
//                                                         Position = _pos,
//                                                         DefaultModuleCode = _code,
//                                                         ModuleState = ModuleState.Maximized
//                                                     };
//                            moduleCollection.Add(analysisModule);
//                            break;
//                        case "MostRecentPracticeAreaNewsModule":
//                            var mostRecentPracticeAreaNewsModule = new MostRecentPracticeAreaNewsModule
//                                                                       {
//                                                                           PracticeAreaCode = MapPracticeAreaCode(djcProfile.SelectedPracticeAreas.PracticeAreas[0]),
//                                                                           HeadlineCount = DJConsultantConfiguration.NUMBER_OF_DEFAULT_HEADLINES,
//                                                                           Position = _pos,
//                                                                           ModuleState = ModuleState.Maximized
//                                                                       }
//                                ;
//                            moduleCollection.Add(mostRecentPracticeAreaNewsModule);
//                            break;
//                        case "TriggersModule":
//                            var triggersModule = new TriggersModule
//                                                     {
//                                                         TriggerTypeCode = GetDefaultTriggerTypeCode(djcProfile.SelectedPracticeAreas.PracticeAreas[0], pageType),
//                                                         HeadlineCount = DJConsultantConfiguration.NUMBER_OF_DEFAULT_HEADLINES,
//                                                         Position = _pos,
//                                                         ModuleState = ModuleState.Maximized
//                                                     }
//                                ;
//                            moduleCollection.Add(triggersModule);
//                            break;
//                        case "FISTopStoriesModule":
//                            var fisTopStoriesModule = new FISTopStoriesModule
//                                                          {
//                                                              //START: Infosys 09/02/2010 - Changes for combining Build and Advice Dashboards to My Page
//                                                              FisTopStoriesTypeCode = FISTopStoriesTypeCode.FISStories,
//                                                              //END: Infosys 09/02/2010 - Changes for combining Build and Advice Dashboards to My Page
//                                                              HeadlineCount = DJConsultantConfiguration.NUMBER_OF_DEFAULT_HEADLINES,
//                                                              Position = _pos,
//                                                              ModuleState = ModuleState.Maximized
//                                                          }
//                                ;
//                            moduleCollection.Add(fisTopStoriesModule);
//                            break;

//                        case "BusinessOpportunitiesModule":
//                            var businessOpportunitiesModule = new BusinessOpportunitiesModule //everything is based on the PA
//                                                                  {
//                                                                      HeadlineCount = DJConsultantConfiguration.NUMBER_OF_DEFAULT_HEADLINES,
//                                                                      Position = _pos,
//                                                                      ModuleState = ModuleState.Maximized
//                                                                  }
//                                ;
//                            moduleCollection.Add(businessOpportunitiesModule);
//                            break;
//                        case "CompanyRadarModule":
//                            var companyRadarModule = new CompanyRadarModule //everything is based on the companies in lens
//                                                         {
//                                                             Position = _pos,
//                                                             ModuleState = ModuleState.Maximized
//                                                         }
//                                ;
//                            moduleCollection.Add(companyRadarModule);
//                            break;
//                        case "MostRecentIndustryNewsModule":
//                            var mostRecentIndustryNewsModule = new MostRecentIndustryNewsModule
//                                //everything is based on the IND in lens
//                                                                   {
//                                                                       HeadlineCount = DJConsultantConfiguration.NUMBER_OF_DEFAULT_HEADLINES,
//                                                                       Position = _pos,
//                                                                       ModuleState = ModuleState.Maximized
//                                                                   }
//                                ;
//                            moduleCollection.Add(mostRecentIndustryNewsModule);
//                            break;
//                        case "MostRecentCompanyNewsModule":
//                            var mostRecentCompanyNewsModule = new MostRecentCompanyNewsModule
//                                //everything is based on the Companies in lens
//                                                                  {
//                                                                      HeadlineCount = DJConsultantConfiguration.NUMBER_OF_DEFAULT_HEADLINES,
//                                                                      Position = _pos,
//                                                                      ModuleState = ModuleState.Maximized
//                                                                  }
//                                ;
//                            moduleCollection.Add(mostRecentCompanyNewsModule);
//                            break;

//                        case "PracticeAreaDiscoveryModule":
//                            var practiceAreaDiscoveryModule = new PracticeAreaDiscoveryModule
//                                //everything is based on the PA in lens
//                                                                  {
//                                                                      Position = _pos,
//                                                                      ModuleState = ModuleState.Maximized
//                                                                  }
//                                ;
//                            moduleCollection.Add(practiceAreaDiscoveryModule);
//                            break;
//                        case "IndustryDiscoveryModule":
//                            var industryDiscoveryModule = new IndustryDiscoveryModule
//                                //everything is based on the IND in lens
//                                                              {
//                                                                  Position = _pos,
//                                                                  ModuleState = ModuleState.Maximized
//                                                              }
//                                ;
//                            moduleCollection.Add(industryDiscoveryModule);
//                            break;
//                        case "AnalysisAndProfileForIndustryModule":
//                            var anpForIndustryModule = new AnalysisAndProfileForIndustryModule
//                                //everything is based on the IND in lens
//                                                           {
//                                                               HeadlineCount = DJConsultantConfiguration.NUMBER_OF_DEFAULT_HEADLINES,
//                                                               Position = _pos,
//                                                               ModuleState = ModuleState.Maximized
//                                                           }
//                                ;
//                            moduleCollection.Add(anpForIndustryModule);
//                            break;
//                    }

//                    //try
//                    //{
//                    //    Module aModule = CreateModule(aModuleType.Attribute("modId").Value);
//                    //    aModule.Position = 0;
//                    //    aModule.Head
//                    //}
//                    //catch (Exception ex)
//                    //{
//                    //    UiContextLogger.Debug(ex);
//                    //    UiContextLogger.Debug("Unable to load  Module of type:" + aModuleType.Attribute("modId").Value);
//                    //}
//                }
//                _pos++; //increment for the position;
//            }
//            return moduleCollection;
//        }

//        //START: Infosys 09/02/2010 - Changes for combining Build and Advice Dashboards to My Page
//        /// <summary>
//        /// Gets the default fis top stories type code.
//        /// </summary>
//        /// <param name="pageType">Type of the page.</param>
//        /// <returns></returns>
//        //private static FISTopStoriesTypeCode GetDefaultFisTopStoriesTypeCode(string pageType)
//        //{
//        //    switch (pageType.ToUpper())
//        //    {
//        //        case DJConsultantConfiguration.ADVISE_PageName:
//        //            return FISTopStoriesTypeCode.FISStories;
//        //        case DJConsultantConfiguration.BUILD_FO_PageName:
//        //            return FISTopStoriesTypeCode.FISStories;
//        //    }
//        //    return FISTopStoriesTypeCode.FISStories;
//        //}
//        //END: Infosys 09/02/2010 - Changes for combining Build and Advice Dashboards to My Page

//        /// <summary>
//        /// Gets the default trigger type code.
//        /// </summary>
//        /// <param name="practiceArea">The practice area.</param>
//        /// <param name="pageType">Type of the page.</param>
//        /// <returns></returns>
//        private static TriggerTypeCode GetDefaultTriggerTypeCode(ConsultantPracticeArea practiceArea, string pageType)
//        {
//            //START: Infosys 09/02/2010 - Changes for combining Build and Advice Dashboards to My Page
//            switch (practiceArea)
//            {
//                case ConsultantPracticeArea.PA_ALL:
//                    if (pageType.ToUpper() == DJConsultantConfiguration.MYPAGE_PageName)
//                    {
//                        return TriggerTypeCode.DJFinancialAnnouncement;
//                    }
//                    break;
//                case ConsultantPracticeArea.PA_ECON_LEGAL:
//                    if (pageType.ToUpper() == DJConsultantConfiguration.MYPAGE_PageName)
//                    {
//                        return TriggerTypeCode.DJFinancialAnnouncement;
//                    }
//                    break;
//                case ConsultantPracticeArea.PA_HR:
//                    if (pageType.ToUpper() == DJConsultantConfiguration.MYPAGE_PageName)
//                    {
//                        return TriggerTypeCode.DJFinancialAnnouncement;
//                    }
//                    break;
//                case ConsultantPracticeArea.PA_IT:
//                    if (pageType.ToUpper() == DJConsultantConfiguration.MYPAGE_PageName)
//                    {
//                        return TriggerTypeCode.DJBankruptcy;
//                    }
//                    break;
//                case ConsultantPracticeArea.PA_MKTNG:
//                    if (pageType.ToUpper() == DJConsultantConfiguration.MYPAGE_PageName)
//                    {
//                        return TriggerTypeCode.DJFinancialAnnouncement;
//                    }
//                    break;
//                case ConsultantPracticeArea.PA_OPS:
//                    if (pageType.ToUpper() == DJConsultantConfiguration.MYPAGE_PageName)
//                    {
//                        return TriggerTypeCode.DJFinancialAnnouncement;
//                    }
//                    break;
//                case ConsultantPracticeArea.PA_RISK:
//                    if (pageType.ToUpper() == DJConsultantConfiguration.MYPAGE_PageName)
//                    {
//                        return TriggerTypeCode.DJFinancialAnnouncement;
//                    }
//                    break;
//                case ConsultantPracticeArea.PA_STRTGY:
//                    if (pageType.ToUpper() == DJConsultantConfiguration.MYPAGE_PageName)
//                    {
//                        return TriggerTypeCode.DJFinancialAnnouncement;
//                    }
//                    break;
//                case ConsultantPracticeArea.PA_TAX:
//                    if (pageType.ToUpper() == DJConsultantConfiguration.MYPAGE_PageName)
//                    {
//                        return TriggerTypeCode.DJFinancialAnnouncement;
//                    }
//                    break;
//            }
//            if (pageType.ToUpper() == DJConsultantConfiguration.MYPAGE_PageName)
//            {
//                return TriggerTypeCode.DJFinancialAnnouncement;
//            }
//            return TriggerTypeCode.DJMergerAcquisition;
//            //END: Infosys 09/02/2010 - Changes for combining Build and Advice Dashboards to My Page
//        }

//        /// <summary>
//        /// Gets the NOA module type and ID based on industry.
//        /// </summary>
//        /// <param name="moduleType">Type of the module.</param>
//        /// <param name="consultantIndustry">The consultant industry.</param>
//        /// <param name="noaModuleType">Type of the noa module.</param>
//        /// <returns></returns>
//        private static string GetNOAModuleTypeAndIDBasedOnIndustry(XElement moduleType, ConsultantIndustry consultantIndustry, ref NOAModuleType noaModuleType)
//        {
//            XElement _mod;
//            string _retCode = null;
//            /*
//             * Find the Module that is under NOAA Module Type for the industry provided. and which has defaultDahboar==true
//            */

//            var anIndustry = from aIndustry in moduleType.Elements("consultantIndustry")
//                                               where
//                                                   aIndustry.Attribute("id") != null &&
//                                                   aIndustry.Attribute("id").Value != null &&
//                                                   aIndustry.Attribute("id").Value == consultantIndustry.ToString()
//                                               select aIndustry;

//            var oneModule = from aModule in anIndustry.Elements("module")
//                                              where
//                                                  aModule.Attribute("isDefaultOnDashboard") != null &&
//                                                  aModule.Attribute("isDefaultOnDashboard").Value != null &&
//                                                  aModule.Attribute("isDefaultOnDashboard").Value == "true"
//                                              select aModule;

//            if (oneModule.Count() > 0)
//            {
//                _mod = oneModule.First();

//                //moduleType.Elements("consultantIndustry").Where(
//                //    aIndustry => aIndustry.Attribute("id").Value == consultantIndustry.ToString()).Elements("module").First(
//                //    aModule => aModule.Attribute("isDefaultOnDashboard") != null
//                //               &&
//                //               aModule.Attribute("isDefaultOnDashboard").Value == "true");

//                if (_mod != null && _mod.Attribute("id") != null)
//                {
//                    _retCode = _mod.Attribute("id").Value;


//                    if (String.IsNullOrEmpty(_retCode))
//                    {
//                        _mod = moduleType.Elements("consultantIndustry").Where(
//                            aIndustry => aIndustry.Attribute("id").Value == consultantIndustry.ToString()).Elements("module")
//                            .
//                            First();
//                        _retCode = _mod.Attribute("id").Value;
//                    }
//                    switch (_mod.Attribute("type").Value.ToLower())
//                    {
//                        case "collection":
//                            noaModuleType = NOAModuleType.Collections;
//                            break;
//                        case "search":
//                            noaModuleType = NOAModuleType.Search;
//                            break;
//                    }
//                }
//            }
//            return _retCode;
//        }

//        /// <summary>
//        /// Maps the practice area code from ConsultantPracticeArea to PracticeAreaCode(PAM))
//        /// </summary>
//        /// <param name="practiceArea">The practice area.</param>
//        /// <returns></returns>
//        /// //SM_TODO: Why do we have 2 enums doing the same...
//        private static PracticeAreaCode MapPracticeAreaCode(ConsultantPracticeArea practiceArea)
//        {
//            switch (practiceArea)
//            {
//                case ConsultantPracticeArea.PA_ALL:
//                    return PracticeAreaCode.PA_ALL;
//                case ConsultantPracticeArea.PA_ECON_LEGAL:
//                    return PracticeAreaCode.PA_ECON_LEGAL;
//                case ConsultantPracticeArea.PA_HR:
//                    return PracticeAreaCode.PA_HR;
//                case ConsultantPracticeArea.PA_IT:
//                    return PracticeAreaCode.PA_IT;
//                case ConsultantPracticeArea.PA_MKTNG:
//                    return PracticeAreaCode.PA_MKTNG;
//                case ConsultantPracticeArea.PA_OPS:
//                    return PracticeAreaCode.PA_OPS;
//                case ConsultantPracticeArea.PA_RISK:
//                    return PracticeAreaCode.PA_RISK;
//                case ConsultantPracticeArea.PA_STRTGY:
//                    return PracticeAreaCode.PA_STRTGY;
//                case ConsultantPracticeArea.PA_TAX:
//                    return PracticeAreaCode.PA_TAX;
//            }
//            return PracticeAreaCode.PA_ALL;
//        }

//        /// <summary>
//        /// Returns consultant region list, sorted in the order of the translated name.
//        /// Value All will be on the top
//        /// </summary>
//        public static List<NameValuePair> GetSortedRegionList()
//        {
//            var regions = new SortedDictionary<string, ConsultantRegion>();
//            foreach (ConsultantRegion r in Enum.GetValues(typeof (ConsultantRegion)))
//                regions.Add(RT.GetString(string.Format("{0}_token", r)), r);

//            var l = new List<NameValuePair>();
//            foreach (var r in regions)
//                if (r.Value == ConsultantRegion.REGION_ALL)
//                    l.Insert(0, new NameValuePair {Name = r.Key, Value = r.Value.ToString()});
//                else
//                    l.Add(new NameValuePair {Name = r.Key, Value = r.Value.ToString()});
//            return l;
//        }

//        /// <summary>
//        /// Returns consultant industry list, sorted in the order of the translated name.
//        /// Value All will be on the top
//        /// </summary>
//        public static List<NameValuePair> GetSortedIndustryList()
//        {
//            return GetSortedIndustryList(true);
//        }

//        public static List<NameValuePair> GetSortedIndustryList(bool includeAll)
//        {
//            var industries = new SortedDictionary<string, ConsultantIndustry>();
//            foreach (ConsultantIndustry i in Enum.GetValues(typeof(ConsultantIndustry)))
//                industries.Add(RT.GetString(string.Format("{0}_token", i)), i);

//            var l = new List<NameValuePair>();
//            foreach (var i in industries)
//                if (i.Value == ConsultantIndustry.IND_ALL)
//                {
//                    if (includeAll)
//                        l.Insert(0, new NameValuePair { Name = i.Key, Value = i.Value.ToString() });
//                }
//                else
//                    l.Add(new NameValuePair { Name = i.Key, Value = i.Value.ToString() });
//            return l;
//        }

//        public List<ConsultantLensQueries> GetConsultantLensQuery(LensQueryType[] QueryTypes, ReturnCodesOrQuery ReturnCodesOrQuery)
//        {
//            var request = new GetItemsByClassIDRequest
//                              {
//                                  ClassID = new[] {PreferenceClassID.DJCProfile}
//                              };
//            var _resp = PreferenceService.GetItemsByClassID(ControlDataManager.Clone(ControlData), request);
//            if (_resp != null && _resp.DJCProfile != null)
//            {
//                return GetConsultantLensQuery(_resp.DJCProfile.DJCProfile, QueryTypes, ReturnCodesOrQuery);
//            }
//            return null;
//        }

//        public List<ConsultantLensQueries> GetConsultantLensQuery(PreferenceDJCProfile DJCProfile,
//                                                                  LensQueryType[] queryTypes, ReturnCodesOrQuery returnCodesOrQuery)
//        {
//            //create the query and then give it back

//            var searchStrings = new List<ConsultantLensQueries>(4);
//            var _qiD = new List<string>();
//            if (DJCProfile != null) //need the lens to proceed
//            {
//                ConsultantLensQueries companyQuery;
//                foreach (var ty in queryTypes)
//                {
//                    switch (ty)
//                    {
//                        case LensQueryType.AllLens:
//                            //for all lens components.. co,pa, re and ind
//                            companyQuery = GetCompaniesFreeTextQuery(DJCProfile.SelectedCompanies, returnCodesOrQuery);
//                            if (companyQuery != null)
//                                searchStrings.Add(companyQuery);
//                            _qiD.Add(DJCProfile.SelectedIndustries.Industries[0].ToString()); //ind all
//                            _qiD.Add(DJCProfile.SelectedPracticeAreas.PracticeAreas[0].ToString()); //pa_risk
//                            _qiD.Add(DJCProfile.SelectedRegions.Regions[0].ToString()); //regionall
//                            break;
//                        case LensQueryType.Company:
//                            companyQuery = GetCompaniesFreeTextQuery(DJCProfile.SelectedCompanies, returnCodesOrQuery);
//                            if (companyQuery != null)
//                                searchStrings.Add(companyQuery);
//                            break;
//                        case LensQueryType.Industry:
//                            _qiD.Add(DJCProfile.SelectedIndustries.Industries[0].ToString()); //ind all
//                            break;
//                        case LensQueryType.PracticeArea:
//                            _qiD.Add(DJCProfile.SelectedPracticeAreas.PracticeAreas[0].ToString()); //pa_risk
//                            break;
//                        case LensQueryType.Region:
//                            _qiD.Add(DJCProfile.SelectedRegions.Regions[0].ToString()); //regionall

//                            break;
//                    }
//                }
//                searchStrings.AddRange(GetFreeTextFilters(_qiD));
//            }
//            return searchStrings;
//        }

//        public static List<string> GetRegionFilter(ConsultantRegion consultantRegion)
//        {

//            var regionCode =
//                    (ConsultantRegionCode[])
//                    Attribute.GetCustomAttributes(typeof(ConsultantRegion).GetField(consultantRegion.ToString()),
//                                                 typeof(ConsultantRegionCode));

//            var regionList = new List<string>();

//            foreach(var regionItem in regionCode)
//            {
//                regionList.Add(regionItem.RegionCode);
//            }

//            return regionList;
//        }

//        private ConsultantLensQueries GetCompaniesFreeTextQuery(SelectedCompanies _djcProfileCompanies, ReturnCodesOrQuery returnCodesOrQuery)
//        {
//            //if ALL companies, then a good as no filter
//            if (_djcProfileCompanies != null && _djcProfileCompanies.FocusType != CompanyFocusType.All)
//            {
//                var _cosString = new List<string>(10);
//                try
//                {
//                    _log.InfoFormat("CompanyFocusType:{0}", _djcProfileCompanies.FocusType.ToString());
//                    foreach (var aCo in _djcProfileCompanies.Companies)
//                    {
//                        _log.InfoFormat("ConsultantCompany from lens:{0};focal:{1},", aCo.Code, aCo.IsFocal);
//                        if (_djcProfileCompanies.FocusType == CompanyFocusType.SelectedFocal && aCo.IsFocal)
//                        {
//                            _cosString.Add(aCo.Code);
//                        }
//                    }

//                    var _compnyQuery = new ConsultantLensQueries();
//                    switch (returnCodesOrQuery)
//                    {
//                        case ReturnCodesOrQuery.Codes:
//                            _compnyQuery.ConsultantFreeTextQueryString = _cosString;
//                            break;
//                        case ReturnCodesOrQuery.FreeTextQuery:
//                            _compnyQuery.ConsultantFreeTextQueryString = new List<string>
//                                                                             {
//                                                                                 String.Format("fds=({0})",
//                                                                                               String.Join(" or ", _cosString.ToArray()))
//                                                                             };
//                            break;
//                        case ReturnCodesOrQuery.CompanyCodesAndNames:
//                            _compnyQuery.ConsultantFreeTextQueryString = SeparateNewsCodedAndNonNewsCoded(_cosString);
//                            break;
//                    }
//                    _compnyQuery.ConsultantLensQueryType = LensQueryType.Company;
//                    _compnyQuery.ConsultantQueryName = "CO_ALL";
//                    return _compnyQuery; //return "fds=(" + String.Join(" or ", _cosString.ToArray()) + ")";
//                }
//                catch (Exception ex)
//                {
//                    _log.DebugFormat("ERROR WHEN GetCompaniesFreeTextQuery:{0}--{1}", ex.StackTrace, ex.Message);
//                }
//            }
//            return null;
//        }
        
//        private List<string> SeparateNewsCodedAndNonNewsCoded (List<string> fcodes)
//        {
//            List<string> _queries = new List<string>();
//            List<string> _nonNewsCodedNames = new List<string>();
//            List<string> _newsCoded = new List<string>();
//            GetCompanyNewsCodingResponse _response;
//            var codingRequest = new GetCompanyNewsCodingRequest
//            {
//                maxResultsToReturn = fcodes.Count,
//                request = new NewsCodingRequest
//                {
//                    codeScheme = ListCodeScheme.Code
//                }
//            };
//            codingRequest.request.codeCollection.AddRange(fcodes);

//            companyNewsQueryResult result = null;
//            ServiceResponse serviceResponse =
//                Factiva.Gateway.Services.V1_0.ScreeningService.GetCompanyNewsCoding(ControlDataManager.Clone(ControlData), codingRequest);

//            if (serviceResponse != null && serviceResponse.rc == 0)
//            {
//                object o;
//                serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out o);
//                _response = (GetCompanyNewsCodingResponse) o;
//                if (_response!=null && _response.GetCompanyNewsCodingResult!=null )
//                {
//                    result = _response.GetCompanyNewsCodingResult;
//                }
//                if (result != null)
//                {
//                    if (result.companyNewsQueryCollection[0] != null && result.companyNewsQueryCollection.Count > 0)
//                    {

//                        foreach (companyNewsQuery query in result.companyNewsQueryCollection)
//                        {

//                            switch (query.scope)
//                            {
//                                case NewsQueryScope.About:
//                                    _newsCoded.Add(query.djOrgCode);
//                                    break;
//                                case NewsQueryScope.Occur:
//                                    _nonNewsCodedNames.Add(query.MixedValue);
//                                    break;
//                                case NewsQueryScope.None:
//                                    _nonNewsCodedNames.Add(query.MixedValue);
//                                    break;
//                            }

//                        }

//                    }
//                }
//            }
//            var _sbr =  new StringBuilder(210);
//            if (_newsCoded.Count>0)
//            {
//                _sbr.AppendFormat("fds=({0})", String.Join(" or ", _newsCoded.ToArray()));
//            }
//            foreach (string _coName in _nonNewsCodedNames)
//            {
//                _sbr.AppendFormat(@"{1}{0}", "\""+_coName+"\"", _sbr.Length > 0 && !String.IsNullOrEmpty(_coName) ? " or " : string.Empty);
//            }

//            return new List<string>()
//                       {
//                           _sbr.ToString()
//                       };            
//        }
//        private ICollection<ConsultantLensQueries> GetFreeTextFilters(IEnumerable<string> queryIDs)
//        {
//            var searchStrings = new List<ConsultantLensQueries>(3);

//            try
//            {
//                foreach (var queryId in queryIDs)
//                {
//                    var _request = new GetQueriesDetailsListRequest();
//                    _request.Filters.Add(
//                        new NameSearchFilter {Name = new List<string> {queryId}, SearchOperator = FilterSearchOperator.Exact});
//                    _request.QueryTypes = new List<QueryType> {QueryType.PlatformQuery};

//                    var _svcResponse = Invoke<GetQueriesDetailsListResponse>(GenerateControlDataWithCacheInfo("PF_" + queryId), _request, false);

//                    var queryResponse = _svcResponse.ObjectResponse;

//                    if (queryResponse == null || queryResponse.QueryDetailsItems == null)
//                        continue;
//                    foreach (QueryDetailsItem queryDetailsItem in queryResponse.QueryDetailsItems)
//                    {
//                        var aQuery = queryDetailsItem.Query;

//                        foreach (var group in aQuery.Groups)
//                        {
//                            if (group.FilterGroup == null || group.FilterGroup.Filters.Count <= 0)
//                                continue;
//                            foreach (var filter in group.FilterGroup.Filters)
//                            {
//                                if (!(filter is FreeTextFilter))
//                                    continue;
//                                var freeTextFilter = filter as FreeTextFilter;
//                                var _qu = new ConsultantLensQueries
//                                              {
//                                                  ConsultantFreeTextQueryString = freeTextFilter.Texts,
//                                                  ConsultantQueryName =
//                                                      ((PlatformQuery) aQuery).Properties.Name
//                                              };
//                                searchStrings.Add(_qu);
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                _log.FatalFormat("ERROR WHEN GETTING QUERIES:{0}--{1}", ex.StackTrace, ex.Message);
//            }

//            return searchStrings;
//        }

//        private ControlData GenerateControlDataWithCacheInfo(string CacheKey)
//        {
//            return ControlDataManager.GetControlDataForTransactionCache(ControlData, CacheKey, CacheScope.All, DJConsultantConfiguration.CACHE_EXPIRATION_TIME_FOR_QUERIES, 0, CacheExiprationPolicy.Sliding, false);
//        }

//        #region Briefing Book Utility Manager Methods 


//        /// <summary>
//        /// Gets the briefing book opinion and analysis items for the drop down lists.
//        /// </summary>
//        /// <returns>A List of code string; The UI needs to convert these to tokens and use them as values for the drop down as well</returns>
//        public static List<string> GetBriefingBookOpinionAndAnalysisItems ()
//        {
//            IEnumerable <XElement> allNOAAModules = DJConsultantConfiguration.GetInstance.GetBriefingBookNOAAModuleDetails();

//            var  _listOfIds = new List<string>();
//            foreach(var anElement  in allNOAAModules)
//            {
//                if (anElement.Attribute("id") != null && anElement.Attribute("id").Value!=null)
//                    _listOfIds.Add(anElement.Attribute("id").Value);
//            }

//            return _listOfIds;
//        }

//        /// <summary>
//        /// Gets the briefing book's opinion and analysis query by the type of the drop down item
//        /// Append the conmpany code or company name (after checking if news coded or not) and then perform the search transaction
//        /// </summary>
//        /// <param name="TypeOfItem">The type of item.</param>
//        /// <returns>query string</returns>
//        public static string GetBriefingBookOpinionAndAnalysisQueryByType(BBOpinionAnalysis TypeOfItem)
//        {
//            XElement queryNode = DJConsultantConfiguration.GetInstance.GetBriefingBookNOAAQuery(TypeOfItem.ToString());
//            if(queryNode!=null && !String.IsNullOrEmpty(queryNode.Value))
//                return queryNode.Value;

//            return "";
            
//        }
//        /// <summary>
//        /// Gets the briefing book business opportunity query by practice area.
//        /// Append the conmpany code or company name (after checking if news coded or not) and then perform the search transaction
//        /// </summary>
//        /// <param name="practiceArea">The practice area.</param>
//        /// <returns></returns>
//        public static string GetBriefingBookBusinessOpportunityQueryByPracticeArea(ConsultantPracticeArea practiceArea)
//        {
//            XElement queryNode = DJConsultantConfiguration.GetInstance.GetBriefingBookBusinessOpQuery(practiceArea.ToString());
//            if (queryNode != null && !String.IsNullOrEmpty(queryNode.Value))
//                return queryNode.Value;

//            return "";
//        }

//        #endregion Briefing Book Utility Manager Methods
//    }


//    public enum LensQueryType
//    {
//        /// <summary>
//        /// Get all lens. For Companies, it is just focal.
//        /// </summary>
//        AllLens,
//        /// <summary>
//        /// All Focus Companies
//        /// </summary>
//        Company,
//        /// <summary>
//        /// Lens Industries
//        /// </summary>
//        Industry,
//        /// <summary>
//        /// Lens Regions
//        /// </summary>
//        Region,
//        /// <summary>
//        /// Lens PracticeAreas.
//        /// 
//        /// </summary>
//        PracticeArea,
//    }

//    public enum BBOpinionAnalysis
//    {
//        BB_noaa_djColumns,
//        BB_noaa_ec,
//        BB_noaa_gl_pr,
//        BB_noaa_wsj,
//        BB_noaa_dc,
//        BB_noaa_pe_hf,
//        BB_noaa_eq_cap,
//        BB_noaa_ma,
//    }

//    public class ConsultantLensQueries
//    {
//        public LensQueryType ConsultantLensQueryType { get; set; }

//        public List<string> ConsultantFreeTextQueryString { get; set; }

//        public string ConsultantQueryName { get; set; }
//    }

//    public enum ReturnCodesOrQuery
//    {
//        /// <summary>
//        /// Codes as a list will be returned
//        /// </summary>
//        Codes,
//        /// <summary>
//        /// Traditional Query will be created when possible like for Companies, region.
//        /// </summary>
//        FreeTextQuery,

//        /// <summary>
//        /// Based on the News Coded, non news coded, the combination of CompanyCodes or Normalized names will be returned.
//        /// </summary>
//        CompanyCodesAndNames,

//    }
//}