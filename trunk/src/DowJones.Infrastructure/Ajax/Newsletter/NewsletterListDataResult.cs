using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DowJones.Ajax.Newsletter;
using DowJones.Formatters;
using Newtonsoft.Json;

namespace DowJones.Ajax.Newsletter
{
    [DataContract(Name = "newsletterListDataResult", Namespace = "")]
    public class NewsletterListDataResult : IListDataResult
    {
        private NewsletterListResultSet _resultSet;
        
        [DataMember(Name = "resultSet")]
        [JsonProperty("resultSet")]
        public NewsletterListResultSet ResultSet
        {
            get
            {
                return _resultSet ?? (_resultSet = new NewsletterListResultSet());
            }

            set
            {
                _resultSet = value;
            }
        }

        public NewsletterListDataResult()
        {
        }

        public NewsletterListDataResult(NewsletterListResultSet resultSet)
        {
            if (resultSet == null)
                return;

            ResultSet = resultSet;
        }
    }
}
