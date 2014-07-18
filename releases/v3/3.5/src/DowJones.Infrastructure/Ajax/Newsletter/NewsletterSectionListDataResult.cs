using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DowJones.Formatters;
using Newtonsoft.Json;

namespace DowJones.Ajax.Newsletter
{
    [DataContract(Name = "newsletterSectionListDataResult", Namespace = "")]
    public class NewsletterSectionListDataResult : IListDataResult
    {
         private NewsletterSectionListResultSet _resultSet;

        [DataMember(Name = "resultSet")]
        [JsonProperty("resultSet")]
        public NewsletterSectionListResultSet ResultSet
        {
            get
            {
                return _resultSet ?? (_resultSet = new NewsletterSectionListResultSet());
            }

            set
            {
                _resultSet = value;
            }
        }

        public NewsletterSectionListDataResult()
        {
        }

        public NewsletterSectionListDataResult(NewsletterSectionListResultSet resultSet)
        {
            if (resultSet == null)
                return;

            ResultSet = resultSet;
        }
    }
}
