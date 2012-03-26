using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.Script.Services;
using System.Web.Services;
using DowJones.Tools.Ajax.Suggest;
using DowJones.Tools.ServiceLayer.WebServices;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Loggers;
using log4net;

// THIS FILE IS NOT USED.

// Check ServiceLayer.cs.

namespace DowJones.Tools.WebServices.Suggest
{
    [WebService(Namespace = "DowJones.Utilities.WebServices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class SuggestService : BaseWebService
    {
        private static readonly ILog m_Log = LogManager.GetLogger(typeof(SuggestService));
        private static readonly FactivaSuggests suggestService = new FactivaSuggests();

        [WebMethod]
        [ScriptMethod]
        public SuggestCompanyResponseDelegate SuggestCompany(SuggestCompanyRequestDelegate request,
                                                             string interfaceLanguage, string productPrefix)
        {
            var suggetsCompanyResponseDelegate = new SuggestCompanyResponseDelegate();
            using (new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                try
                {
                    //SessionData sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);

                    var cList = suggestService.SuggestCompany(request.SearchText, request.MaxResults,
                                                                              request.FilterNewsCoded, request.FilterFCE,
                                                                              request.FilterCQS, request.Package);
                    suggetsCompanyResponseDelegate.Result = cList;
                    return suggetsCompanyResponseDelegate;
                }
                catch (DowJonesUtilitiesException emgEx)
                {
                    UpdateAjaxDelegate(emgEx, suggetsCompanyResponseDelegate);
                    return suggetsCompanyResponseDelegate;
                }
                catch (Exception exception)
                {
                    UpdateAjaxDelegate(new DowJonesUtilitiesException(exception, -1), suggetsCompanyResponseDelegate);
                    return suggetsCompanyResponseDelegate;
                }
            }
        }
    }
}