

using DowJones.Tools.Managers.Charting;

namespace DowJones.Tools.Charting
{
    public abstract class AbstractChartGenerator : IChartGenerator
    {
        public abstract IBytesResponse GetBytes();

        public abstract IUriResponse GetUri();

        public abstract IEmbeddedHTMLResponse GetHTML();

        internal abstract ChartTemplate GetChartTemplate();

        #region Implementation of IGeneratesITXML

        internal abstract string ToITXML();

        #endregion
    }
}
