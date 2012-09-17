using DowJones.DependencyInjection;

namespace DowJones.Dash.Website.DataSources.Gomez
{
    public class BrowserStats : GomezDataSource, IInitializable
    {
        public void Initialize()
        {
            Query = @"exec [SplunkExport].[dbo].[GetPageLoadDetailsByBrowser] @pageid = 421139, @seconds = 300 ";
        }
    }
}