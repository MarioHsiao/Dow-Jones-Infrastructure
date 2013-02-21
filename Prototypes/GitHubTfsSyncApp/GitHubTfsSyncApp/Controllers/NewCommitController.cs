using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using GitHubTfsSyncApp.Helpers;
using GitHubTfsSyncApp.Models.GitHub;
using GitHubTfsSyncApp.Workers;
using log4net;

namespace GitHubTfsSyncApp.Controllers
{
	public class NewCommitController : ApiController
	{
		private readonly ILog _log = LogManager.GetLogger(typeof(NewCommitController));


		public void Post(WebHookResponse response)
		{
			var ser = new JsonNetSerializer();
			// TODO: should log or do some other error handling. For demo, return will suffice.
			if (!ModelState.IsValid)
			{
				_log.Error("ModelState not valid. Dump begins:\n");
				_log.Error(ser.Serialize(ModelState));
				return;
			}

			_log.Debug(ser.Serialize(response));

			//var localWorkspace = HostingEnvironment.MapPath("~\\StagingArea");
			var localWorkspace = ConfigurationManager.AppSettings.Get("TFS:LocalWorkspace");

			// TODO: Use IoC.
			var worker = new TfsSyncWorker(
									ConfigurationManager.AppSettings.Get("TFS:Url"),
									ConfigurationManager.AppSettings.Get("TFS:Project"),
									localWorkspace,
									new NetworkCredential(ConfigurationManager.AppSettings.Get("TFS:Username"), ConfigurationManager.AppSettings.Get("TFS:Password")));

			new Task(() => worker.Process(response.Commits, response.Repository)).Start();
		}
	}
}