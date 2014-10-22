using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EMG.widgets.ui.dto.request;
using factiva.nextgen;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;

namespace EMG.widgets.ui.controls.management
{
    /// <summary>
    /// This control is used to filter the widget list shown on widget inventory page.
    /// </summary>
    public class SearchWidgetListControl : BaseControl
    {
        private WidgetManagementDTO m_WidgetManagementDTO; 

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchWidgetListControl"/> class.
        /// </summary>
        public SearchWidgetListControl()
        {
            ID = "SearchWidgetListCtrl";
        }

        /// Gets or sets the widget management DTO.
        /// </summary>
        /// <value>The widget management DTO.</value>
        public WidgetManagementDTO WidgetManagementDTO
        {
            get { return m_WidgetManagementDTO; }
            set { m_WidgetManagementDTO = value; }
        }
        
        private Control GetTypeFilterControl()
        {
            HtmlGenericControl control = new HtmlGenericControl("div");
            control.ID = "typeFilterCtrlContainer";
            control.Attributes.Add("class", "typeFilterCtrlContainer");
            
            HtmlGenericControl filterLabel = new HtmlGenericControl("div");
            filterLabel.Attributes.Add("class", "typeFilterLbl");
            filterLabel.InnerText = string.Format("{0}: " ,ResourceText.GetInstance.GetString("filter"));

            HtmlSelect filter = new HtmlSelect();
            filter.ID = "typeFilterCtrl";
            filter.Attributes.Add("class", "typeFilterCtrl");
            filter.DataTextField = "Text";
            filter.DataValueField = "Value";

            ListItemCollection items = new ListItemCollection();
            items.Add(new ListItem(ResourceText.GetInstance.GetString("all"), ""));

            string alert = ResourceText.GetInstance.GetString("alert");
            string newsletter = ResourceText.GetInstance.GetString("newsletter");
            string workspace = ResourceText.GetInstance.GetString("workspace");

            if (SessionData.Instance().FactivaAccessObject.IsTrackCoreServiceOn)
            {
                if ((m_WidgetManagementDTO.SA_FROM != "GL") || 
                    (m_WidgetManagementDTO.SA_FROM == "GL" && SessionData.Instance().FactivaAccessObject.IsDotComTrackDisplayServiceOn))
                {
                    items.Add(new ListItem(alert, alert));
                }
            }
            
            if (SessionData.Instance().FactivaAccessObject.IsDotComNewsletterDisplayServiceOn)
            {
                items.Add(new ListItem(newsletter, newsletter));
            }
            
            if (SessionData.Instance().FactivaAccessObject.IsDotComWorkspaceDisplayServiceOn)
            {
                items.Add(new ListItem(workspace, workspace));
            }

            filter.DataSource = items;
            filter.DataBind();

            control.Controls.Add(filterLabel);
            control.Controls.Add(filter);

            return control;
        }

        private static Control GetSpacer()
        {
            HtmlGenericControl control = new HtmlGenericControl("div");
            control.ID = "tSpacer";
            control.Attributes.Add("class", "searchTextSpacer");
            control.InnerHtml = "&nbsp;";
            return control;
        }

        private static Control GetSearchTextControl()
        {
            HtmlGenericControl control = new HtmlGenericControl("div");
            control.ID = "searchTextCtrlContainer";
            control.Attributes.Add("class", "searchTextCtrlContainer");

            HtmlGenericControl searchLabel = new HtmlGenericControl("div");
            searchLabel.ID = "searchTextCtrlLbl";
            searchLabel.Attributes.Add("class", "searchTextCtrlLbl");
            searchLabel.InnerHtml = string.Format("{0}: ", ResourceText.GetInstance.GetString("searchWidgets"));

            HtmlInputText search = new HtmlInputText("text");
            search.ID = "searchTextInputCtrl";
            search.Attributes.Add("class", "searchTextInputCtrl");

            /*HtmlInputButton searchButton = new HtmlInputButton();
            searchButton.ID = "searchButtonCtrl";
            searchButton.Attributes.Add("class", "button");
            searchButton.Value = string.Format("{0}", ResourceText.GetInstance.GetString("search"));
            */

            HtmlAnchor searchButton = new HtmlAnchor();
            searchButton.ID = "searchButtonCtrl";
            searchButton.HRef = "javascript:void(0)";
            searchButton.Attributes.Add("class", "button");
            searchButton.InnerHtml = string.Format("<span>{0}</span>", ResourceText.GetInstance.GetString("search"));

            HtmlGenericControl clear = new HtmlGenericControl("div");
            clear.ID = "clearSearchCtrl";
            clear.Attributes.Add("class", "clearSearchCtrl");
            clear.InnerHtml = string.Format("{0}", ResourceText.GetInstance.GetString("clear"));

            control.Controls.Add(searchLabel);
            control.Controls.Add(search);
            control.Controls.Add(searchButton);
            control.Controls.Add(clear);

            return control;
        }

        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void Render(HtmlTextWriter writer)
        {

            Controls.Add(GetTypeFilterControl());
            Controls.Add(GetSearchTextControl());
            Controls.Add(GetSpacer());

            base.Render(writer);
        }

    }
}
