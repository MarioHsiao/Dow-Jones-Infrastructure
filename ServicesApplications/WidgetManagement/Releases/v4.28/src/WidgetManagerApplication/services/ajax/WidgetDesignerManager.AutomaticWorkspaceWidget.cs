using System;
using System.Reflection;
using System.Web.Script.Services;
using System.Web.Services;
using EMG.widgets.ui.delegates.core.automaticWorkspace;
using EMG.widgets.ui.delegates.input;
using EMG.widgets.ui.delegates.output;
using EMG.widgets.ui.delegates.output.literals;
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
        /// Gets the baseline automatic workspace widget.
        /// </summary>
        /// <param name="assetIds">The asset ids.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overidden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        [GenerateScriptType(typeof(AutomaticWorkspaceWidgetDelegate))]
        [GenerateScriptType(typeof(AutomaticWorkspaceWidgetData))]
        [GenerateScriptType(typeof(AutomaticWorkspaceWidgetDefinition))]
        [GenerateScriptType(typeof(AutomaticWorkspaceWidgetLiterals))]
        public AutomaticWorkspaceWidgetDelegate GetBaselineAutomaticWorkspaceWidget(int[] assetIds, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            // SetHeaderCacheDuration(m_CacheDuration);
            // Initialize Delegate
            AutomaticWorkspaceWidgetDelegate automaticWorkspaceWidgetDelegate = new AutomaticWorkspaceWidgetDelegate();
            try
            {
                // Initialize SessionData.
                new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                // Use CommandPattern to fill delegate with product prefix data.
                automaticWorkspaceWidgetDelegate.SessionData = SessionData.Instance();
                automaticWorkspaceWidgetDelegate.FillPreview(GetUniqueAssetIds(assetIds));
            }
            catch (FactivaBusinessLogicException fbe)
            {
                UpdateAjaxDelegate(fbe, automaticWorkspaceWidgetDelegate);
            }
            catch (Exception exception)
            {
                UpdateAjaxDelegate(new FactivaBusinessLogicException(exception, -1), automaticWorkspaceWidgetDelegate);
            }

            return automaticWorkspaceWidgetDelegate;
        }


        /// <summary>
        /// Creates the alert headline widget.
        /// </summary>
        /// <param name="createAutomaticWorkspaceWidgetDelegate">The create alert headline widget delegate.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overidden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        [GenerateScriptType(typeof(CreateAutomaticWorkspaceWidgetDelegate))]
        [GenerateScriptType(typeof(CreateWidgetResponseDelegate))]
        public CreateWidgetResponseDelegate CreateAutomaticWorkspaceWidget(CreateAutomaticWorkspaceWidgetDelegate createAutomaticWorkspaceWidgetDelegate, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            using (new TransactionLogger(Log, MethodBase.GetCurrentMethod()))
            {
                CreateWidgetResponseDelegate createWidgetResponseDelegate = new CreateWidgetResponseDelegate();
                try
                {
                    SessionData sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                    WidgetManager manager = new WidgetManager(sessionData.SessionBasedControlDataEx, sessionData.InterfaceLanguage);
                    createWidgetResponseDelegate.WidgetId = manager.CreateAutomaticWorkspaceWidget(createAutomaticWorkspaceWidgetDelegate).ToString();
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
        /// Updates the alert headline widget delegate.
        /// </summary>
        /// <param name="updateAutomaticWorkspaceWidgetDelegate">The update automatic workspace widget delegate.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overidden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        [GenerateScriptType(typeof(UpdateAutomaticWorkspaceWidgetDelegate))]
        [GenerateScriptType(typeof(UpdateWidgetResponseDelegate))]
        public UpdateWidgetResponseDelegate UpdateAutomaticWorkspaceWidget(UpdateAutomaticWorkspaceWidgetDelegate updateAutomaticWorkspaceWidgetDelegate, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            using (new TransactionLogger(Log, MethodBase.GetCurrentMethod()))
            {
                UpdateWidgetResponseDelegate updateWidgetResponseDelegate = new UpdateWidgetResponseDelegate();
                try
                {
                    SessionData sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                    WidgetManager manager = new WidgetManager(sessionData.SessionBasedControlDataEx, sessionData.InterfaceLanguage);
                    updateWidgetResponseDelegate.HasBeenUpdated = manager.UpdateAutomaticWorkspaceWidget(updateAutomaticWorkspaceWidgetDelegate);
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
        /// Gets the update alert widget.
        /// </summary>
        /// <param name="widgetId">The widget id.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overidden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public AutomaticWorkspaceWidgetDelegate GetAutomaticWorkspaceWidget(string widgetId, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            using (new TransactionLogger(Log, MethodBase.GetCurrentMethod()))
            {
                //SetHeaderCacheDuration(m_CacheDuration);
                AutomaticWorkspaceWidgetDelegate automaticWorkspaceWidgetDelegate = new AutomaticWorkspaceWidgetDelegate();
                if (string.IsNullOrEmpty(widgetId) || string.IsNullOrEmpty(widgetId.Trim()))
                {
                    throw new ArgumentNullException("widgetId", "parameter is null or empty.");
                }
                try
                {
                    // Initialize SessionData.
                    new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                    automaticWorkspaceWidgetDelegate.SessionData = SessionData.Instance();
                    // Use CommandPattern to fill delegate with product prefix data.
                    automaticWorkspaceWidgetDelegate.FillPreview(widgetId);
                }
                catch (FactivaBusinessLogicException fbe)
                {
                    UpdateAjaxDelegate(fbe, automaticWorkspaceWidgetDelegate);
                }
                catch (Exception exception)
                {
                    UpdateAjaxDelegate(new FactivaBusinessLogicException(exception, -1), automaticWorkspaceWidgetDelegate);
                }
                return automaticWorkspaceWidgetDelegate;
            }
        }

        /// <summary>
        /// Gets the automatic workspace widget script code.
        /// </summary>
        /// <param name="widgetId">The widget id.</param>
        /// <param name="updateAutomaticWorkspaceWidgetDelegate">The update automatic workspace widget delegate.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overidden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        [GenerateScriptType(typeof(GetWidgetCodeResponseDelegate))]
        public GetWidgetCodeResponseDelegate GetAutomaticWorkspaceWidgetScriptCode(string widgetId, UpdateAutomaticWorkspaceWidgetDelegate updateAutomaticWorkspaceWidgetDelegate, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            if (updateAutomaticWorkspaceWidgetDelegate != null)
            {
                UpdateWidgetResponseDelegate responseDelegate = UpdateAutomaticWorkspaceWidget(updateAutomaticWorkspaceWidgetDelegate, accessPointCode, interfaceLanguage, productPrefix);
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
