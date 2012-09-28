using System;
using log4net;

namespace DowJones.Dash.DataSources
{
    public abstract class DataSourceEvent : EventArgs
    {
        public string Name { get; private set; }

        public class DataReceived : DataSourceEvent
        {
            public object Data { get; private set; }

            public DataReceived(string name, object data)
            {
                Name = name;
                Data = data;
            }
        }

        public class Error : DataSourceEvent
        {
            public Exception Exception { get; private set; }

            public Error(string name, Exception exception = null)
            {
                Name = name;
                Exception = exception;
            }
        }
    }

    public interface IDataSource
    {
        event EventHandler<DataSourceEvent.DataReceived> DataReceived;
        event EventHandler<DataSourceEvent.Error> Error;
        string Name { get; }
        void Start();
    }

    public abstract class DataSource : IDataSource
    {
        protected abstract ILog Log { get; }

        private readonly string _name;

        public event EventHandler<DataSourceEvent.DataReceived> DataReceived;

        public event EventHandler<DataSourceEvent.Error> Error;

        public string DataName { get; set; }

        public virtual string Name
        {
            get { return _name; }
        }

        protected bool Enabled
        {
            get { return DataReceived != null; }
        }

        protected DataSource(string name = null, string dataName = null)
        {
            _name = name ?? GetType().Name;
            DataName = dataName ?? Name;
        }

        public abstract void Start();

        protected virtual void OnDataReceived(object data, string name = null)
        {
            if (DataReceived != null && data != null)
            {
                Log.Debug("Data received");
                DataReceived(this, new DataSourceEvent.DataReceived(name ?? DataName, data));
            }
            else
            {
                Log.Debug("Data received, but no listeners!");
            }
        }

        protected virtual void OnError(Exception ex = null, string name = null)
        {
            Log.WarnFormat("Data Source {0} Failed: {1}", GetType().Name, ex);

            if (Error != null)
                Error(this, new DataSourceEvent.Error(name ?? DataName, ex));
        }
    }
}