using System;
using System.Net.Http;
using System.Threading.Tasks;
using log4net;
using Nancy;

namespace DowJones.SyntaxHost.Modules
{
    public class IndexModule : NancyModule
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof (IndexModule));

        public IndexModule()
        {
            Before += async (ctx, ct) =>
                            {
                                AddToLog("Before Hook Delay\n");
                                await Task.Delay(500);

                                return null;
                            };

            After += async (ctx, ct) =>
                           {
                               AddToLog("After Hook Delay\n");
                               await Task.Delay(500);
                               AddToLog("After Hook Complete\n");
                               //ctx.Response += GetLog();
                           };

            Get["/", true] = async (x, ct) =>
                                   {
                                       AddToLog("Delay 1\n");
                                       await Task.Delay(500);

                                       AddToLog("Delay 2\n");
                                       await Task.Delay(500);

                                       AddToLog("Executing async http client\n");
                                       var client = new HttpClient();
                                       HttpResponseMessage res = await client.GetAsync("http://nancyfx.org");
                                       string content = await res.Content.ReadAsStringAsync();
                                       AddToLog("Response: " + content.Split('\n')[0] + "\n");

                                       //return (Response) GetLog();
                                       return View["index"];
                                   };

            //Get["/"] = parameters => View["index"];
        }

        public void AddToLog(string logLine)
        {
            _logger.Info(logLine);

            if (!Context.Items.ContainsKey("Log"))
            {
                Context.Items["Log"] = string.Empty;
            }

            Context.Items["Log"] = (string) Context.Items["Log"] + DateTime.Now.ToLongTimeString() + " : " + logLine;
        }

        public string GetLog()
        {
            if (!Context.Items.ContainsKey("Log"))
            {
                Context.Items["Log"] = string.Empty;
            }

            return (string) Context.Items["Log"];
        }
    }
}