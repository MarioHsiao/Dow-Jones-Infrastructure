using System.Collections.Generic;

namespace DowJones.Web.Navigation
{
    public interface IMenuNode : IEnumerable<IMenuNode>
    {
        string ID { get; set; }
        string DisplayName { get; set; }
        bool IsActive { get; set; }
        IEnumerable<IMenuNode> Nodes { get; }
        IDictionary<string, string> Properties { get; }
    }
}