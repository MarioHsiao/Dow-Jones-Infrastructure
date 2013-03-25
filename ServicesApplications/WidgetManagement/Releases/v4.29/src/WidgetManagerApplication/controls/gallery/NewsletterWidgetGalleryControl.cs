using System.Web.UI;
using System.Web.UI.HtmlControls;
using EMG.widgets.ui.controls.navigation;
using EMG.widgets.ui.Properties;
using factiva.nextgen;
using factiva.nextgen.ui;

namespace EMG.widgets.ui.controls.gallery
{
    ///<summary>
    /// Newsletter Widget Gallery Control
    ///</summary>
    public class NewsletterWidgetGalleryControl : AbstractGalleryControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsletterWidgetGalleryControl"/> class.
        /// </summary>
        public NewsletterWidgetGalleryControl()
        {
            ID = "nWidgetGalleryControl";
            Attributes.Add("class", "galleryItem");
        }

        /// <summary>
        /// Gets the title control.
        /// </summary>
        /// <returns></returns>
        private static Control GetTitleControl()
        {
            HtmlGenericControl designTitle = new HtmlGenericControl("div");
            designTitle.Attributes.Add("class", "galleryItemTitle");
            designTitle.InnerText = ResourceText.GetInstance.GetString("newsletterWidget");
            return designTitle;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <returns></returns>
        private static Control GetDescription()
        {
            HtmlGenericControl designTitle = new HtmlGenericControl("div");
            designTitle.Attributes.Add("class", "galleryItemDescription");
            designTitle.InnerText = ResourceText.GetInstance.GetString("newsletterWidgetDescription");
            return designTitle;
        }

        /// <summary>
        /// Gets the button.
        /// </summary>
        /// <returns></returns>
        private Control GetButton()
        {
            string hRef = "javascript:void(0)";
            if (m_WidgetManagementDTO.IsValid())
            {
                if (!string.IsNullOrEmpty(m_WidgetManagementDTO.doneUrl) && !string.IsNullOrEmpty(m_WidgetManagementDTO.doneUrl.Trim()))
                {
                    hRef = string.Format(Settings.Default.BaseUrl_ManualNewsletterWorkspaceWidget, SessionData.Instance().ProductPrefix, m_WidgetManagementDTO.doneUrl);
                }
            }

            WidgetLinkButtonControl control = new WidgetLinkButtonControl();
            control.Attributes.Add("class", "galleryItemButton");
            control.HRef = hRef;
            control.AltTag = string.Format("{0}", ResourceText.GetInstance.GetString("publishNewsletter"));
            return control;
        }

        /// <summary>
        /// Gets the sample widget image for the gallery.
        /// </summary>
        /// <returns></returns>
        private static Control GetImage()
        {
            HtmlGenericControl imageContainer = new HtmlGenericControl("div");
            imageContainer.InnerHtml = "&nbsp;";
            imageContainer.Attributes.Add("class", "galleryItemImageContainer");

            HtmlImage imageControl = new HtmlImage();
            imageControl.Attributes.Add("class", "galleryItemImage");
            imageControl.Src = "../img/widgets/newsletter_widget_samp.png";

            imageContainer.Controls.Add(imageControl);

            return imageContainer;
        }
        
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            Controls.Add(GetTitleControl());
            Controls.Add(GetDescription());
            Controls.Add(GetButton());
            Controls.Add(GetImage());
            base.OnPreRender(e);
        }

    }
}
