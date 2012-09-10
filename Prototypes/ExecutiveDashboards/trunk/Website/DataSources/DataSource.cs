using System;

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

    public interface IDataSource
    {
        event EventHandler<DataReceivedEventArgs> DataReceived;
        string Name { get; }
        void Start();
    }

    public abstract class DataSource : IDataSource
    {
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        public virtual string Name
        {
            get { return GetType().Name; }
        }

        protected bool Enabled
        {
            get { return DataReceived != null; }
        }

        public abstract void Start();

        protected void OnDataReceived(object data)
        {
            if(DataReceived != null && data != null)
                DataReceived(this, new DataReceivedEventArgs(data));
        }
    }
}