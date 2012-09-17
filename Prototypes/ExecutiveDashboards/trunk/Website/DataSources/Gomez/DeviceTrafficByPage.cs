using DowJones.DependencyInjection;

namespace DowJones.Dash.Website.DataSources.Gomez
{
    public class DeviceTrafficByPage : GomezDataSource, IInitializable
    {
        public void Initialize()
        {
            Query = @"exec [SplunkExport].[dbo].[GetDeviceTrafficByPage] @pageid = 421139, @seconds = 300";
        }
    }
}