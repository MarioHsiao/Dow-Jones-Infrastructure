using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace DowJones.Dash.DataSources
{
    public abstract class PollingDataSource : DataSource
    {
        public int ErrorDelay
        {
            get { return PollDelay * 2; }
        }

        public int PollDelay { get; set; }

        public PollingDataSource()
        {
            if(PollDelay == 0)
            {
                PollDelay = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPollDelay"]);
                Log("No poll delay set - defaulting to {0}", PollDelay);
            }
        }

        public override void Start()
        {
            StartNewThread();
        }

        protected override void OnDataReceived(object data)
        {
            base.OnDataReceived(data);
            StartNewThread(PollDelay);
        }

        protected override void OnError(Exception ex = null)
        {
            base.OnError(ex);
            StartNewThread(ErrorDelay);
        }


        protected abstract void Poll();


        private void StartNewThread(int? delay = null)
        {
            if(delay != null)
            {
                Log("Waiting for {0} seconds...", delay);

                Thread.Sleep(delay.Value * 1000);
            }

            Log("Polling for data...");
            Task.Factory.StartNew(Poll);
        }
    }
}