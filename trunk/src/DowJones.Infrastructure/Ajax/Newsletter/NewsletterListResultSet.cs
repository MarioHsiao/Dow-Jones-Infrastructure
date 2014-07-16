using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DowJones.Formatters;
using Newtonsoft.Json;

namespace DowJones.Ajax.Newsletter
{
    public class NewsletterListResultSet
    {
        private WholeNumber _count;

        [DataMember(Name = "count")]
        [JsonProperty("count")]
        public WholeNumber Count
        {
            get
            {
                return _count ?? (_count = new WholeNumber(Newsletters.Count));
            }
            set { _count = value; }
        }

        private List<NewsletterInfo> _newsletters;

        [DataMember(Name = "newsletters")]
        [JsonProperty("newsletters")]
        public List<NewsletterInfo> Newsletters
        {
            get
            {
                return _newsletters ?? (_newsletters = new List<NewsletterInfo>());
            }
            set { _newsletters = value; }
        }

        public NewsletterListResultSet()
        {
        }

        public NewsletterListResultSet(IEnumerable<NewsletterInfo> newsletters)
        {
            newsletters = newsletters ?? Enumerable.Empty<NewsletterInfo>();
            Newsletters = new List<NewsletterInfo>(newsletters);
        }
    }
}
