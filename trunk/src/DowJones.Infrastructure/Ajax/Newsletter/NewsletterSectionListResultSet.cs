using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DowJones.Formatters;
using Newtonsoft.Json;

namespace DowJones.Ajax.Newsletter
{
    public class NewsletterSectionListResultSet
    {
         private WholeNumber _count;

        [DataMember(Name = "count")]
        [JsonProperty("count")]
        public WholeNumber Count
        {
            get
            {
                return _count ?? (_count = new WholeNumber(NewsletterSections.Count));
            }
            set { _count = value; }
        }

        private List<NewsletterSectionInfo> _newsletterSections;

        [DataMember(Name = "newsletterSections")]
        [JsonProperty("newsletterSections")]
        public List<NewsletterSectionInfo> NewsletterSections
        {
            get
            {
                return _newsletterSections ?? (_newsletterSections = new List<NewsletterSectionInfo>());
            }
            set { _newsletterSections = value; }
        }

        public NewsletterSectionListResultSet()
        {
        }

        public NewsletterSectionListResultSet(IEnumerable<NewsletterSectionInfo> newsletterSections)
        {
            newsletterSections = newsletterSections ?? Enumerable.Empty<NewsletterSectionInfo>();
            NewsletterSections = new List<NewsletterSectionInfo>(newsletterSections);
        }
    }
}
