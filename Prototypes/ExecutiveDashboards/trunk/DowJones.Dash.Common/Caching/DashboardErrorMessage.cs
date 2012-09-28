using System;

namespace DowJones.Dash.Caching
{
    public class DashboardErrorMessage : DashboardMessage
    {
        public string Error { get; private set; }

        public DashboardErrorMessage(string eventName, string source, Exception exception = null)
            : base(eventName, source, null)
        {
            Error = (exception == null) ? "General Error" : exception.Message;
        }
    }
}