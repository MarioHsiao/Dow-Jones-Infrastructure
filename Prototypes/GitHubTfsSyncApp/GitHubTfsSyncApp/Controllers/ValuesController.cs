using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using GitHubTfsSyncApp.Models.GitHub;

namespace GitHubTfsSyncApp.Controllers
{
	public class NewCommitController : ApiController
	{

		public void Post([FromBody]WebHookResponse response)
		{
			// TODO: should log or do some other error handling. For demo, return will suffice.
			if(response == null)
				return;

			var worker = new TfsSyncWorker();

			new Task(() => worker.Process(response.Commits, response.Repository)).Start();
		}

	}

	public class TfsSyncWorker
	{
		public void Process(IEnumerable<Commit> commits, Repository repository)
		{
			EnsureRepositoryIsQueryAble(repository);
		}

		private void EnsureRepositoryIsQueryAble(Repository repository)
		{
			if(repository == null)
				throw new ArgumentNullException("repository", "repository object cannot be null");


		}
	}
}