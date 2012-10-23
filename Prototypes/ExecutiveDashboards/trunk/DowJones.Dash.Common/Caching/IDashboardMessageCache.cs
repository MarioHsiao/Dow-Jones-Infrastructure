using System.Collections.Generic;

namespace DowJones.Dash.Caching
{
    public interface IDashboardMessageCache
    {
        void Add(DashboardMessage message);
        ICollection<DashboardMessage> Get(params string[] dataSources);
        ICollection<DashboardMessage> GetAll();
    }
}