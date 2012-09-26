using System;
using log4net;

namespace DowJones.Dash.DataSources
{
    public class DataReceivedEventArgs : EventArgs
    {
        public object Data { get; private set; }

        public DataReceivedEventArgs(object data)
        {
            Data = data;
        }
    }

    public class ErrorEventArgs : EventArgs
    {
        public Exception Exception { get; private set; }

        public ErrorEventArgs(Exception exception = null)
        {
            Exception = exception;
        }
    }

    public interface IDataSource
    {
        event EventHandler<DataReceivedEventArgs> DataReceived;
        event EventHandler<ErrorEventArgs> Error;
        string Name { get; }
        void Start();
    }

    public abstract class DataSource : IDataSource
    {
        protected abstract ILog Log { get; }

        private readonly string _name;

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        public event EventHandler<ErrorEventArgs> Error;

        public virtual string Name
        {
            get { return _name; }
        }

        protected bool Enabled
        {
            get { return DataReceived != null; }
        }

        protected DataSource(string name = null)
        {
            _name = name ?? GetType().Name;
        }

        public abstract void Start();

        protected virtual void OnDataReceived(object data)
        {
            if (DataReceived != null && data != null)
            {
                Log.Debug("Data received");
                DataReceived(this, new DataReceivedEventArgs(data));
            }
            else
            {
                Log.Debug("Data received, but no listeners!");
            }
        }

        protected virtual void OnError(Exception ex = null)
        {
            Log.WarnFormat("Data Source {0} Failed: {1}", GetType().Name, ex);

            if (Error != null)
                Error(this, new ErrorEventArgs(ex));
        }
    }
}