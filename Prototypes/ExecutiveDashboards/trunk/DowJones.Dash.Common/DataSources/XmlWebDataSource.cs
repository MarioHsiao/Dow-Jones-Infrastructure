using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using log4net;

namespace DowJones.Dash.Common.DataSources
{
    public class XmlWebDataSource : WebDataSource
    {
        protected override ILog Log
        {
            get { return _log; }
        }
        private static readonly ILog _log = LogManager.GetLogger(typeof(XmlWebDataSource));

        public XmlWebDataSource(string name, string dataName, string path, IDictionary<string, object> parameters = null, Func<int> pollDelayFactory = null, Func<int> errorDelayFactory = null)
            : base(name,dataName, path, parameters, pollDelayFactory, errorDelayFactory)
        {
        }

        protected internal override dynamic ParseResponse(Stream stream)
        {
            XNode node = XNode.ReadFrom(new XmlTextReader(new StreamReader(stream)));
            var json = JsonConvert.SerializeXNode(node, Newtonsoft.Json.Formatting.None, true);
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            return data;
        }
    }
}