Build automation is the act of scripting or automating a wide variety of tasks that software developers do in their day-to-day activities such as compiling a solution, executing automated tests, generating distributable packages, and deploying a project to various environments.
These types of scripts may be executed manually on a developer's local machine, but they become much more valuable when they are automated to execute on a regular basis (preferably on a machine other than the developer's local machine; e.g. a centralized build server).

Team Foundation Server includes a build server component that is able to execute automated builds in a variety of ways.
TFS build scripts are sometimes written using the [MSBuild specification](http://msdn.microsoft.com/en-us/library/0k6kkbsd.aspx) to perform more advanced tasks, however the typical scenario involves pointing TFS to a Visual Studio solution to have it compile all of the projects within.
This makes it quite easy to add a centralized automated build to any solution.


*[TFS]: Team Foundation Server 
*[QA]:  Quality Assurance