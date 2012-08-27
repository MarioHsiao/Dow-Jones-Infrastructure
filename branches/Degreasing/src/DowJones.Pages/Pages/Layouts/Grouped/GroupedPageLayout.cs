using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DowJones.Pages
{
    [DataContract(Name = "groupedLayout", Namespace = "")]
    public class GroupedPageLayout : PageLayout
    {
        public const int DefaultNumberOfGroups = 3;

        [DataMember(Name = "groups")]
        public List<Group> Groups { get; set; }

        public GroupedPageLayout() : this(DefaultNumberOfGroups)
        {
        }

        public GroupedPageLayout(int numberOfGroups)
            : this(new int[numberOfGroups].Select(x => new Group()))
        {
        }

        public GroupedPageLayout(IEnumerable<Group> groups)
        {
            Groups = new List<Group>(groups ?? Enumerable.Empty<Group>());
        }

        public override void AddModule(int moduleId)
        {
            var nextGroup = Groups.FirstOrDefault();

            if(nextGroup == null)
            {
                Groups.Add(new Group(new [] { moduleId }));
            }
            else
            {
                nextGroup.Add(moduleId);
            }
        }

        public override void RemoveModule(int moduleId)
        {
            foreach (var @group in Groups.Where(x => x.Contains(moduleId)))
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