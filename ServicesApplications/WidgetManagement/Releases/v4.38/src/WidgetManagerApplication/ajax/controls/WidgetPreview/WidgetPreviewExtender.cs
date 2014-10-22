using System.ComponentModel;
using System.Web.UI;
using AjaxControlToolkit;
using EMG.Toolkit.Web;
using factiva.nextgen.ui;
using CommonToolkitScripts=AjaxControlToolkit.CommonToolkitScripts;

[assembly : WebResource("EMG.widgets.ui.ajax.controls.WidgetPreview.WidgetPreviewBehavior.js", "text/javascript")]
[assembly : WebResource("EMG.widgets.ui.ajax.controls.WidgetPreview.1x1.gif", "image/gif")]
namespace EMG.widgets.ui.ajax.controls.WidgetPreview
{
    /// <summary>
    /// 
    /// </summary>
    [Designer(typeof (WidgetPreviewDesigner))]
    [RequiredScript(typeof(CommonToolkitScripts),0)]
    [RequiredScript(typeof(AnimationScripts),1)]
    [RequiredScript(typeof(TableSorterExtender),2)]
    [RequiredScript(typeof(TableSorterPagingExtender),3)]
    [RequiredScript(typeof(TableSorterFilterExtender),4)]
    [RequiredScript(typeof(PopupExtender),5)]
    [RequiredScript(typeof(ConfirmButtonExtender),6)]
    [ClientScriptResource("EMG.widgets.ui.ajax.controls.WidgetPreview.WidgetPreviewBehavior", typeof(WidgetPreviewExtender), "WidgetPreviewBehavior.js")]
    [TargetControlType(typeof (Control))]
    public class WidgetPreviewExtender : ExtenderControlBase
    {
        private string first;
        private string last;
        private string next;
        private string prev;
        /// <summary>
        /// Initializes a new instance of the <see cref="WidgetPreviewExtender"/> class.
        /// </summary>
        public WidgetPreviewExtender()
        {
            TableSorterPagingExtender tsp = new TableSorterPagingExtender();

            first = tsp.FirstImageUrl;
            last = tsp.LastImageUrl;
            next = tsp.NextImageUrl;
            prev = tsp.PrevImageUrl;
        }
        /// <summary>
        /// Gets or sets the on widget delete.
        /// </summary>
        /// <value>The on widget delete.</value>
        [DefaultValue("")]
        [Category("Behavior")]
        [ExtenderControlEvent]
        [ClientPropertyName("widgetDelete")]
        public string OnWidgetDelete
        {
            get { return GetPropertyValue("OnWidgetDelete", string.Empty); }
            set { SetPropertyValue("OnWidgetDelete", value); }
        }

        /// <summary>
        /// Gets or sets the on widget edit.
        /// </summary>
        /// <value>The on widget edit.</value>
        [DefaultValue("")]
        [Category("Behavior")]
        [ExtenderControlEvent]
        [ClientPropertyName("widgetEdit")]
        public string OnWidgetEdit
        {
            get { return GetPropertyValue("OnWidgetEdit", string.Empty); }
            set { SetPropertyValue("OnWidgetEdit", value); }
        }

        /// <summary>
        /// Gets or sets the on widget edit.
        /// </summary>
        /// <value>The on widget edit.</value>
        [DefaultValue("")]
        [Category("Behavior")]
        [ExtenderControlEvent]
        [ClientPropertyName("widgetPreview")]
        public string OnWidgetPreview
        {
            get { return GetPropertyValue("OnWidgetPreview", string.Empty); }
            set { SetPropertyValue("OnWidgetPreview", value); }
        }


        /// <summary>
        /// Gets or sets the on widget edit.
        /// </summary>
        /// <value>The on widget edit.</value>
        [DefaultValue("")]
        [Category("Behavior")]
        [ExtenderControlEvent]
        [ClientPropertyName("widgetPublish")]
        public string OnWidgetPublish
        {
            get { return GetPropertyValue("OnWidgetPublish", string.Empty); }
            set { SetPropertyValue("OnWidgetPublish", value); }
        }

