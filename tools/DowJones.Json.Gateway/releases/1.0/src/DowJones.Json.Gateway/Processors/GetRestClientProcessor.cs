using log4net;
using RestSharp;

namespace DowJones.Json.Gateway.Processors
{
    internal class GetRestClientProcessor : UrlBasedRestClientProcessor
    {
        private ILog _log = LogManager.GetLogger(typeof(UrlBasedRestClientProcessor));

        internal override ILog Log
        {
            get { return _log; }
            set { _log = value; }
        }

        protected override Method Method
        {
            get { return Method.GET; }
        }
    }
}