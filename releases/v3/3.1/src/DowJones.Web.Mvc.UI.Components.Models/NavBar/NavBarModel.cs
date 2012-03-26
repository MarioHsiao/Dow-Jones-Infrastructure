using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Web.Mvc.UI.Components.NavBar
{

    public class NavBarModel
    {

        [ClientData("tabs")]
        public IEnumerable<NavItem> Tabs { get; set; }
        
        [ClientData("actionItems")]
        public List<NavItem> ActionItems
        {
            get
            {
                if (_actionItems == null)
                {
                    _actionItems = GetDefaultActionItems();
                }

                return _actionItems;

            }
            set { _actionItems = value; }
        }

        public bool ShowTabMenu
        {
            get { return (Tabs != null && Tabs.Count() > 1); }
        }
        
        public bool ShowAddTab { get; set; }
        
        
        public NavBarModel()
        {
            // by default, enable the button
            ShowAddTab = true;
            
        }

        #region ..:: Action Items ::..

        private List<NavItem> _actionItems = null;
        

        private List<NavItem> GetDefaultActionItems()
        {
            const string ShowTabMenuClass = "fi fi_d-double-arrow";
            const string ShowTabMenuActionId = "menu-tab";
            const string AddTabClass = "fi fi_circle-plus";
            const string AddTabActionId = "add-tab";
            const string AddTabTooltip = "Add New Tab";

            var actionItems = new List<NavItem>();

            if (ShowAddTab)
            {
                actionItems.Add(new NavItem
                {
                    IconClass = AddTabClass,
                    Id = AddTabActionId,
                    Tooltip = AddTabTooltip,
                    HasMenuItems = true
                    
                });
            }

            if (Tabs != null && Tabs.Count() > 1)
            {
                actionItems.Add(new NavItem
                {
                    IconClass = ShowTabMenuClass,
                    Id = ShowTabMenuActionId,
                    HasMenuItems = true,
                    CssClass = "dj_menuTab"
                });
            }

            return actionItems;
        }

        #endregion
    }
}
