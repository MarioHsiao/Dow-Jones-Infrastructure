using System.Collections.Generic;
using System.Linq;

namespace DowJones.Web.Navigation
{
    public class Menu : List<IMenuNode>, IMenu
    {
        public IEnumerable<IMenuNode> Nodes
        {
            get { return this; }
        }

        public Menu(IEnumerable<IMenuNode> navigationMenuNodes = null)
            : base(navigationMenuNodes ?? Enumerable.Empty<IMenuNode>())
        {
        }
    }
}