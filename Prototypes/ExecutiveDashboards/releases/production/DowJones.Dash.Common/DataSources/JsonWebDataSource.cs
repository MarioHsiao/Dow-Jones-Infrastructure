using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using log4net;

namespace DowJones.Dash.Common.DataSources
{
    public class JsonWebDataSource : WebDataSource
    {
        protected override ILog Log
        {
            get { return _log; }
        }
        private static readonly ILog _log = LogManager.GetLogger(typeof(JsonWebDataSource));

        public JsonWebDataSource(string name, string dataName, string path, IDictionary<string, object> parameters = null, Func<int> pollDelayFactory = null, Func<int> errorDelayFactory = null)
            : base(name, dataName, path, parameters, pollDelayFactory, errorDelayFactory)
        {
        }

        protected internal override dynamic ParseResponse(Stream stream)
        {
            var json = new StreamReader(stream).ReadToEnd();
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            return data;
        }
    }
}