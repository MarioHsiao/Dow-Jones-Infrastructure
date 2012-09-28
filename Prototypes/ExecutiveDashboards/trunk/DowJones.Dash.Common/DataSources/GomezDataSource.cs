using System;
using System.Collections.Generic;
using System.Configuration;
using log4net;

namespace DowJones.Dash.DataSources
{
    public class GomezDataSource : SqlDataSource
    {
        protected override ILog Log
        {
            get { return _log; }
        }
        private static readonly ILog _log = LogManager.GetLogger(typeof(GomezDataSource));


        public GomezDataSource(string name, string dataName, string query = null, IDictionary<string, object> parameters = null)
            : base(name, dataName, ConfigurationManager.ConnectionStrings["Gomez"].ConnectionString, query, parameters,
                   () => Convert.ToInt32(ConfigurationManager.AppSettings["Gomez.PollDelay"]))
        {
        }
    }
}