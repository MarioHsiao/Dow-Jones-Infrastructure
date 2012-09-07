using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using Newtonsoft.Json;

namespace DowJones.Dash.Website.DataSources
{
    public class TopPages : DataSource
    {
        public int ErrorDelaySeconds { get; set; }
        public int PollDelaySeconds { get; set; }

        public TopPages() : this(2)
        {
        }

        public TopPages(int pollDelay, int? errorDelay = null)
        {
            PollDelaySeconds = pollDelay;
            ErrorDelaySeconds = errorDelay.GetValueOrDefault(pollDelay * 2);

            Task.Factory.StartNew(Poll);
        }

        protected internal void Poll()
        {
            try
            {
                WebRequest
                    .Create("http://api.chartbeat.com/toppages/?host=online.wsj.com&limit=5&apikey=9afde3c7a533572878d27e80cf001244")
                    .GetResponseAsync()
                    .ContinueWith(task => OnResponse(task.Result));
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private void OnResponse(WebResponse response)
        {
            try
            {
                using(var stream = response.GetResponseStream())
                {
                    var pages = Deserialize(stream);
                    OnDataReceived(pages);
                }

                Thread.Sleep(PollDelaySeconds * 1000);
                Poll();
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private void OnError(Exception ex = null)
        {
            Trace.TraceError("TopPages request failed: {0}", ex);
            Thread.Sleep(ErrorDelaySeconds * 1000);
            Poll();
        }

        private static PortalHeadlineListModel Deserialize(Stream stream)
        {
            var json = new StreamReader(stream).ReadToEnd();
            var source = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(json);
            var pages =
                from page in source
                select new PortalHeadlineInfo
                    {
                        Title = page.i,
                        HeadlineUrl = "http://" + page.path,
                        ModificationTimeDescriptor = page.visitors,
                    };

            return new PortalHeadlineListModel
                {Result = new PortalHeadlineListDataResult(new PortalHeadlineListResultSet(pages))};
        }
    }
}