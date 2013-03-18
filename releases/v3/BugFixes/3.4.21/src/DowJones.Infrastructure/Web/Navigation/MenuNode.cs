using System.Collections.Generic;
using System.Linq;

namespace DowJones.Web.Navigation
{
    public class MenuNode : List<IMenuNode>, IMenuNode
    {
        public string ID { get; set; }
        
        public string DisplayName { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<IMenuNode> Nodes
        {
            get { return this; }
        }

        public IDictionary<string, string> Properties { get; private set; }


        public MenuNode(string displayDisplayName, IEnumerable<IMenuNode> children = null)
            : base(children ?? Enumerable.Empty<IMenuNode>())
        {
            DisplayName = displayDisplayName;
            Properties = new Dictionary<string, string>();
        }
    }
}