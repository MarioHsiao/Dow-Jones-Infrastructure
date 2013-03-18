using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DowJones.Pages.Layout
{
    [DataContract(Name = "zoneLayout", Namespace = "")]
    public class ZoneLayout : PageLayout
    {
        public const int DefaultZoneCount = 3;

        [DataMember(Name = "zones")]
        public List<Zone> Zones { get; set; }

        public ZoneLayout() : this(DefaultZoneCount)
        {
        }

        public ZoneLayout(int zoneCount)
            : this(new int[zoneCount].Select(x => new Zone()))
        {
        }

        public ZoneLayout(IEnumerable<Zone> zones)
        {
            Zones = new List<Zone>(zones ?? Enumerable.Empty<Zone>());
        }

        public override void AddModule(int moduleId)
        {
            var nextZone = Zones.FirstOrDefault();

            if(nextZone == null)
            {
                Zones.Add(new Zone(new [] { moduleId }));
            }
            else
            {
                nextZone.Add(moduleId);
            }
        }

        public override void RemoveModule(int moduleId)
        {
            foreach (var zone in Zones.Where(x => x.Contains(moduleId)))
            {
                zone.Remove(moduleId);
            }
        }


        [DataContract(Name = "zone", Namespace = "")]
        public class Zone : List<int>
        {
            public Zone()
            {
            }

            public Zone(IEnumerable<int> moduleIds) : base(moduleIds)
            {
            }
        }
    }
}