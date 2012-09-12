using System.Collections.Generic;
using System.Linq;
using DowJones.Mapping;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public class ZoneLayout : CanvasLayout
    {
        [JsonProperty("zoneCount")]
        public int? ZoneCount { get; set; }

        [JsonProperty("zones")]
        public IEnumerable<IEnumerable<int>> Zones { get; set; }

        public ZoneLayout(int? zoneCount = null)
        {
            ZoneCount = zoneCount;
        }

        public class ZoneLayoutMapper : TypeMapper<Pages.Layout.ZoneLayout, CanvasLayout>
        {
            public override CanvasLayout Map(Pages.Layout.ZoneLayout source)
            {
                return new ZoneLayout
                           {
                               ZoneCount = source.Zones.Count,
                               Zones = source.Zones.Select(x => x)
                           };
            }
        }
    }
}