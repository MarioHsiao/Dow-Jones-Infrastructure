// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderAsyncOperation.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the RenderAsyncOperation type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
//// using EMG.widgets.Managers;
using EMG.widgets.ui.delegates.interfaces;
using EMG.widgets.ui.delegates.output.syndication;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
//// using EMG.widgets.ui.encryption;
using EMG.widgets.ui.utility;
//// using Factiva.BusinessLayerLogic.Managers.V2_0;
using factiva.nextgen;
using factiva.nextgen.ui.formstate;
using EMGUtility = EMG.widgets.ui.utility.Utility;

namespace EMG.widgets.ui.syndication
{
    /// <summary>
    /// </summary>
    public class RenderAsyncOperation : IAsyncResult
    {
        private readonly AsyncCallback callback;
        private readonly HttpContext context;
        private readonly object state;
        private bool completed;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderAsyncOperation"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="context">The context.</param>
        /// <param name="state">The state.</param>
        public RenderAsyncOperation(AsyncCallback callback, HttpContext context, object state)
        {
            this.callback = callback;
            this.context = context;
            this.state = state;
            completed = false;
        }

        #region IAsyncResult Members

        bool IAsyncResult.IsCompleted
        {
            get { return completed; }
        }

        WaitHandle IAsyncResult.AsyncWaitHandle
        {
            get { return null; }
        }

        object IAsyncResult.AsyncState
        {
            get { return state; }
        }

        bool IAsyncResult.CompletedSynchronously
        {
            get { return false; }
        }

        #endregion

        /// <summary>
        /// Starts the async work.
        /// </summary>
        public void StartAsyncWork()
        {
            ThreadPool.QueueUserWorkItem(StartAsyncTask, null);
        }

        /// <summary>
        /// Gets the delegate.
        /// </summary>
        /// <param name="renderWidgetDTO">The render widget DTO.</param>
        /// <returns>The IWidgetSyndicationDelegate object.</returns>
        public IWidgetSyndicationDelegate GetDelegate(RenderWidgetDTO renderWidgetDTO)
        {
            // Instantiate the correct delegate
            switch (renderWidgetDTO.type)
            {
                case WidgetType.AlertHeadlineWidget:
                    var alertHeadlineWidgetDelegate = new AlertHeadlineWidgetDelegate();
                    alertHeadlineWidgetDelegate.Fill(renderWidgetDTO.token, renderWidgetDTO.integrationTarget);
                    return alertHeadlineWidgetDelegate;
                
                case WidgetType.AutomaticWorkspaceWidget:
                    var automaticWorkspaceWidgetDelegate = new AutomaticWorkspaceWidgetDelegate();
                    automaticWorkspaceWidgetDelegate.Fill(renderWidgetDTO.token, renderWidgetDTO.integrationTarget);
                    return automaticWorkspaceWidgetDelegate;

                case WidgetType.ManualNewsletterWorkspaceWidget:
                    var manualNewsletterWorkspaceWidgetDelegate = new ManualNewsletterWorkspaceWidgetDelegate();
                    manualNewsletterWorkspaceWidgetDelegate.Fill(renderWidgetDTO.token, renderWidgetDTO.integrationTarget);
                    return manualNewsletterWorkspaceWidgetDelegate;
            }

            return null;
        }

        /// <summary>
        /// Starts the async task.
        /// </summary>
        /// <param name="workItemState">State of the work item.</param>
        private void StartAsyncTask(object workItemState)
        {
            // initialize the context of the thread
            CallContext.HostContext = context;
            var formState = new FormState(string.Empty);
            var renderWidgetDTO = (RenderWidgetDTO)formState.Accept(typeof(RenderWidgetDTO), false);
            new SessionData("b", "en", 0, false);

            var response = HttpContext.Current.Response;
            response.Buffer = true;

            // initialize essential variables
            //// var tokenProperties = new WidgetTokenProperties(renderWidgetDTO.token);
            //// var userControlData = ControlDataManager.AddProxyCredentialsToControlData(ControlDataManagerEx.GetRssFeed1LightWeightUser(), tokenProperties.UserId, tokenProperties.NameSpace);
            //// var widgetManager = new WidgetManager(userControlData, SessionData.Instance().InterfaceLanguage);

            var widgetSyndicationDelegate = GetDelegate(renderWidgetDTO);
            if (widgetSyndicationDelegate == null)
            {
                return;
            }

            switch (renderWidgetDTO.responseFormat)
            {
                case ResponseFormat.JSON:
                    response.ContentType = EMGUtility.GetMimeType(MimeType.JSON);
                    response.Write(widgetSyndicationDelegate.ToJSON());
                    response.End();
                    break;
                case ResponseFormat.JSONP:
                    response.ContentType = EMGUtility.GetMimeType(MimeType.JSON);

                    // Short-Circuit the delivery of JSONP Data for widgets and check cache
                    var json = widgetSyndicationDelegate.ToJSON();
                    
                    if (string.IsNullOrEmpty(renderWidgetDTO.callBackParam) || string.IsNullOrEmpty(renderWidgetDTO.callBackParam.Trim()))
                    {
                        response.Write(AbstractWidgetDelegate.ConvertToJSONP(json, renderWidgetDTO.callBackFunction));
                    }
                    else
                    {
                        response.Write(AbstractWidgetDelegate.ConvertToJSONP(json, renderWidgetDTO.callBackFunction, renderWidgetDTO.callBackParam));
                    }

                    response.End();
                    break;
                case ResponseFormat.XML:
                    response.ContentType = EMGUtility.GetMimeType(MimeType.XML);
                    response.Write(widgetSyndicationDelegate.ToXML());
                    response.End();
                    break;
                case ResponseFormat.ATOM:
                    response.ContentType = EMGUtility.GetMimeType(MimeType.ATOM);
                    response.Write(widgetSyndicationDelegate.ToATOM());
                    response.End();
                    break;
                case ResponseFormat.RSS:
                    response.ContentType = EMGUtility.GetMimeType(MimeType.RSS);
                    response.Write(widgetSyndicationDelegate.ToRSS());
                    response.End();
                    break;
                case ResponseFormat.PODCAST_RSS:
                    var podcastDelegate = widgetSyndicationDelegate as IWidgetPodcastDelegate;
                    if (podcastDelegate != null)
                    {
                        response.ContentType = EMGUtility.GetMimeType(MimeType.XML);
                        response.Write(podcastDelegate.ToPodcastRSS());
                        response.End();
                    }

                    break;
            }

            completed = true;
            callback(this);
            CallContext.HostContext = null;
        }
    }
}