using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using log4net;

namespace DowJones.Dash.Common.DataSources
{
    public interface IHostConfiguration
    {
        string Region { get; set; }
        string Domain { get; set; }
        MapType MapType { get; set; }
        IEnumerable<PerformanceZone> PerformanceZones { get; set; }
    }

    public enum MapType
    {
        ProvinceState,
        Country,
        World,
        City,
	    Region
    }

    public enum PerformanceZoneType
    {
        Hot,
        Cool,
        Neutral,
    }

    public class PerformanceZone
    {
        [JsonProperty("to")]
        public decimal To { get; set; }
        
        [JsonProperty("from")]
        public decimal From { get; set; }
        
        [JsonProperty("zoneType")]
        public PerformanceZoneType ZoneType { get; set; }
    }
    
    public class BasicHostConfiguration : IHostConfiguration
    {
        [JsonProperty("map")]
        public string Region { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("mapType")]
        public MapType MapType { get; set; }

        [JsonProperty("performanceZones")]
        public IEnumerable<PerformanceZone> PerformanceZones { get; set; }
    }

    public class ConfigurationDataSource<T> : DataSource, IDisposable where T:IHostConfiguration
    {
        protected override ILog Log
        {
            get { return _log; }
        }
        private static readonly ILog _log = LogManager.GetLogger(typeof(ChartBeatApiDataSource));

        public T Configuration { get; set; }

        public override void Start()
        {
           // Nothing to start
            OnDataReceived(Configuration);
        }

        public void Dispose()
        {
            // Nothing to dispose
        }
        
        public ConfigurationDataSource(string name, string dataName, T configuration)
            : base(name, dataName)
        {
            Configuration = configuration;
        }
    }
}
