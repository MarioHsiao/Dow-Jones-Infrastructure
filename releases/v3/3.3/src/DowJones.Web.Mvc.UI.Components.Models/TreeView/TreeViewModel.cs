using System.Collections.Generic;
using DowJones.Models.TreeView;

namespace DowJones.Web.Mvc.UI.Components.TreeView
{
    public class TreeViewModel : ViewComponentModel
    {
        #region ..:: Client Properties ::..
        [ClientProperty("enableCheckboxes")]
        public bool EnableCheckboxes { get; set; }
        #endregion


        #region ..:: Client Data ::..

        [ClientData]
        public List<TreeViewNode> Data { get; set; }

        #endregion


        #region ..:: Client Event Handlers ::..
        #endregion      
           
    }
}
