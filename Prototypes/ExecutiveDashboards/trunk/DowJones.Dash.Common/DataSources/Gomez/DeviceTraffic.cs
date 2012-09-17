using DowJones.DependencyInjection;

namespace DowJones.Dash.DataSources.Gomez
{
    public class DeviceTraffic : GomezDataSource, IInitializable
    {
        public void Initialize()
        {
            Query = @"exec [SplunkExport].[dbo].[GetDeviceTraffic] @seconds = 300";
        }
    }
}