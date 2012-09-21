using DowJones.DependencyInjection;

namespace DowJones.Dash.DataSources.Gomez
{
	public class PageLoadDetailsByCountry : GomezDataSource, IInitializable
    {
        public void Initialize()
        {
			Query = @"exec [SplunkExport].[dbo].[GetPageLoadDetailsByCountry] @seconds = 600";
        }
    }
}