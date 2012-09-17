using DowJones.DependencyInjection;

namespace DowJones.Dash.Website.DataSources.Gomez
{
    public class BrowserStats : GomezDataSource, IInitializable
    {
        public void Initialize()
        {
            Query = @"exec [GetPageLoadHistoricalDetails] @pageid = 421139, @days = 1";
        }
    }
}