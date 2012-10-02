using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace DowJones.Dash.DataSources
{
    public class ConfigurationDataSource : DataSource, IDisposable
    {
        protected override ILog Log
        {
            get { return _log; }
        }
        private static readonly ILog _log = LogManager.GetLogger(typeof(ChartBeatDataSource));

        public dynamic Configuration { get; set; }

        public override void Start()
        {
           // Nothing to start
            OnDataReceived(Configuration);
        }

        public void Dispose()
        {
            // Nothing to dispose
        }



        public ConfigurationDataSource(string name, string dataName, dynamic configuration)
            : base(name, dataName)
        {
            Configuration = configuration;
        }
    }
}
