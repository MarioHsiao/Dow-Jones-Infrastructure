using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using GitHubTfsSyncApp.Models.GitHub;
using GitHubTfsSyncApp.Workers;

namespace GitHubTfsSyncApp.Controllers
{
	public class NewCommitController : ApiController
	{

		public void Post([FromBody]WebHookResponse response)
		{
			// TODO: should log or do some other error handling. For demo, return will suffice.
			if(response == null)
				return;

			// TODO: Use IoC.
			var worker = new TfsSyncWorker();

			new Task(() => worker.Process(response.Commits, response.Repository)).Start();
		}
	}
}