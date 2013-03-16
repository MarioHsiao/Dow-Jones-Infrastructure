using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.Appearance;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class Chart : GenericChart
    {
        protected string Script;

        public Chart(string clientId, RenderType renderType)
        {
            ClientId = clientId;
            RenderType = renderType;
            Appearance = new Appearance {RenderTo = clientId};
        }

        public string ClientId { get; set; }
    }
}