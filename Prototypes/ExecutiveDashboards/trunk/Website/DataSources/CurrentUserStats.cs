using System;
using System.Threading;
using System.Threading.Tasks;

namespace DowJones.Dash.Website.DataSources
{
    public class CurrentUserStats : DataSource
    {
        private readonly Random _rand;
        private readonly UserStats _currentStats;

        public CurrentUserStats()
        {
            _rand = new Random();
            _currentStats = new UserStats
                {
                    DesktopUserCount = _rand.Next(5000, 6000),
                    MobileUserCount = _rand.Next(1000, 2000),
                };

            Task.Factory.StartNew(Poll);
        }

        protected internal void Poll()
        {
            while (true)
            {
                if (!Enabled)
                    continue;

                _currentStats.DesktopUserCount += _rand.Next(-10, 10);
                _currentStats.MobileUserCount += _rand.Next(-10, 10);

                OnDataReceived(_currentStats);

                Thread.Sleep(_rand.Next(10, 2000));
            }
        }

        public class UserStats
        {
            public int DesktopUserCount { get; set; }
            public int MobileUserCount { get; set; }
        }
    }
}