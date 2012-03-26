using System.Collections.Generic;
using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using DowJones.Web.Mvc.UI.Components.Models;

namespace DowJones.Web.Mvc.UI.Components.CompositeHeadline
{
    public class CompositeHeadlineModel : CompositeComponentModel
    {

        /// <summary>
        /// Total duplicate so far starting from 0 to current set of headlines
        /// </summary>
        public double TotalDuplicateCount { get; set; }

        /// <summary>
        /// Total duplicates in current headline set
        /// </summary>
        [ClientProperty("duplicateCount")]
        public double DuplicateCount { get; set; }

        /// <summary>
        /// Gets or sets the total result count.
        /// </summary>
        /// <value>
        /// The total result count.
        /// </value>
        [ClientProperty("totalResultCount")]
        public int TotalResultCount { get; set; }

        public ShowDuplicates ShowDuplicates { get; set; }


        public bool EnableDuplicateOption { get; set; }

        /// <summary>
        /// Gets or sets the last index of the result.
        /// </summary>
        /// <value>
        /// The last index of the result.
        /// </value>
        [ClientProperty("lastResultCount")]
        public int LastResultIndex { get; set; }

        /// <summary>
        /// Gets or sets the first index of the result.
        /// </summary>
        /// <value>
        /// The first index of the result.
        /// </value>
        [ClientProperty("firstResultCount")]
        public int FirstResultIndex { get; set; }

        /// <summary>
        /// Gets or sets the headline list.
        /// </summary>
        /// <value>
        /// The headline list.
        /// </value>
        public HeadlineListModel HeadlineList { get; set; }

        /// <summary>
        /// Gets or sets the post processing.
        /// </summary>
        /// <value>
        /// The post processing.
        /// </value>
        public PostProcessing PostProcessing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show headline view options button.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show headline view options]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowHeadlineViewOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show post processing toolbar.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show post processing]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowPostProcessing { get; set; }

        public bool CanPageNext { get; set; }

        public bool CanPagePrevious { get; set; }

        
        public IEnumerable<SelectListItem> HeadlineSortOptions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether NLA press clips are enabled.
        /// </summary>
        /// </value>
        public bool ShowPressClip { get; set; }

        public CompositeHeadlineModel()
        {
            HeadlineList = new HeadlineListModel();
            ShowHeadlineViewOptions = true;
            ShowPostProcessing = true;
        }



    }
}
