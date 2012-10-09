using DowJones.Dash.DataSources;

namespace DowJones.Dash.Website
{
	public class DataSourceConfig
	{
		public DataSources.Site Site { get; set; }
		public DataSources.GomezCountryCode GomezCountryCode { get; set; }
		public string Host { get; set; }
		public BasicHostConfiguration HostConfiguration { get; set; }
	}
}