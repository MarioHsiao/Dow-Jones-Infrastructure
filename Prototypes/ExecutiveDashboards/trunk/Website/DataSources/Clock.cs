using System;
using System.Threading;
using System.Threading.Tasks;

namespace DowJones.Dash.Website.DataSources
{
    public class Clock : DataSource
    {
        public Clock()
        {
            Task.Factory.StartNew(Poll);
        }

        protected void Poll()
        {
            while (true)
            {
                if(!Enabled)
                    continue;

                var data = new { Time = DateTime.Now };
                OnDataReceived(data);

                Thread.Sleep(100);
            }
        }
    }
}