@using DowJones.Documentation.Website.Extensions;

Dow Jones uses [Team Foundation Server](http://msdn.microsoft.com/en-us/vstudio/ff637362) as application lifecycle management and version control solution.

To connect to Dow Jones Team Foundation Server, go to `TEAM -> Connect to Team Foundation Server...` and click `Servers` to bring up the "Add Server" dialog.
Enter _sbknwstfs1_ as the server name and leave the rest as defaults. 

<img src="@Url.Content("~/Content/images/AddingDowJonesTFSServer.png")" alt="Adding Dow Jones Team Foundation Server">

Dismiss the dialog by clicking Ok and select the relevant project that you wish to connect to:

Select the team project(s) that you would like to work with and click `Connect` to return to Visual Studio:

<img src="@Url.Content("~/Content/images/ConnectingToTFS.png")" alt="Connecting to Team Foundation Server">

@Html.Note("Depending on your access rights, you will see a list of project(s) here. If you do not see any, you may have to contact the team project’s administrator(s) to get access first.")