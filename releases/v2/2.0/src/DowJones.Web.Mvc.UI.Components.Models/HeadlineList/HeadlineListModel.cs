using DowJones.Ajax.HeadlineList;
using System.Collections.Generic;
using System.Linq;
using DowJones.Converters;
using DowJones.Web.Mvc.UI.Components.Common.Types;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.HeadlineList
{
    [JsonConverter(typeof(GeneralJsonEnumConverter))]
    public enum ShowDuplicates
    {
        On,
        Off,
    }

    public class HeadlineListModel : ViewComponentModel
    {
        [ClientProperty("showDuplicates")]
        public ShowDuplicates ShowDuplicates
        {
            get;
            set;
        }

        [ClientProperty("showCheckboxes")]
        public bool ShowCheckboxes { get; set; }

        [ClientProperty("showAccessionNo")]
        public bool ShowAccessionNo { get; set; }

        [ClientProperty("showByLine")]
        public bool ShowByLine { get; set; }

        [ClientProperty("displaySnippets")]
        public SnippetDisplayType DisplaySnippets { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of headlines that can be selected.
        /// </summary>
        /// <value>
        /// The max select.
        /// </value>
        /// <remarks>Default is 0 which allows unlimited selection</remarks>
        [ClientProperty("maxSelect")]
        public int MaxSelect { get; set; }

        public bool DuplicatesAreVisible(HeadlineModel headline)
        {
            return ShowDuplicates == ShowDuplicates.On && (headline != null && headline.DuplicateHeadlines.Any());
         }


        public double TotalRecords { get; set; }

        public IEnumerable<HeadlineModel> Headlines { get; set; }

        public HeadlineListModel()
        {
            Headlines = Enumerable.Empty<HeadlineModel>();
            MaxSelect = 0;      // allow unlimited selections
        }


        public HeadlineListModel(HeadlineListDataResult headlineResult)
            : this()
        {
            Headlines = headlineResult.resultSet.headlines.Select(h => new HeadlineModel(h));
            TotalRecords = headlineResult.hitCount.Value;
        }
    }
}