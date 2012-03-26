using System;
using System.Reflection;
using System.Web.Services;
using DowJones.DependencyInjection;
using DowJones.Managers.PAM;
using DowJones.Tools.Ajax;
using DowJones.Utilities.Ajax.Security;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Loggers;
using DowJones.Utilities.Managers.Core;
using log4net;

namespace DowJones.Tools.ServiceLayer.WebServices
{
    public abstract class BaseWebService : WebService
    {
        [Inject("Cannot use constructor injection for web services")]
        protected ILog Logger { get; set; }

        [Inject("Cannot use constructor injection for web services")]
        protected IPageAssetsManagerFactory PageAssetsManagerFactory { get; set; }


        protected BaseWebService()
        {
            ServiceLocator.Current.Inject(this);
        }

        protected static void UpdateAjaxDelegate(DowJonesUtilitiesException emgEx, IAjaxResponseDelegate ajaxResponseDelegate)
        {
            UpdateAjaxDelegate(emgEx, ajaxResponseDelegate, null);
        }

        protected static void UpdateAjaxDelegate(DowJonesUtilitiesException emgEx, AjaxResponse ajaxResponse)
        {
            UpdateAjaxDelegate(emgEx, ajaxResponse, null);
        }

        protected static void UpdateAjaxDelegate(DowJonesUtilitiesException emgEx, IAjaxResponseDelegate ajaxResponseDelegate, TransactionLogger transactionLogger)
        {
            ajaxResponseDelegate.ReturnCode = emgEx.ReturnCode;
            ajaxResponseDelegate.StatusMessage = ResourceTextManager.Instance.GetErrorMessage(ajaxResponseDelegate.ReturnCode.ToString());
            if (transactionLogger != null)
            {
                ajaxResponseDelegate.ElapsedTime = transactionLogger.ElapsedTimeSinceInvocation;
            }
        }

        protected static void UpdateAjaxDelegate(DowJonesUtilitiesException emgEx, AjaxResponse ajaxResponse, TransactionLogger transactionLogger)
        {
            ajaxResponse.ReturnCode = emgEx.ReturnCode;
            ajaxResponse.StatusMessage = ResourceTextManager.Instance.GetErrorMessage(ajaxResponse.ReturnCode.ToString());
            if (transactionLogger != null)
            {
                ajaxResponse.ElapsedTime = transactionLogger.ElapsedTimeSinceInvocation;
            }
        }

        protected virtual void ProcessRequest(MethodBase method, IAjaxResponseDelegate responseDelegate, ProxyCredentials credentials, Action<IAjaxResponseDelegate, IPageAssetsManager> processingFunction)
        {
            using (new TransactionLogger(Logger, method))
            {
                try
                {
                    var manager = PageAssetsManagerFactory.CreateManager();
                    processingFunction.Invoke(responseDelegate, manager);
                }
                catch (DowJonesUtilitiesException rEx)
                {
                    UpdateAjaxDelegate(rEx, responseDelegate);
                }
                catch (Exception exception)
                {
                    UpdateAjaxDelegate(new DowJonesUtilitiesException(exception, -1), responseDelegate);
                }
            }
        }
    }
}