using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using GitHubTfsSyncApp.Helpers;
using GitHubTfsSyncApp.Models.GitHub;
using GitHubTfsSyncApp.Workers;
using log4net;

namespace GitHubTfsSyncApp.Controllers
{
	public class NewCommitController : ApiController
	{
		private readonly ILog _log = LogManager.GetLogger(typeof(NewCommitController));

		
		public void Post([FromBody]WebHookResponse response)
		{
			// TODO: should log or do some other error handling. For demo, return will suffice.
			if(response == null)
				return;

			var ser = new JsonNetSerializer();

			_log.Debug(ser.Serialize(response));

			// TODO: Use IoC.
			//var worker = new TfsSyncWorker();

			//new Task(() => worker.Process(response.Commits, response.Repository)).Start();
		}
	}
}