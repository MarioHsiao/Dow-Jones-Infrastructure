using System;
using System.Reflection;
using System.Web.Script.Services;
using System.Web.Services;
using EMG.widgets.ui.delegates.input;
using EMG.widgets.ui.delegates.output;
using EMG.widgets.ui.delegates.output.services;
using EMG.widgets.ui.delegates.output.syndication;
using Factiva.BusinessLayerLogic.Exceptions;
using Factiva.BusinessLayerLogic.Managers.V2_0;
using Factiva.BusinessLayerLogic.Utility;
using factiva.nextgen;
using Encryption = FactivaEncryption.encryption;

namespace EMG.widgets.services
{
    /// <summary>
    /// 
    /// </summary>
    public partial class WidgetDesignerManager
    {
        /// <summary>
        /// Gets the update alert widget.
        /// </summary>
        /// <param name="widgetId">The widget id.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overidden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public AlertHeadlineWidgetDelegate GetAlertHeadlineWidget(string widgetId, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            using (new TransactionLogger(Log, MethodBase.GetCurrentMethod()))
            {
                //SetHeaderCacheDuration(m_CacheDuration);
                AlertHeadlineWidgetDelegate alertWidgetDelegate = new AlertHeadlineWidgetDelegate();
                if (string.IsNullOrEmpty(widgetId) || string.IsNullOrEmpty(widgetId.Trim()))
                {
                    throw new ArgumentNullException("widgetId", "parameter is null or empty.");
                }
                try
                {
                    // Initialize SessionData.
                    new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                    alertWidgetDelegate.SessionData = SessionData.Instance();
                    // Use CommandPattern to fill delegate with product prefix data.
                    alertWidgetDelegate.FillPreview(widgetId);
                }
                catch (FactivaBusinessLogicException fbe)
                {
                    UpdateAjaxDelegate(fbe, alertWidgetDelegate);
                }
                catch (Exception exception)
                {
                    UpdateAjaxDelegate(new FactivaBusinessLogicException(exception, -1), alertWidgetDelegate);
                }
                return alertWidgetDelegate;
            }
        }

        /// <summary>
        /// Gets the alert headlines.
        /// </summary>
        /// <param name="alertIDs">The alert IDs.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overidden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        [GenerateScriptType(typeof(AlertHeadlineWidgetDelegate))]
        public AlertHeadlineWidgetDelegate GetBaselineAlertHeadlineWidget(int[] alertIDs, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            using (new TransactionLogger(Log, MethodBase.GetCurrentMethod()))
            {
                // SetHeaderCacheDuration(m_CacheDuration);
                // Initialize Delegate
                AlertHeadlineWidgetDelegate alertWidgetDelegate = new AlertHeadlineWidgetDelegate();
                try
                {
                    // Initialize SessionData.
                    new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                    // Use CommandPattern to fill delegate with product prefix data.
                    alertWidgetDelegate.SessionData = SessionData.Instance();
                    alertWidgetDelegate.FillPreview(GetUniqueAssetIds(alertIDs));
                }
                catch (FactivaBusinessLogicException fbe)
                {
                    UpdateAjaxDelegate(fbe, alertWidgetDelegate);
                }
                catch (Exception exception)
                {
                    UpdateAjaxDelegate(new FactivaBusinessLogicException(exception, -1), alertWidgetDelegate);
                }

                return alertWidgetDelegate;
            }
        }

        /// <summary>
        /// Updates the alert headline widget delegate.
        /// </summary>
        /// <param name="updateAlertHeadlineWidgetDelegate">The update alert headline widget delegate.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overidden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        [GenerateScriptType(typeof(UpdateAlertHeadlineWidgetDelegate))]
        [GenerateScriptType(typeof(UpdateWidgetResponseDelegate))]
        public UpdateWidgetResponseDelegate UpdateAlertHeadlineWidget(UpdateAlertHeadlineWidgetDelegate updateAlertHeadlineWidgetDelegate, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            using (new TransactionLogger(Log, MethodBase.GetCurrentMethod()))
            {
                UpdateWidgetResponseDelegate updateWidgetResponseDelegate = new UpdateWidgetResponseDelegate();
                try
                {
                    SessionData sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                    WidgetManager manager = new WidgetManager(sessionData.SessionBasedControlDataEx, sessionData.InterfaceLanguage);
                    updateWidgetResponseDelegate.HasBeenUpdated = manager.UpdateAlertHeadlineWidget(updateAlertHeadlineWidgetDelegate);
                }
                catch (FactivaBusinessLogicException fbe)
                {
                    UpdateAjaxDelegate(fbe, updateWidgetResponseDelegate);
                }
                catch (Exception exception)
                {
                    UpdateAjaxDelegate(new FactivaBusinessLogicException(exception, -1), updateWidgetResponseDelegate);
                }
                return updateWidgetResponseDelegate;
            }
        }

        /// <summary>
        /// Creates the alert headline widget.
        /// </summary>
        /// <param name="createAlertHeadlineWidgetDelegate">The create alert headline widget delegate.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overidden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        [GenerateScriptType(typeof(CreateAlertHeadlineWidgetDelegate))]
        [GenerateScriptType(typeof(CreateWidgetResponseDelegate))]
        public CreateWidgetResponseDelegate CreateAlertHeadlineWidget(CreateAlertHeadlineWidgetDelegate createAlertHeadlineWidgetDelegate, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            using (new TransactionLogger(Log, MethodBase.GetCurrentMethod()))
            {
                CreateWidgetResponseDelegate createWidgetResponseDelegate = new CreateWidgetResponseDelegate();
                try
                {
                    SessionData sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                    WidgetManager manager = new WidgetManager(sessionData.SessionBasedControlDataEx, sessionData.InterfaceLanguage);
                    createWidgetResponseDelegate.WidgetId = manager.CreateAlertHeadlineWidget(createAlertHeadlineWidgetDelegate).ToString();
                }
                catch (FactivaBusinessLogicException fbe)
                {
                    UpdateAjaxDelegate(fbe, createWidgetResponseDelegate);
                }
                catch (Exception exception)
                {
                    UpdateAjaxDelegate(new FactivaBusinessLogicException(exception, -1), createWidgetResponseDelegate);
                }
                return createWidgetResponseDelegate;
            }
        }
        
        /// <summary>
        /// Gets the alert headline widget script code.
        /// </summary>
        /// <param name="widgetId">The widget id.</param>
        /// <param name="updateAlertHeadlineWidgetDelegate">The update alert headline widget delegate.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overidden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        [GenerateScriptType(typeof(GetWidgetCodeResponseDelegate))]
        public GetWidgetCodeResponseDelegate GetAlertHeadlineWidgetScriptCode(string widgetId, UpdateAlertHeadlineWidgetDelegate updateAlertHeadlineWidgetDelegate, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            if (updateAlertHeadlineWidgetDelegate != null)
            {
                UpdateWidgetResponseDelegate responseDelegate = UpdateAlertHeadlineWidget(updateAlertHeadlineWidgetDelegate, accessPointCode, interfaceLanguage, productPrefix);
                if (responseDelegate.ReturnCode != 0)
                {
                    GetWidgetCodeResponseDelegate temp = new GetWidgetCodeResponseDelegate();
                    temp.ReturnCode = responseDelegate.ReturnCode;
                    temp.StatusMessage = responseDelegate.StatusMessage;
                    return temp;
                } 
            }
            using (new TransactionLogger(Log, MethodBase.GetCurrentMethod()))
            {
                return GetWidgetScriptCode(widgetId, accessPointCode, interfaceLanguage, productPrefix, 0);
            }
        }
        
    }
}
