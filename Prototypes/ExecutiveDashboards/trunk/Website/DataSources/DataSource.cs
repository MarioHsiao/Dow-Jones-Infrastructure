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
        string Name { get; }
        event EventHandler<DataReceivedEventArgs> DataReceived;
    }

    public abstract class DataSource : IDataSource
    {
        public virtual string Name
        {
            get { return GetType().Name; }
        }

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        protected bool Enabled
        {
            get { return DataReceived != null; }
        }

        protected void OnDataReceived(object data)
        {
            if(DataReceived != null && data != null)
                DataReceived(this, new DataReceivedEventArgs(data));
        }
    }
}