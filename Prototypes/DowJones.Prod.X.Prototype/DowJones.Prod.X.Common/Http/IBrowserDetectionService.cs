namespace DowJones.Prod.X.Common.Http
{
    public interface IBrowserDetectionService
    {
        bool IsMobileSafari { get; }
        bool IsTabletSafari { get; }
        bool IsMobileAndroid { get; }
        bool IsTelevision { get; }
        bool IsDesktop { get; }
        string UserAgent { get; }
        DeviceType DeviceType { get; }
    }
}
