using System;
using System.Collections.Generic;

namespace DowJones.Dash.Caching
{
    public interface IDashboardMessageQueue
    {
        void Enqueue(DashboardMessage message);
        ICollection<DashboardMessage> Get();
        bool IsActive();
    }
}