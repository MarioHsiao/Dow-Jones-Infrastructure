

using DowJones.Charting.Manager;

namespace DowJones.Charting.Core
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
