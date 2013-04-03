using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;
using factiva.nextgen.ui;

namespace EMG.widgets.ui.delegates.core.alertHeadline
{
    /// <summary>
    /// Alert Headline Widget Definition class.
    /// </summary>
    public class AlertHeadlineWidgetDefinition : HeadlineWidgetDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlertHeadlineWidgetDefinition"/> class.
        /// </summary>
        public AlertHeadlineWidgetDefinition()
        {
        }

        /// <summary>
        /// Alert Ids
        /// </summary>
        public int[] alertIds;

        /// <summary>
        /// Discovery Tab definition
        /// </summary>
        public WidgetDiscoveryTab[] DiscoveryTabs = InitializeDiscoveryTabs();
        
        /// <summary>
        /// Bar color for Corda charts.
        /// </summary>
        public string ChartBarColor = "#DCDADA";
        
        /// <summary>
        /// Gets the display type value.
        /// </summary>
        /// <value>The display type value.</value>
        public string DisplayTypeValue
        {
            get { return DisplayType.ToString(); }
        }

        /// <summary>
        /// Initializes the discovery tabs.
        /// </summary>
        /// <returns></returns>
        public static WidgetDiscoveryTab[] InitializeDiscoveryTabs()
        {
            WidgetDiscoveryTabCollection tabs = new WidgetDiscoveryTabCollection();
            
            WidgetDiscoveryTab headlines = new WidgetDiscoveryTab();
            headlines.Active = true;
            headlines.DisplayCheckbox = false;
            headlines.Id = "headlines";
            headlines.Text = ResourceText.GetInstance.GetString("headlines");

            WidgetDiscoveryTab companies = new WidgetDiscoveryTab();
            companies.Active = true;
            companies.DisplayCheckbox = true;
            companies.Id = "companies";
            companies.Text = ResourceText.GetInstance.GetString("companies");

            WidgetDiscoveryTab executives = new WidgetDiscoveryTab();
            executives.Active = true;
            executives.DisplayCheckbox = true;
            executives.Id = "executives";
            executives.Text = ResourceText.GetInstance.GetString("executives");

            WidgetDiscoveryTab industries = new WidgetDiscoveryTab();
            industries.Active = true;
            industries.DisplayCheckbox = true;
            industries.Id = "industries";
            industries.Text = ResourceText.GetInstance.GetString("industries");

            WidgetDiscoveryTab regions = new WidgetDiscoveryTab();
            regions.Active = true;
            regions.DisplayCheckbox = true;
            regions.Id = "regions";
            regions.Text = ResourceText.GetInstance.GetString("regions");

            WidgetDiscoveryTab subjects = new WidgetDiscoveryTab();
            subjects.Active = true;
            subjects.DisplayCheckbox = true;
            subjects.Id = "subjects";
            subjects.Text = ResourceText.GetInstance.GetString("newsSubjects");

            tabs.Insert(0, headlines);
            tabs.Insert(1, companies);
            tabs.Insert(2, executives);
            tabs.Insert(3, industries);
            tabs.Insert(4, regions);
            tabs.Insert(5, subjects);

            return tabs.ToArray();
        }
    }
}