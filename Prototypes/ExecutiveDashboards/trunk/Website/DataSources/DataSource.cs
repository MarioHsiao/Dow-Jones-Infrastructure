using System;
using System.Diagnostics;

namespace DowJones.Dash.Website.DataSources
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
        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public event EventHandler<ErrorEventArgs> Error;

        public virtual string Name
        {
            get { return GetType().Name; }
        }

        protected bool Enabled
        {
            get { return DataReceived != null; }
        }

        public abstract void Start();

        protected virtual void OnDataReceived(object data)
        {
            if (DataReceived != null && data != null)
            {
                Log("Data received");
                DataReceived(this, new DataReceivedEventArgs(data));
            }
            else
            {
                Log("Data received, but no listeners!");
            }
        }

        protected virtual void OnError(Exception ex = null)
        {
            Trace.TraceError("{0} Failed: {1}", GetType().Name, ex);

            if (Error != null)
                Error(this, new ErrorEventArgs(ex));
        }


        protected void Log(string format, params object[] args)
        {
            Trace.WriteLine(string.Format(format, args), GetType().Name);
        }
    }
}