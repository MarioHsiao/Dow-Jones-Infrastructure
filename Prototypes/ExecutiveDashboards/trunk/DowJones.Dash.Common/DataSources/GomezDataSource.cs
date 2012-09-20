using System;
using System.Collections.Generic;
using System.Configuration;

namespace DowJones.Dash.DataSources
{
    public class GomezDataSource : SqlDataSource
    {
        public GomezDataSource(string name, string query = null, IDictionary<string,object> parameters = null)
            : base(name, ConfigurationManager.ConnectionStrings["Gomez"].ConnectionString, query, parameters,
                   () => Convert.ToInt32(ConfigurationManager.AppSettings["Gomez.PollDelay"]))
        {
        }
    }
}