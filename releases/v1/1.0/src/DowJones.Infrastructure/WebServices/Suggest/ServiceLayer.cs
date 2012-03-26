using System;
using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Tools.Ajax;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Managers.Core;

namespace DowJones.Tools.ServiceLayer.WebServices
{
    public abstract class AjaxResponse : IAjaxResponseDelegate
    {
        public string ExceptionMessage { set; get; }

        #region IAjaxResponseDelegate Members

        public long ReturnCode { get; set; }

        public long ElapsedTime { get; set; }

        public string StatusMessage { get; set; }

        #endregion

        protected static void UpdateAjaxDelegate(DowJonesUtilitiesException emgEx, AjaxResponse ajaxResponse)
        {
            ajaxResponse.ReturnCode = emgEx.ReturnCode;
            ajaxResponse.StatusMessage = ResourceTextManager.Instance.GetErrorMessage(ajaxResponse.ReturnCode.ToString());
        }
    }

    public abstract class AjaxRequest
    {
        public string InterfaceLanguage;
        public string ProductPrefix;
    }

    public struct Declarations
    {
        public const string SCHEMA_VERSION = "urn:factiva:fcp:v1_0";
    }

    public enum TaxonomyListTypes
    {
        Industry,
        NewsSubject,
        Region_All,
        Region_CNTRY,
        Region_SP,
        Region_MA,
        Region_SNR,
        Region_SR
    }

    public enum GenericListTypes
    {
        SIC,
        NACE,
        NAICS,
        CITY,
        EXECTITLE,
        AUDITORS,
        FCEBANKS
    }

    public class SuggestCompanyResponse : AjaxResponse
    {
        [XmlElement(Type = typeof (SugestedCompanyList), ElementName = "SuggestCompanyResult", IsNullable = false,
            Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)] [EditorBrowsable(EditorBrowsableState.Never)] [ScriptIgnore] private SugestedCompanyList __suggestCompanyResult;

        [XmlIgnore]
        public SugestedCompanyList Result
        {
            get { return __suggestCompanyResult ?? (__suggestCompanyResult = new SugestedCompanyList()); }
            set { __suggestCompanyResult = value; }
        }
    }

    public class SuggestCompanyRequest : AjaxRequest
    {
        public filter FilterCQS;
        public filter FilterFCE;
        public filter FilterNewsCoded;
        public int MaxResults;
        public string Package;
        public string SearchText;
    }

    public class SuggestExecutiveResponse : AjaxResponse
    {
        [XmlElement(Type = typeof (SuggestedExecutiveList), ElementName = "SuggestExecutiveResult", IsNullable = false,
            Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)] [EditorBrowsable(EditorBrowsableState.Never)] [ScriptIgnore] private SuggestedExecutiveList
            __suggestExecutiveResult;

        [XmlIgnore]
        public SuggestedExecutiveList Result
        {
            get { return __suggestExecutiveResult ?? (__suggestExecutiveResult = new SuggestedExecutiveList()); }
            set { __suggestExecutiveResult = value; }
        }
    }

    public class SuggestExecutiveRequest : AjaxRequest
    {
        public filter FilterInDJ;
        public filter FilterNewsCoded;

        public bool IncludeJobData;
        public int MaxResults;
        public string Package;
        public string SearchText;
    }

    public class SuggestSourceRequest : AjaxRequest
    {
        public bool IncludeGroups;
        public string[] LanguageCodes;
        public int MaxResults;
        public string Package;
        public string SearchText;
        public SourceStatus[] SourceStatuses;
        public SourceType[] SourceTypes;
    }

    public class SuggestAuthorRequest : AjaxRequest
    {
        public string[] CountryIdFilters;
        public filter FilterInDJ;
        public int MaxResults;
        public string[] OutletIdFilters;
        public string Package;
        public string SearchText;
    }

    public class SuggestPublishersRequest : AjaxRequest
    {
        public string[] CountryIdFilters;
        public filter FilterInDJ;
        public int MaxResults;
        public string Package;
        public string SearchText;
    }

    public class SuggestSourceResponse : AjaxResponse
    {
        [XmlElement(Type = typeof (SuggestedSourceList), ElementName = "SuggestSourceResult", IsNullable = false,
            Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)] [EditorBrowsable(EditorBrowsableState.Never)] [ScriptIgnore] private SuggestedSourceList __suggestSourceResult;

        [XmlIgnore]
        public SuggestedSourceList Result
        {
            get { return __suggestSourceResult ?? (__suggestSourceResult = new SuggestedSourceList()); }
            set { __suggestSourceResult = value; }
        }
    }

