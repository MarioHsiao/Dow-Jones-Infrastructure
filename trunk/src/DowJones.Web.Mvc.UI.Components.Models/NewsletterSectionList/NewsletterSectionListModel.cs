using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Web.Mvc.UI.Components.NewsletterSectionList
{
    public class NewsletterSectionListModel
    {
        [ClientProperty("nlid")]
        public long NewsletterId { get; set; }
        
        [ClientProperty("sections")]
        public IEnumerable<Section> Sections { get; set; }

        public NewsletterSectionListModel()
        {
            Sections = Enumerable.Empty<Section>();
        }
    }

    public class Section
    {
        [ClientProperty("name")]
        public string Name { get; set; }

        [ClientProperty("index")]
        public int Index { get; set; }

        [ClientProperty("positionIndicator")]
        public string PositionIndicator { get; set; }
    }

}
