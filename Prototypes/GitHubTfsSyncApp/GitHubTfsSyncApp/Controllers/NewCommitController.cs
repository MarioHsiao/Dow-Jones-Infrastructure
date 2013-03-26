using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Mvc;
using GitHubTfsSyncApp.Configuration;
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
           
            ProjectDetails projDetails = GetGithubTfsConfig(response.Repository);
            if (projDetails == null)
            {
                throw new Exception("TFS Project is not defined. Please define the mapping between Github and TFS Project");
            }
            // TODO: Use IoC.
            var worker = new TfsSyncWorker(
                                    projDetails.TfsUrl,
                                    projDetails.TfsProjectName,
                                    projDetails.TfsLocalWorkspace,
                                    GitHubAccessConfigurationGenerator.CreateFromWebConfig(projDetails.GitHubCredentials,
                                    projDetails.GitHubClientId,
                                    projDetails.GitHubClientSecret,
                                    ConfigurationManager.AppSettings.Get("GitHub:ApiEndPoint")),
                                    projDetails.Filters,
                                    new NetworkCredential(projDetails.TfsUserName, projDetails.TfsPassword)
                                  ); 

            worker.Process(response.Commits, response.Repository);
            //new Task(() => worker.Process(response.Commits, response.Repository)).Start();
        }

        private ProjectDetails GetGithubTfsConfig(Repository repository)
        {
            if (Config.GithubTFSConfigSectionInstance.Projects != null)
            {
                IEnumerator iEnumerator = Config.GithubTFSConfigSectionInstance.Projects.GetEnumerator();
                while (iEnumerator.MoveNext())
                {
                    if (((ProjectDetails)iEnumerator.Current).GitHubProjectName.Equals(repository.Name,
                                                                                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        return ((ProjectDetails)iEnumerator.Current);
                    }
                }
            }
            return null;
        }
    }
}