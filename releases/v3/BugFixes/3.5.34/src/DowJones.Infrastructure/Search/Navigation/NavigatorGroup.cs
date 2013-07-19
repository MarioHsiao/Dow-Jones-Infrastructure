using System.Collections.Generic;

namespace DowJones.Search.Navigation
{
    public class NavigatorGroup
    {
        public string Code { get; set; }

        public int Count { get; set; }

        public string Name { get; set; }

        public Navigator Navigator { get; set; }

        public NavigatorGroup()
        {
        }

        public NavigatorGroup(string code, int count, string name)
        {
            Code = code;
            Count = count;
            Name = name;
        }
    }

    public class SourceGroups : List<NavigatorGroup>
    {
        public string PrimaryGroupId { get; set; }
        public string SecondaryGroupId { get; set; }

        public SourceGroups()
        {
        }

        public SourceGroups(IEnumerable<NavigatorGroup> sourceGroups)
            : base(sourceGroups)
        {
        }
    }

    public class CompositeNavigatorGroup : NavigatorGroup
    {
        public IEnumerable<NavigatorGroup> Groups;
    }
}