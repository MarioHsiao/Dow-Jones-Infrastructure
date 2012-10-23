using System;
using System.Configuration;
using System.Threading;
using DowJones.Utilities;

namespace DowJones.Dash.Common.DataSources
{
    public abstract class PollingDataSource : DataSource, IDisposable
    {
        private static readonly Func<int> DefaultPollDelayFactory =
            () => Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPollDelay"]);

        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private Lazy<int> _errorDelay;

        private Lazy<int> _pollDelay;

        protected PollingDataSource(string name = null, string dataName = null, Func<int> pollDelayFactory = null, Func<int> errorDelayFactory = null)
            : base(name, dataName)
        {
            _pollDelay = new Lazy<int>(pollDelayFactory ?? DefaultPollDelayFactory);

            errorDelayFactory = errorDelayFactory ?? (() => _pollDelay.Value * 2);
            _errorDelay = new Lazy<int>(errorDelayFactory);
        }

        public int ErrorDelay
        {
            get { return _errorDelay.Value; }
            set { _errorDelay = new Lazy<int>(() => value); }
        }
        public int PollDelay
        {
            get { return _pollDelay.Value; }
            set { _pollDelay = new Lazy<int>(() => value); }
        }
        public void Dispose()
        {
            _cancellationToken.Cancel(false);
        }

        public override void Start()
        {
            TaskFactoryManager.Instance.GetDefaultTaskFactory().StartNew(
                () => Poll(null),
                _cancellationToken.Token
            );
        }

        protected internal void Poll(int? delay)
        {
            if (delay != null)
            {
                Log.DebugFormat("Waiting for {0} seconds...", delay);

                Thread.Sleep(delay.Value * 1000);
            }

            Log.Debug("Polling for data...");
            Poll();
        }

        protected override void OnDataReceived(object data, string name = null)
        {
            base.OnDataReceived(data, name);
            Poll(PollDelay);
        }

        protected override void OnError(Exception ex = null, string name = null)
        {
            // Don't let thread aborts go on forever
            if (ex is ThreadAbortException || (ex != null && ex.InnerException is ThreadAbortException))
            {
                if (_cancellationToken != null && !_cancellationToken.IsCancellationRequested)
                    _cancellationToken.Cancel(false);

                throw ex;
            }

            if (ex != null)
            {
                base.OnError(ex: ex);
            }

            Poll(ErrorDelay);
        }
        protected abstract void Poll();
    }
}