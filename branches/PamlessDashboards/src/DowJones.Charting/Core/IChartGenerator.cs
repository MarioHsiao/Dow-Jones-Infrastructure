namespace DowJones.Charting.Core
{
    internal interface IChartGenerator
    {
        IBytesResponse GetBytes();
        IUriResponse GetUri();
        IEmbeddedHTMLResponse GetHTML();
    }
}
