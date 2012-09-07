using System;

namespace DowJones.Dash.Website.DataSources
{
    public class DataReceivedEvent : EventArgs
    {
        public object Data { get; private set; }

        public DataReceivedEvent(object data)
        {
            Data = data;
        }
    }

    public interface IDataSource
    {
        event EventHandler<DataReceivedEvent> DataReceived;
    }

    public abstract class DataSource : IDataSource
    {
        public event EventHandler<DataReceivedEvent> DataReceived;

        protected bool Enabled
        {
            get { return DataReceived != null; }
        }

        protected void OnDataReceived(object data)
        {
            if(DataReceived != null && data != null)
                DataReceived(this, new DataReceivedEvent(data));
        }
    }
}