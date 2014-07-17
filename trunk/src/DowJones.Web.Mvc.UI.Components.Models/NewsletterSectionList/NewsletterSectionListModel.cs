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

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("subSections")]
        public IEnumerable<Section> SubSections { get; set; }
    }

}
