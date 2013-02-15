using System.Web.Http;
using GitHubTfsSyncApp.Helpers;
using GitHubTfsSyncApp.Models.GitHub;
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

			// TODO: Use IoC.
			//var worker = new TfsSyncWorker();

			//new Task(() => worker.Process(response.Commits, response.Repository)).Start();
		}
	}
}