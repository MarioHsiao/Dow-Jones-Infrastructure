using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI
{
    public class ClientState
    {

        public string ClientID { get; set; }

        public object Data { get; set; }

        public IDictionary<string, string> EventHandlers { get; set; }

        public IDictionary<string, object> Options { get; set; }

        public IDictionary<string, string> Tokens { get; set; }


        public ClientState()
        {
            EventHandlers = new Dictionary<string, string>();
            Options = new Dictionary<string, object>();
            Tokens = new Dictionary<string, string>();
        }

    }
}
