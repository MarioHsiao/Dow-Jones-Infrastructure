using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Utilities.Formatters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DowJones.Tools.Ajax.PortalHeadlineList
{
    [DataContract(Name="portalHeadlineListResultSet", Namespace = "")]
    public class PortalHeadlineListResultSet
    {
        private WholeNumber _count;

        [DataMember(Name = "count")]
        [JsonProperty("count")]
        public WholeNumber Count
        {
            get
            {
                return _count ?? (_count = new WholeNumber(Headlines.Count));
            }
            set { _count = value; }
        }

        private WholeNumber _first;

        [DataMember(Name = "first")]
        [JsonProperty("first")]
        public WholeNumber First
        {
            get
            {
                return _first ?? (_first = new WholeNumber(0));
            }
            set { _first = value; }
        }

        private WholeNumber _duplicateCount;

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        [DataMember(Name = "duplicateCount")]
        [JsonProperty("duplicateCount")]
        public WholeNumber DuplicateCount
        {
            get { return _duplicateCount ?? (_duplicateCount = new WholeNumber(0)); }
            set { _duplicateCount = value; }
        }


        private List<PortalHeadlineInfo> _headlines;

        [DataMember(Name = "headlines")]
        [JsonProperty("headlines")]
        public List<PortalHeadlineInfo> Headlines
        {
            get
            {
                return _headlines ?? (_headlines = new List<PortalHeadlineInfo>());
            }
            set { _headlines = value; }
        }
    }
}
