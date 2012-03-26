using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DowJones.Tools.Ajax.HeadlineList;
using System.Text;
using System.Text.RegularExpressions;

namespace DowJones.Web.Mvc.UI.Components.Models.HeadlineList
{
    public class HeadlineListModel// : ViewComponentModel
    {
        [DowJones.Web.Mvc.UI.ClientProperty("showDuplicates")]
        public bool ShowDuplicates { get; set; }
        [DowJones.Web.Mvc.UI.ClientProperty("showCheckboxes")]
        public bool ShowCheckboxes { get; set; }

        public List<HeadlineModel> Headlines { get; set; }

        public HeadlineListModel()
        {
            this.Headlines = new List<HeadlineModel>();
        }
        public HeadlineListModel(HeadlineListDataResult headlineResult)
            : this()
        {            
            var headlines = headlineResult.resultSet.headlines; 
            foreach (var headline in headlines)
            {
                this.Headlines.Add(new HeadlineModel(headline));
            }
        }
    }
}