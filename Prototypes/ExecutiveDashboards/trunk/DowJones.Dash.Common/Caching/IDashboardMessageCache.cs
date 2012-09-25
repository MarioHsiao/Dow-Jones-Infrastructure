using System.Collections.Generic;

namespace DowJones.Dash.Caching
{
    public interface IDashboardMessageCache
    {
        void Add(DashboardMessage message);
        IEnumerable<DashboardMessage> Get(params string[] eventNames);
    }
}