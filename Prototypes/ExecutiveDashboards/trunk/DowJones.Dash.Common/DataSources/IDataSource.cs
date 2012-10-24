using System;

namespace DowJones.Dash.Common.DataSources
{
    public interface IDataSource
    {
        event EventHandler<DataSourceEvent.DataReceived> DataReceived;
        event EventHandler<DataSourceEvent.Error> Error;
        string Name { get; }
        void Start();
        void Suspend();
    }
}