        /// <summary>
        /// Gets or sets the on widget edit.
        /// </summary>
        /// <value>The on widget edit.</value>
        [DefaultValue("")]
        [Category("Behavior")]
        [ExtenderControlEvent]
        [ClientPropertyName("previewBack")]
        public string OnPreviewBack
        {
            get { return GetPropertyValue("OnPreviewBack", string.Empty); }
            set { SetPropertyValue("OnPreviewBack", value); }
        }


        /// <summary>
        /// Gets or sets the delete token.
        /// </summary>
        /// <value>The delete token.</value>
        [ExtenderControlProperty]
        [RequiredProperty]
        [ClientPropertyName("deleteToken")]
        public string DeleteToken
        {
            get
            {
                return GetPropertyValue("DeleteToken", string.Empty);
            }
            set
            {
                SetPropertyValue("DeleteToken", value);
            }
        }

        /// <summary>
        /// Gets or sets the delete token.
        /// </summary>
        /// <value>The delete token.</value>
        [ExtenderControlProperty]
        [RequiredProperty]
        [ClientPropertyName("dateToken")]
        public string DateToken
        {
            get
            {
                return GetPropertyValue("DateToken", string.Empty);
            }
            set
            {
                SetPropertyValue("DateToken", value);
            }
        }

        /// <summary>
        /// Gets or sets the delete token.
        /// </summary>
        /// <value>The delete token.</value>
        [ExtenderControlProperty]
        [RequiredProperty]
        [ClientPropertyName("loadingToken")]
        public string LoadingToken
        {
            get
            {
                return GetPropertyValue("LoadingToken", string.Empty);
            }
            set
            {
                SetPropertyValue("LoadingToken", value);
            }
        }

        /// <summary>
        /// Gets or sets the preview token.
        /// </summary>
        /// <value>The preview token.</value>
        [ExtenderControlProperty]
        [RequiredProperty]
        [ClientPropertyName("previewToken")]
        public string PreviewToken
        {
            get
            {
                return GetPropertyValue("PreviewToken", string.Empty);
            }
            set
            {
                SetPropertyValue("PreviewToken", value);
            }
        }

        /// <summary>
        /// Gets or sets the edit token.
        /// </summary>
        /// <value>The edit token.</value>
        [ExtenderControlProperty]
        [RequiredProperty]
        [ClientPropertyName("editToken")]
        public string EditToken
        {
            get
            {
                return GetPropertyValue("EditToken", string.Empty);
            }
            set
            {
                SetPropertyValue("EditToken", value);
            }
        }

        /// <summary>
        /// Gets or sets the edit token.
        /// </summary>
        /// <value>The edit token.</value>
        [ExtenderControlProperty]
        [RequiredProperty]
        [ClientPropertyName("backToken")]
        public string BackToken
        {
            get
            {
                return GetPropertyValue("BackToken", string.Empty);
            }
            set
            {
                SetPropertyValue("BackToken", value);
            }
        }


        /// <summary>
        /// Gets or sets the delete token.
        /// </summary>
        /// <value>The delete token.</value>
        [ExtenderControlProperty]
        [RequiredProperty]
        [ClientPropertyName("publishToken")]
        public string PublishToken
        {
            get
            {
                return GetPropertyValue("PublishToken", string.Empty);
            }
            set
            {
                SetPropertyValue("PublishToken", value);
            }
        }

        /// <summary>
        /// Gets or sets the edit token.
        /// </summary>
        /// <value>The edit token.</value>
        [ExtenderControlProperty]
        [RequiredProperty]
        [ClientPropertyName("noWidgetsToken")]
        public string NoWidgetsToken
        {
            get
            {
                return GetPropertyValue("NoWidgetsToken", string.Empty);
            }
            set
            {
                SetPropertyValue("NoWidgetsToken", value);
            }
        }

        /// <summary>
        /// Gets or sets the name token.
        /// </summary>
        /// <value>The name token.</value>
        [ExtenderControlProperty]
        [RequiredProperty]
        [ClientPropertyName("nameToken")]
        public string NameToken
        {
            get
            {
                return GetPropertyValue("NameToken", string.Empty);
            }
            set
            {
                SetPropertyValue("NameToken", value);
            }
        }

