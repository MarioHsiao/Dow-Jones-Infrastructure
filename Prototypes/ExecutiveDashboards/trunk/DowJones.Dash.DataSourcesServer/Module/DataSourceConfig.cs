using DowJones.Dash.Common.DataSources;

namespace DowJones.Dash.DataSourcesServer.Module
{
    public class DataSourceConfig
    {
        public DataSourcesModule.Site Site { get; set; }
        public DataSourcesModule.GomezCountryCode GomezCountryCode { get; set; }
        public string Host { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public BasicHostConfiguration HostConfiguration { get; set; }
        public MapType MapType { get; set; }
    }
}