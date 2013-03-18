using System.IO;
using System.Net;
using GitHubTfsSyncApp.Controllers;
using GitHubTfsSyncApp.Helpers;
using GitHubTfsSyncApp.Models.GitHub;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GitHubTfsSyncApp.Tests
{
	[TestClass]
	public class SerializerTests
	{
		[TestMethod]
		public void CanDeserializeWebHookResponse()
		{
			const string sampleResponse = "{ \"after\": \"9a272ff0768f0179acc292c7c2ffbe5f2d04b320\", \"before\": \"fb0f2c12540a559a948afc54eeb9b661cff5333c\", \"commits\": [ { \"added\": [ \"CheckinTest.md\" ], \"author\": { \"email\": \"hrusikesh.panda@dowjones.com\", \"name\": \"pandah\", \"username\": \"pandah\" }, \"committer\": { \"email\": \"hrusikesh.panda@dowjones.com\", \"name\": \"pandah\", \"username\": \"pandah\" }, \"distinct\": true, \"id\": \"69d262f60a946c426e4ece7bd25f9235fbc30af0\", \"message\": \"Create CheckinTest.md\", \"modified\": [], \"removed\": [], \"timestamp\": \"2013-02-14T21:29:44+00:00\", \"url\": \"https://github.dowjones.net/pandah/TfsSyncDemo/commit/69d262f60a946c426e4ece7bd25f9235fbc30af0\" }, { \"added\": [], \"author\": { \"email\": \"hrusikesh.panda@dowjones.com\", \"name\": \"pandah\", \"username\": \"pandah\" }, \"committer\": { \"email\": \"hrusikesh.panda@dowjones.com\", \"name\": \"pandah\", \"username\": \"pandah\" }, \"distinct\": true, \"id\": \"9a272ff0768f0179acc292c7c2ffbe5f2d04b320\", \"message\": \"Update CheckinTest.md\", \"modified\": [ \"CheckinTest.md\" ], \"removed\": [], \"timestamp\": \"2013-02-14T21:30:24+00:00\", \"url\": \"https://github.dowjones.net/pandah/TfsSyncDemo/commit/9a272ff0768f0179acc292c7c2ffbe5f2d04b320\" } ], \"compare\": \"https://github.dowjones.net/pandah/TfsSyncDemo/compare/fb0f2c12540a...9a272ff0768f\", \"created\": false, \"deleted\": false, \"forced\": false, \"head_commit\": { \"added\": [], \"author\": { \"email\": \"hrusikesh.panda@dowjones.com\", \"name\": \"pandah\", \"username\": \"pandah\" }, \"committer\": { \"email\": \"hrusikesh.panda@dowjones.com\", \"name\": \"pandah\", \"username\": \"pandah\" }, \"distinct\": true, \"id\": \"9a272ff0768f0179acc292c7c2ffbe5f2d04b320\", \"message\": \"Update CheckinTest.md\", \"modified\": [ \"CheckinTest.md\" ], \"removed\": [], \"timestamp\": \"2013-02-14T21:30:24+00:00\", \"url\": \"https://github.dowjones.net/pandah/TfsSyncDemo/commit/9a272ff0768f0179acc292c7c2ffbe5f2d04b320\" }, \"hook_callpath\": \"new\", \"pusher\": { \"name\": \"none\" }, \"ref\": \"refs/heads/master\", \"repository\": { \"created_at\": \"2013-02-14T15:54:52+00:00\", \"description\": \"Demo Repository to show how Github TFS bridge will work.\", \"fork\": false, \"forks\": 0, \"has_downloads\": true, \"has_issues\": true, \"has_wiki\": true, \"id\": 1148, \"name\": \"TfsSyncDemo\", \"open_issues\": 0, \"owner\": { \"email\": \"hrusikesh.panda@dowjones.com\", \"name\": \"pandah\" }, \"private\": false, \"pushed_at\": \"2013-02-14T21:30:26+00:00\", \"size\": 152, \"stargazers\": 0, \"url\": \"https://github.dowjones.net/pandah/TfsSyncDemo\", \"watchers\": 0 } }";

			var serializerSettings = new JsonSerializerSettings
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver(),
				};
			var jsonDeserializer = JsonSerializer.Create(serializerSettings);

			var deserializedResponse = jsonDeserializer.Deserialize<WebHookResponse>(new JsonTextReader(new StringReader(sampleResponse)));
			Assert.AreEqual(deserializedResponse.After, "9a272ff0768f0179acc292c7c2ffbe5f2d04b320");

		    var gitHubAccessConfiguration = new GitHubAccessConfiguration
		        {
		            ClientId = Settings1.Default.GitHubClientId,
		            Credentials = Settings1.Default.GitHubCredentials,
		            ClientSecret = Settings1.Default.GitHubClientSecret,
		            EndPoint = Settings1.Default.GitHubApiEndPoint
		        };


            //var config = new TfsAccessConfiguration
            //    {
            //        Url = Settings1.Default.TFSUrl,
            //        Project = Settings1.Default.TFSProject,
            //        LocalWorkspace = Settings1.Default.TFSLocalWorkspace,
            //        Credentials =
            //            new NetworkCredential()
            //                {
            //                    UserName = Settings1.Default.TFSUsername,
            //                    Password = Settings1.Default.TFSPassword
            //                }
            //    };
            //var controller = new NewCommitController();
           // controller.Post(deserializedResponse, config, gitHubAccessConfiguration);
		}
	}
}
