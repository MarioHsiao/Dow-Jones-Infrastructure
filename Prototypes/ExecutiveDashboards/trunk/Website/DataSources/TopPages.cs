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
                Thread.Sleep(2000);
            }
        }

        private object GetData()
        {
            try
            {
                var request = WebRequest.Create("http://api.chartbeat.com/toppages/?host=online.wsj.com&limit=5&apikey=9afde3c7a533572878d27e80cf001244");
                using(var response = request.GetResponse())
                using(var stream = new StreamReader(response.GetResponseStream()))
                {
                    var source = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(stream.ReadToEnd());
                    var pages =
                        from page in source
                        select new PortalHeadlineInfo
                            {
                                Title = page.i,
                                HeadlineUrl = "http://" + page.path,
                                ModificationTimeDescriptor = page.visitors,
                            };

                    return new PortalHeadlineListModel { Result = new PortalHeadlineListDataResult(new PortalHeadlineListResultSet(pages)) };
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("TopPages request failed: {0}", ex);
                return null;
            }
        }
    }
}