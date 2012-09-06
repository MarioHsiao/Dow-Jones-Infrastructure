using System.Collections.Generic;
using System.Linq;
using DowJones.Mapping;
using DowJones.Pages;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public abstract class CanvasLayout
    {
    }

    public class ZoneCanvasLayout : CanvasLayout
    {
        [JsonProperty("zoneCount")]
        public int? ZoneCount { get; set; }

        [JsonProperty("zones")]
        public IEnumerable<IEnumerable<int>> Zones { get; set; }

        public ZoneCanvasLayout(int? zoneCount = null)
        {
            ZoneCount = zoneCount;
        }

        public class ZoneCanvasLayoutMapper : TypeMapper<ZonePageLayout, CanvasLayout>
        {
            public override CanvasLayout Map(ZonePageLayout source)
            {
                return new ZoneCanvasLayout
                           {
                               ZoneCount = source.Zones.Count,
                               Zones = source.Zones.Select(x => x)
                           };
            }
        }
    }
}