        /// <summary>
        /// Gets or sets the type token.
        /// </summary>
        /// <value>The type token.</value>
        [ExtenderControlProperty]
        [RequiredProperty]
        [ClientPropertyName("typeToken")]
        public string TypeToken
        {
            get
            {
                return GetPropertyValue("TypeToken", string.Empty);
            }
            set
            {
                SetPropertyValue("TypeToken", value);
            }
        }

        /// <summary>
        /// Gets or sets the alert token.
        /// </summary>
        /// <value>The alert token.</value>
        [ExtenderControlProperty]
        [RequiredProperty]
        [ClientPropertyName("alertToken")]
        public string AlertToken
        {
            get
            {
                return GetPropertyValue("AlertToken", string.Empty);
            }
            set
            {
                SetPropertyValue("AlertToken", value);
            }
        }

        /// <summary>
        /// Gets or sets the newsletter token.
        /// </summary>
        /// <value>The newsletter token.</value>
        [ExtenderControlProperty]
        [RequiredProperty]
        [ClientPropertyName("newsletterToken")]
        public string NewsletterToken
        {
            get
            {
                return GetPropertyValue("NewsletterToken", string.Empty);
            }
            set
            {
                SetPropertyValue("NewsletterToken", value);
            }
        }

        /// <summary>
        /// Gets or sets the workspace token.
        /// </summary>
        /// <value>The workspace token.</value>
        [ExtenderControlProperty]
        [RequiredProperty]
        [ClientPropertyName("workspaceToken")]
        public string WorkspaceToken
        {
            get
            {
                return GetPropertyValue("WorkspaceToken", string.Empty);
            }
            set
            {
                SetPropertyValue("WorkspaceToken", value);
            }
        }

        /// <summary>
        /// Gets the first image.
        /// </summary>
        /// <value>The first image.</value>
        [ExtenderControlProperty]
        [ClientPropertyName("firstImage")]
        public string FirstImage
        {
            get
            {
                return first;
            }
        }

        /// <summary>
        /// Gets the prev image.
        /// </summary>
        /// <value>The prev image.</value>
        [ExtenderControlProperty]
        [ClientPropertyName("prevImage")]
        public string PrevImage
        {
            get
            {
                return prev;
            }
        }

        /// <summary>
        /// Gets the next image.
        /// </summary>
        /// <value>The next image.</value>
        [ExtenderControlProperty]
        [ClientPropertyName("nextImage")]
        public string NextImage
        {
            get
            {
                return next;
            }
        }

        /// <summary>
        /// Gets the last image.
        /// </summary>
        /// <value>The last image.</value>
        [ExtenderControlProperty]
        [ClientPropertyName("lastImage")]
        public string LastImage
        {
            get
            {
                return last;
            }
        }

        /// <summary>
        /// Gets or sets the first token.
        /// </summary>
        /// <value>The first token.</value>
        [ExtenderControlProperty]
        [ClientPropertyName("firstToken")]
        public string FirstToken
        {
            get { return ResourceText.GetInstance.GetString("earlyLabel"); }
        }

        /// <summary>
        /// Gets or sets the previous tokens.
        /// </summary>
        /// <value>The previous tokens.</value>
        [ExtenderControlProperty]
        [ClientPropertyName("previousToken")]
        public string PreviousToken
        {
            get { return ResourceText.GetInstance.GetString("previousItem"); }
        }

        /// <summary>
        /// Gets or sets the next token.
        /// </summary>
        /// <value>The next token.</value>
        [ExtenderControlProperty]
        [ClientPropertyName("nextToken")]
        public string NextToken
        {
            get { return ResourceText.GetInstance.GetString("nextItem"); }
        }

        /// <summary>
        /// Gets or sets the last token.
        /// </summary>
        /// <value>The last token.</value>
        [ExtenderControlProperty]
        [ClientPropertyName("lastToken")]
        public string LastToken
        {
            get { return ResourceText.GetInstance.GetString("last"); }
        }

        /// <summary>
        /// Gets the results per page token.
        /// </summary>
        /// <value>The results per page token.</value>
        [ExtenderControlProperty]
        [ClientPropertyName("resultsPerPageToken")]
        public string ResultsPerPageToken
        {
            get { return string.Concat(ResourceText.GetInstance.GetString("resultsPerPage"), ": "); }
        }

    }
}