using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace DowJones.Dash.DataSources
{
    public abstract class PollingDataSource : DataSource
    {
        private static readonly Func<int> DefaultPollDelayFactory = 
            () => Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPollDelay"]);

        public int ErrorDelay
        {
            get { return _errorDelay.Value; }
            set { _errorDelay = new Lazy<int>(() => value); }
        }
        private Lazy<int> _errorDelay;

        public int PollDelay
        {
            get { return _pollDelay.Value; }
            set { _pollDelay = new Lazy<int>(() => value); }
        }
        private Lazy<int> _pollDelay;


        protected PollingDataSource(string name = null, Func<int> pollDelayFactory = null, Func<int> errorDelayFactory = null)
            : base(name)
        {
            _pollDelay = new Lazy<int>(pollDelayFactory ?? DefaultPollDelayFactory);

            errorDelayFactory = errorDelayFactory ?? (() => _pollDelay.Value * 2);
            _errorDelay = new Lazy<int>(errorDelayFactory);
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