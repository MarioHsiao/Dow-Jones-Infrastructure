using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DowJones.Pages
{
    [DataContract(Name = "groupedLayout", Namespace = "")]
    public class ZonePageLayout : PageLayout
    {
        public const int DefaultNumberOfGroups = 3;

        [DataMember(Name = "groups")]
        public List<Group> Zones { get; set; }

        public ZonePageLayout() : this(DefaultNumberOfGroups)
        {
        }

        public ZonePageLayout(int numberOfGroups)
            : this(new int[numberOfGroups].Select(x => new Group()))
        {
        }

        public ZonePageLayout(IEnumerable<Group> groups)
        {
            Zones = new List<Group>(groups ?? Enumerable.Empty<Group>());
        }

        public override void AddModule(int moduleId)
        {
            var nextGroup = Zones.FirstOrDefault();

            if(nextGroup == null)
            {
                Zones.Add(new Group(new [] { moduleId }));
            }
            else
            {
                nextGroup.Add(moduleId);
            }
        }

        public override void RemoveModule(int moduleId)
        {
            foreach (var @group in Zones.Where(x => x.Contains(moduleId)))
            {
                @group.Remove(moduleId);
            }
        }


        [DataContract(Name = "group", Namespace = "")]
        public class Group : List<int>
        {
            public Group()
            {
            }

            public Group(IEnumerable<int> groups) : base(groups)
            {
            }
        }
    }
}