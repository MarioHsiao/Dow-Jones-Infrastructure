namespace DowJones.Tools.Charting
{
    internal interface IChartGenerator
    {
        IBytesResponse GetBytes();
        IUriResponse GetUri();
        IEmbeddedHTMLResponse GetHTML();
    }
}