    public class SuggestAuthorResponse : AjaxResponse
    {
        [XmlElement(Type = typeof (SuggestedAuthorList), ElementName = "SuggestAuthorResult", IsNullable = false,
            Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)] [EditorBrowsable(EditorBrowsableState.Never)] [ScriptIgnore] private SuggestedAuthorList __suggestAuthorResult;

        [XmlIgnore]
        public SuggestedAuthorList Result
        {
            get { return __suggestAuthorResult ?? (__suggestAuthorResult = new SuggestedAuthorList()); }
            set { __suggestAuthorResult = value; }
        }
    }

    public class SuggestPublishersResponse : AjaxResponse
    {
        [XmlElement(Type = typeof (SuggestedOutletList), ElementName = "SuggestOutletResult", IsNullable = false,
            Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)] [EditorBrowsable(EditorBrowsableState.Never)] [ScriptIgnore] private SuggestedOutletList __suggestOutletResult;

        [XmlIgnore]
        public SuggestedOutletList Result
        {
            get { return __suggestOutletResult ?? (__suggestOutletResult = new SuggestedOutletList()); }
            set { __suggestOutletResult = value; }
        }
    }

    public class SuggestTaxonomyRequest : AjaxRequest
    {
        public codeStatus CodeStatus;

        public bool IncludeOtherData;
        public TaxonomyListTypes ListType;
        public int MaxResults;
        public string Package;
        public string SearchText;
        public visibility Visibility;
    }

    public class SuggestTaxonomyResponse : AjaxResponse
    {
        [XmlElement(Type = typeof (SugestedTaxonomyList), ElementName = "SuggestTaxonomyResult", IsNullable = false,
            Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)] [EditorBrowsable(EditorBrowsableState.Never)] [ScriptIgnore] private SugestedTaxonomyList
            __suggestTaxonomyResult;

        [XmlIgnore]
        public SugestedTaxonomyList Result
        {
            get { return __suggestTaxonomyResult ?? (__suggestTaxonomyResult = new SugestedTaxonomyList()); }
            set { __suggestTaxonomyResult = value; }
        }
    }

    public class SuggestGenericRequest : AjaxRequest
    {
        public bool IncludeOtherData;
        public GenericListTypes ListType;
        public int MaxResults;
        public string Package;
        public string SearchText;
    }

    public class SuggestGenericResponse : AjaxResponse
    {
        [XmlElement(Type = typeof (SugestedGenericList), ElementName = "SuggestGenericResult", IsNullable = false,
            Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)] [EditorBrowsable(EditorBrowsableState.Never)] [ScriptIgnore] private SugestedGenericList __suggestGenericResult;

