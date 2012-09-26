using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace DowJones.Dash.DataSources
{
    public abstract class PollingDataSource : DataSource, IDisposable
    {
        private static readonly Func<int> DefaultPollDelayFactory = 
            () => Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPollDelay"]);

        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

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


        public void Dispose()
        {
            _cancellationToken.Cancel(false);
        }

        public override void Start()
        {
            Task.Factory.StartNew(
                () => Poll(null), 
                _cancellationToken.Token
            );
        }

        protected override void OnDataReceived(object data)
        {
            base.OnDataReceived(data);
            Poll(PollDelay);
        }

        protected override void OnError(Exception ex = null)
        {
            base.OnError(ex);
            Poll(ErrorDelay);
        }

        protected internal void Poll(int? delay)
        {
            if(delay != null)
            {
                Log.DebugFormat("Waiting for {0} seconds...", delay);

                Thread.Sleep(delay.Value * 1000);
            }

            Log.Debug("Polling for data...");
            Poll();
        }

        protected abstract void Poll();
    }
}