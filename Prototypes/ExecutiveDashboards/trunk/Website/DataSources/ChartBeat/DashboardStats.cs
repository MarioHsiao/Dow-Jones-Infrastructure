using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DowJones.Dash.Website.DataSources.ChartBeat
{
    public class DashboardStats  : ChartBeatDataSource
    {
        public DashboardStats() : base("/dashapi/stats/")
        {
        }
    }
}