        [XmlIgnore]
        public SugestedGenericList Result
        {
            get { return __suggestGenericResult ?? (__suggestGenericResult = new SugestedGenericList()); }
            set { __suggestGenericResult = value; }
        }
    }


    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "EMG.ServiceLayer.WebServices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class SuggestService : BaseWebService
    {
        private static readonly FactivaSuggests suggestService = new FactivaSuggests();

        [WebMethod]
        [ScriptMethod]
        public SuggestCompanyResponse SuggestCompany(SuggestCompanyRequest companyRequest)
        {
            var response = new SuggestCompanyResponse();
            var request = companyRequest;
            try
            {
                //SessionData sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);

                var cList = suggestService.SuggestCompany(request.SearchText, request.MaxResults,
                                                                          request.FilterNewsCoded, request.FilterFCE,
                                                                          request.FilterCQS, request.Package);
                response.Result = cList;
                return response;
            }

            catch (DowJonesUtilitiesException emgEx)
            {
                UpdateAjaxDelegate(emgEx, response);
                return response;
            }
            catch (Exception exception)
            {
                UpdateAjaxDelegate(new DowJonesUtilitiesException(exception, -1), response);
                return response;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public SuggestExecutiveResponse SuggestExecutive(SuggestExecutiveRequest executiveRequest)
        {
            var response = new SuggestExecutiveResponse();
            var request = executiveRequest;
            try
            {
                //SessionData sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);

                var eList = suggestService.SuggestExecutive(request.SearchText, request.MaxResults,
                                                                               request.FilterNewsCoded,
                                                                               request.FilterInDJ, request.Package,
                                                                               request.IncludeJobData);
                response.Result = eList;
                return response;
            }

            catch (DowJonesUtilitiesException emgEx)
            {
                UpdateAjaxDelegate(emgEx, response);
                return response;
            }
            catch (Exception exception)
            {
                UpdateAjaxDelegate(new DowJonesUtilitiesException(exception, -1), response);
                return response;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public SuggestTaxonomyResponse SuggestTaxonomy(SuggestTaxonomyRequest taxonomyRequest)
        {
            var response = new SuggestTaxonomyResponse();
            var request = taxonomyRequest;
            var rType = regionType.All;
            var t = request.ListType.ToString();
            if (t.Split('_').GetLength(0) == 2)
            {
                try
                {
                    rType = (regionType) Enum.Parse(typeof (regionType), t.Split('_')[1].Trim(), true);
                }
                catch
                {
                    rType = regionType.All;
                }
            }
            try
            {
                SugestedTaxonomyList tList;
                switch (request.ListType)
                {
                    case TaxonomyListTypes.Industry:
                        tList = suggestService.SuggestIndustry(request.SearchText, request.MaxResults,
                                                               request.InterfaceLanguage, request.CodeStatus,
                                                               request.Visibility, request.IncludeOtherData);
                        break;
                    case TaxonomyListTypes.NewsSubject:
                        tList = suggestService.SuggestNewsSubject(request.SearchText, request.MaxResults,
                                                                  request.InterfaceLanguage, request.CodeStatus,
                                                                  request.Visibility, request.IncludeOtherData);
                        break;
                    default:
                        tList = suggestService.SuggestRegion(request.SearchText, request.MaxResults, rType,
                                                             request.InterfaceLanguage, request.CodeStatus,
                                                             request.Visibility, request.IncludeOtherData);
                        break;
                }

                response.Result = tList;
                return response;
            }

            catch (DowJonesUtilitiesException emgEx)
            {
                UpdateAjaxDelegate(emgEx, response);
                return response;
            }
            catch (Exception exception)
            {
                UpdateAjaxDelegate(new DowJonesUtilitiesException(exception, -1), response);
                return response;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public SuggestGenericResponse SuggestGenericList(SuggestGenericRequest genericRequest)
        {
            var response = new SuggestGenericResponse();
            var request = genericRequest;
            try
            {
                string t = request.ListType.ToString();

                var gList = suggestService.SuggestGenericList(request.SearchText, request.MaxResults, t,
                                                                              "",
                                                                              request.InterfaceLanguage,
                                                                              request.IncludeOtherData);
                response.Result = gList;
                return response;
            }

            catch (DowJonesUtilitiesException emgEx)
            {
                UpdateAjaxDelegate(emgEx, response);
                return response;
            }
            catch (Exception exception)
            {
                UpdateAjaxDelegate(new DowJonesUtilitiesException(exception, -1), response);
                return response;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public SuggestSourceResponse SuggestSource(SuggestSourceRequest sourceRequest)
        {
            var response = new SuggestSourceResponse();
            var request = sourceRequest;
            try
            {
                //SessionData sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);

                var sList = suggestService.SuggestSource(request.SearchText, request.MaxResults,
                                                                         request.SourceTypes, request.SourceStatuses,
                                                                         request.LanguageCodes, request.IncludeGroups);
                response.Result = sList;
                return response;
            }
            catch (DowJonesUtilitiesException emgEx)
            {
                UpdateAjaxDelegate(emgEx, response);
                return response;
            }
            catch (Exception exception)
            {
                UpdateAjaxDelegate(new DowJonesUtilitiesException(exception, -1), response);
                return response;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public SuggestAuthorResponse SuggestAuthors(SuggestAuthorRequest authorsRequest)
        {
            var response = new SuggestAuthorResponse();
            var request = authorsRequest;
            try
            {
                //SessionData sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);

                var sList = suggestService.SuggestAuthor(request.SearchText, request.MaxResults,
                                                                         request.OutletIdFilters,
                                                                         request.CountryIdFilters, request.FilterInDJ,
                                                                         request.Package);
                response.Result = sList;
                return response;
            }
            catch (DowJonesUtilitiesException emgEx)
            {
                UpdateAjaxDelegate(emgEx, response);
                return response;
            }
            catch (Exception exception)
            {
                UpdateAjaxDelegate(new DowJonesUtilitiesException(exception, -1), response);
                return response;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public SuggestPublishersResponse SuggestPublishers(SuggestPublishersRequest publisherRequest)
        {
            var response = new SuggestPublishersResponse();
            var request = publisherRequest;
            try
            {
                //SessionData sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);

                var pList = suggestService.SuggestOutlet(request.SearchText, request.MaxResults,
                                                                         request.CountryIdFilters, request.FilterInDJ,
                                                                         request.Package);
                response.Result = pList;
                return response;
            }

            catch (DowJonesUtilitiesException emgEx)
            {
                UpdateAjaxDelegate(emgEx, response);
                return response;
            }
            catch (Exception exception)
            {
                UpdateAjaxDelegate(new DowJonesUtilitiesException(exception, -1), response);
                return response;
            }
        }
    }
}