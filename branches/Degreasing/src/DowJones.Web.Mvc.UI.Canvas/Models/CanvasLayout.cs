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

    public class GroupedCanvasLayout : CanvasLayout
    {
        [JsonProperty("groups")]
        public IEnumerable<IEnumerable<int>> Groups { get; set; }

        public GroupedCanvasLayout(int numberOfGroups = 3)
        {
            Groups = new IEnumerable<int>[numberOfGroups];
        }

        public class GroupedCanvasLayoutMapper : TypeMapper<GroupedPageLayout, CanvasLayout>
        {
            public override CanvasLayout Map(GroupedPageLayout source)
            {
                return new GroupedCanvasLayout
                           {
                               Groups = source.Groups.Select(x => x)
                           };
            }
        }
    }
}