using System.Threading;
using System.Threading.Tasks;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;

namespace DowJones.Dash.Website.DataSources
{
    public class TopPages : DataSource
    {
        public TopPages()
        {
            Task.Factory.StartNew(Poll);
        }

        protected internal void Poll()
        {
            while(true)
            {
                var data = GetData();
                OnDataReceived(data);
                Thread.Sleep(1000);
            }
        }

        private object GetData()
        {
            PortalHeadlineInfo[] headlines = new[]
                {
                    new PortalHeadlineInfo { Title = "Top Page 1", HeadlineUrl = "http://www.google.com", ModificationTimeDescriptor = "55555" },
                    new PortalHeadlineInfo { Title = "Top Page 2", HeadlineUrl = "http://www.google.com", ModificationTimeDescriptor = "44444" },
                    new PortalHeadlineInfo { Title = "Top Page 3", HeadlineUrl = "http://www.google.com", ModificationTimeDescriptor = "33333" },
                    new PortalHeadlineInfo { Title = "Top Page 4", HeadlineUrl = "http://www.google.com", ModificationTimeDescriptor = "22222" },
                    new PortalHeadlineInfo { Title = "Top Page 5", HeadlineUrl = "http://www.google.com", ModificationTimeDescriptor = "11111" },
                };

            return new PortalHeadlineListModel { Result = new PortalHeadlineListDataResult(new PortalHeadlineListResultSet(headlines)) };
        }
    }
}