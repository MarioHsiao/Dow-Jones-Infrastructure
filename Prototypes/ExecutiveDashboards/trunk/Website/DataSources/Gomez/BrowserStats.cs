using DowJones.DependencyInjection;

namespace DowJones.Dash.Website.DataSources.Gomez
{
    public class BrowserStats : GomezDataSource, IInitializable
    {
        public void Initialize()
        {
            Query = @"exec [SplunkExport].[dbo].[GetPageLoadDetailsByBrowser] @seconds = 300 ";
        }
    }
}