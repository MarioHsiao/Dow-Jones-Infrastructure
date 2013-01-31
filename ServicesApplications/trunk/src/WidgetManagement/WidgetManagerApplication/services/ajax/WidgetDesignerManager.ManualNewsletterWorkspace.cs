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

namespace EMG.widgets.services
{
    public partial class WidgetDesignerManager
    {

        /// <summary>
        /// Gets the baseline automatic workspace widget.
        /// </summary>
        /// <param name="assetIds">The asset ids.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overidden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ManualNewsletterWorkspaceWidgetDelegate GetBaselineManualNewsletterWorkspaceWidget(int[] assetIds, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            // SetHeaderCacheDuration(m_CacheDuration);
            // Initialize Delegate
            ManualNewsletterWorkspaceWidgetDelegate manualNewsletterWorkspaceWidgetDelegate = new ManualNewsletterWorkspaceWidgetDelegate();
            try
            {
                // Initialize SessionData.
                new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                // Use CommandPattern to fill delegate with product prefix data.
                manualNewsletterWorkspaceWidgetDelegate.SessionData = SessionData.Instance();
                manualNewsletterWorkspaceWidgetDelegate.FillPreview(GetUniqueAssetIds(assetIds));
            }
            catch (FactivaBusinessLogicException fbe)
            {
                UpdateAjaxDelegate(fbe, manualNewsletterWorkspaceWidgetDelegate);
            }
            catch (Exception exception)
            {
                UpdateAjaxDelegate(new FactivaBusinessLogicException(exception, -1), manualNewsletterWorkspaceWidgetDelegate);
            }

            return manualNewsletterWorkspaceWidgetDelegate;
        }

        /// <summary>
        /// Creates the manual newsletter workspace widget.
        /// </summary>
        /// <param name="createManaualNewsletterWorkspaceWidgetDelegate">The create alert headline widget delegate.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overidden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        [GenerateScriptType(typeof(CreateManaualNewsletterWorkspaceWidgetDelegate))]
        [GenerateScriptType(typeof(CreateWidgetResponseDelegate))]
        public CreateWidgetResponseDelegate CreateManualNewsletterWorkspaceWidget(CreateManaualNewsletterWorkspaceWidgetDelegate createManaualNewsletterWorkspaceWidgetDelegate, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            using (new TransactionLogger(Log, MethodBase.GetCurrentMethod()))
            {
                CreateWidgetResponseDelegate createWidgetResponseDelegate = new CreateWidgetResponseDelegate();
                try
                {
                    SessionData sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                    WidgetManager manager = new WidgetManager(sessionData.SessionBasedControlDataEx, sessionData.InterfaceLanguage);
                    createWidgetResponseDelegate.WidgetId = manager.CreateManualNewsletterWorkspaceWidget(createManaualNewsletterWorkspaceWidgetDelegate).ToString();
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
        /// Gets the update alert widget.
        /// </summary>
        /// <param name="widgetId">The widget id.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overidden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ManualNewsletterWorkspaceWidgetDelegate GetManualNewsletterWorkspaceWidget(string widgetId, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            using (new TransactionLogger(Log, MethodBase.GetCurrentMethod()))
            {
                //SetHeaderCacheDuration(m_CacheDuration);
                var manualNewsletterWorkspaceWidgetDelegate = new ManualNewsletterWorkspaceWidgetDelegate();
                if (string.IsNullOrEmpty(widgetId) || string.IsNullOrEmpty(widgetId.Trim()))
                {
                    throw new ArgumentNullException("widgetId", "parameter is null or empty.");
                }
                try
                {
                    // Initialize SessionData.
                    new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                    manualNewsletterWorkspaceWidgetDelegate.SessionData = SessionData.Instance();
                    // Use CommandPattern to fill delegate with product prefix data.
                    manualNewsletterWorkspaceWidgetDelegate.FillPreview(widgetId);
                }
                catch (FactivaBusinessLogicException fbe)
                {
                    UpdateAjaxDelegate(fbe, manualNewsletterWorkspaceWidgetDelegate);
                }
                catch (Exception exception)
                {
                    UpdateAjaxDelegate(new FactivaBusinessLogicException(exception, -1), manualNewsletterWorkspaceWidgetDelegate);
                }
                return manualNewsletterWorkspaceWidgetDelegate;
            }
        }


        /// <summary>
        /// Updates the alert headline widget delegate.
        /// </summary>
        /// <param name="updateManualNewsletterWorkspaceWidgetDelegate">The update automatic workspace widget delegate.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overidden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        [GenerateScriptType(typeof(UpdateManualNewsletterWorkspaceWidgetDelegate))]
        [GenerateScriptType(typeof(UpdateWidgetResponseDelegate))]
        public UpdateWidgetResponseDelegate UpdateManualNewsletterWorkspaceWidget(UpdateManualNewsletterWorkspaceWidgetDelegate updateManualNewsletterWorkspaceWidgetDelegate, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            using (new TransactionLogger(Log, MethodBase.GetCurrentMethod()))
            {
                UpdateWidgetResponseDelegate updateWidgetResponseDelegate = new UpdateWidgetResponseDelegate();
                try
                {
                    SessionData sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                    WidgetManager manager = new WidgetManager(sessionData.SessionBasedControlDataEx, sessionData.InterfaceLanguage);
                    updateWidgetResponseDelegate.HasBeenUpdated = manager.UpdateManualNewsletterWorkspaceWidget(updateManualNewsletterWorkspaceWidgetDelegate);
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
        /// Gets the manual newsletter workspace widget script code.
        /// </summary>
        /// <param name="widgetId">The widget id.</param>
        /// <param name="updateManualNewsletterWorkspaceWidgetDelegate">The update manual newsletter workspace widget delegate.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overidden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        [GenerateScriptType(typeof(GetWidgetCodeResponseDelegate))]
        public GetWidgetCodeResponseDelegate GetManualNewsletterWorkspaceWidgetScriptCode(string widgetId, UpdateManualNewsletterWorkspaceWidgetDelegate updateManualNewsletterWorkspaceWidgetDelegate, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            if (updateManualNewsletterWorkspaceWidgetDelegate != null)
            {
                UpdateWidgetResponseDelegate responseDelegate = UpdateManualNewsletterWorkspaceWidget(updateManualNewsletterWorkspaceWidgetDelegate, accessPointCode, interfaceLanguage, productPrefix);
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
                return GetWidgetScriptCode(widgetId, accessPointCode, interfaceLanguage, productPrefix);
            }
        }
    }
}
