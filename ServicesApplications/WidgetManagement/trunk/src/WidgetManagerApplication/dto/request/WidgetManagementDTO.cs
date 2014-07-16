using System;
using System.Web;
using EMG.widgets.ui.dto.lob;
using EMG.widgets.ui.Properties;
using Factiva.BusinessLayerLogic.Attributes;
using factiva.nextgen.ui;

namespace EMG.widgets.ui.dto.request
{
    /// <summary>
    /// 
    /// </summary>
    public class WidgetManagementDTO : AbstractWidgetRequestDTO
    {
        private const string OLD_PARAMETER_NAME_ASSETIDS = "fldrids";

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("at")] public WidgetManagementAction action = WidgetManagementAction.List;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("sat")] public WidgetManagementSecondaryAction secondaryAction = WidgetManagementSecondaryAction.None;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("assetIds")] public string[] assetIds;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("refProd")] public WidgetRefererProduct refererProduct = WidgetRefererProduct.UnSpecified;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("returnurl")] public string doneUrl;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("SA_FROM")] public string SA_FROM = "GL";

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("wid")] public string widgetId;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("wt")] public WidgetType widgetType = WidgetType.AlertHeadlineWidget;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("sb")] public WidgetSortBy widgetSortBy = WidgetSortBy.Date;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("page")] 
        public int page;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("pagesize")] 
        public int pageSize = 20;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("sortOrder")]
        public string sortOrder = "[[2,1]]";

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("im")] public int isMct = 0;

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public override void Load()
        {
            // Note: This code snippet has been implmented for backward compatability - dacostad 11.12.2008

            #region Backward Compatability Code Snippet

            if ((assetIds == null || assetIds.Length == 0) && HttpContext.Current != null)
            {
                string folderIds = HttpContext.Current.Request[OLD_PARAMETER_NAME_ASSETIDS];
                if (!string.IsNullOrEmpty(folderIds) &&
                    !string.IsNullOrEmpty(folderIds.Trim()))
                {
                    assetIds = folderIds.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
                }
            }

            #endregion

            // Validate the action
            if (!string.IsNullOrEmpty(widgetId) &&
                !string.IsNullOrEmpty(widgetId.Trim()) &&
                action == WidgetManagementAction.Create)
            {
                action = WidgetManagementAction.Update;
            }

            switch (SA_FROM.ToUpper())
            {
                case "GL":
                    SetWidgetRefererProduct(WidgetRefererProduct.FactivaDotCom);
                    if (string.IsNullOrEmpty(doneUrl))
                    {
                        doneUrl = Settings.Default.GL_BaseUrl;
                    }
                    break;
                case "FC":
                    SetWidgetRefererProduct(WidgetRefererProduct.FactivaCompaniesAndExecutives);
                    if (string.IsNullOrEmpty(doneUrl))
                    {
                        doneUrl = Settings.Default.FC_BaseUrl;
                    }
                    break;
                case "IF":
                    SetWidgetRefererProduct(WidgetRefererProduct.IWorksPremium);
                    if (string.IsNullOrEmpty(doneUrl))
                    {
                        doneUrl = Settings.Default.IF_BaseUrl;
                    }
                    break;
                case "IN":
                    SetWidgetRefererProduct(WidgetRefererProduct.Insight);
                    if (string.IsNullOrEmpty(doneUrl))
                    {
                        doneUrl = Settings.Default.IN_BaseUrl;
                    }
                    break;
            }
        }

        private void SetWidgetRefererProduct(WidgetRefererProduct value)
        {
            if (refererProduct == WidgetRefererProduct.UnSpecified)
            {
                refererProduct = value;
            }
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid()
        {
            // Currently the product only supports the following criteria.
            switch (action)
            {
                case WidgetManagementAction.Create:
                    switch (widgetType)
                    {
                        // Can have more that one asset [Alert Folder] in AlertHeadlineWidget
                        case WidgetType.AlertHeadlineWidget:
                            if (assetIds != null &&
                                assetIds.Length > 0 &&
                                assetIds.Length >= 1)
                            {
                                return true;
                            }
                            break;
                        // one asset [Workspace] in AlertHeadlineWidget
                        case WidgetType.AutomaticWorkspaceWidget:
                            if (assetIds != null &&
                                assetIds.Length > 0 &&
                                assetIds.Length == 1)
                            {
                                return true;
                            }
                            break;
                        // one asset [Newsletter] in AlertHeadlineWidget
                        case WidgetType.ManualNewsletterWorkspaceWidget:
                            if (assetIds != null &&
                                assetIds.Length > 0 && 
                                assetIds.Length == 1)
                            {
                                return true;
                            }
                            break;
                    }
                    return false;
                case WidgetManagementAction.Update:
                    switch (widgetType)
                    {
                        case WidgetType.AlertHeadlineWidget:
                            if (!string.IsNullOrEmpty(widgetId) && 
                                !string.IsNullOrEmpty(widgetId.Trim()))
                            {
                                return true;
                            }
                            break;
                        case WidgetType.AutomaticWorkspaceWidget:
                            if (!string.IsNullOrEmpty(widgetId) && 
                                !string.IsNullOrEmpty(widgetId.Trim()))
                            {
                                return true;
                            }
                            break;
                        case WidgetType.ManualNewsletterWorkspaceWidget:
                            if (!string.IsNullOrEmpty(widgetId) && 
                                !string.IsNullOrEmpty(widgetId.Trim()))
                            {
                                return true;
                            }
                            break;
                    }
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Gets the redirection URL.
        /// </summary>
        /// <returns></returns>
        public string GetRedirectionUrl()
        {
            UrlBuilder ub = new UrlBuilder();
            ub.BaseUrl = "~/widgetManagement/default.aspx";
            ub.Append(UrlBuilder.GetParameterName(typeof (WidgetManagementDTO), "widgetId"), widgetId);
            ub.Append(UrlBuilder.GetParameterName(typeof (WidgetManagementDTO), "action"), (int) action);
            ub.Append(UrlBuilder.GetParameterName(typeof (WidgetManagementDTO), "assetIds"), assetIds);
            ub.Append(UrlBuilder.GetParameterName(typeof (WidgetManagementDTO), "refererProduct"), (int) refererProduct);
            ub.Append(UrlBuilder.GetParameterName(typeof (WidgetManagementDTO), "isMct"), (int) isMct);
            ub.Append(UrlBuilder.GetParameterName(typeof (WidgetManagementDTO), "widgetType"), (int) widgetType);
            ub.Append(UrlBuilder.GetParameterName(typeof (WidgetManagementDTO), "widgetSortBy"), (int) widgetSortBy);
            ub.Append(UrlBuilder.GetParameterName(typeof (WidgetManagementDTO), "SA_FROM"), SA_FROM);
            ub.Append(UrlBuilder.GetParameterName(typeof (WidgetManagementDTO), "doneUrl"), doneUrl);
            if (secondaryAction != WidgetManagementSecondaryAction.None)
            {
                ub.Append(UrlBuilder.GetParameterName(typeof(WidgetManagementDTO), "secondaryAction"), (int) secondaryAction);
            }


            return ub.ToString();
        }

        /// <summary>
        /// Gets the transfer URL.
        /// </summary>
        /// <returns></returns>
        public string GetTransferUrl()
        {
            UrlBuilder ub = new UrlBuilder();
            ub.BaseUrl = "~/widgetManagement/default.aspx";
            return ub.ToString();
        }
    }
}