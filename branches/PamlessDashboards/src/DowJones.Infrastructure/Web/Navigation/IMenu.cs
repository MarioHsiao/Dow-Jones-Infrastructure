using System.Collections.Generic;

namespace DowJones.Web.Navigation
{
    public interface IMenu
    {
        IEnumerable<IMenuNode> Nodes { get; }
    }
}