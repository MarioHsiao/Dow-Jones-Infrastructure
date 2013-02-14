using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using GitHubTfsSyncApp.Helpers;
using GitHubTfsSyncApp.Models.GitHub;
using GitHubTfsSyncApp.Workers;
using Newtonsoft.Json;
using log4net;

namespace GitHubTfsSyncApp.Controllers
{
	public class NewCommitController : ApiController
	{
		private readonly ILog _log = LogManager.GetLogger(typeof(NewCommitController));


		public void Post(FormDataCollection formData)
		{
			var payload = formData.Get("payload");
			// TODO: should log or do some other error handling. For demo, return will suffice.
			if (payload == null)
				return;

			var resp = JsonConvert.DeserializeObject<WebHookResponse>(payload);
			var ser = new JsonNetSerializer();

			_log.Debug(payload);
			_log.Debug(ser.Serialize(resp));

			// TODO: Use IoC.
			//var worker = new TfsSyncWorker();

			//new Task(() => worker.Process(response.Commits, response.Repository)).Start();
		}
	}
}