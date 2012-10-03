using System;

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
}