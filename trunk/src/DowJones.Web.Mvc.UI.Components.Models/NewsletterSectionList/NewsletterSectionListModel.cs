using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.NewsletterSectionList
{
    public class NewsletterSectionListModel : ViewComponentModel
    {
        [JsonProperty("nlid")]
        public long NewsletterId { get; set; }

        [JsonProperty("sections")]
        public IEnumerable<Section> Sections { get; set; }

        public NewsletterSectionListModel()
        {
            Sections = Enumerable.Empty<Section>();
        }
    }

    public class Section
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("positionIndicator")]
        public string PositionIndicator { get; set; }
    }

}
