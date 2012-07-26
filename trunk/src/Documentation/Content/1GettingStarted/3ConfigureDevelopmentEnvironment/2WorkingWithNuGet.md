@using DowJones.Documentation.Website.Extensions

Dow Jones projects leverage NuGet packages to manage external dependencies. 
In addition to the public NuGet package repository, the Dow Jones Platform Integration Team hosts several custom NuGet repositories which you need to add to your NuGet configuration in Visual Studio:

#### Adding the Dow Jones NuGet Package Source
Before you can consume the custom Dow Jones NuGet packages, you must first tell Visual Studio and NuGet where to find them by following the steps below:

First, open the NuGet settings dialog within Visual Studio by selecting the `Tools > Library Package Manager > Package Manager Settings` menu option:

<img src="@Url.Content("~/Content/images/NuGet_PackageSources.png")" alt="NuGet Package Sources">

Then, populate the dialog with the following values:

* *Name*: Dow Jones package source
* *Source*: [&#x5c;&#x5c;fsegdevw1.win.dowjones.net\WebSites\nuget\main\Packages](file:///fsegdevw1.win.dowjones.net\WebSites\nuget\main\Packages)

@Html.Note("If you are unable to connect to the package source above or it performs poorly, replace it with <em>http://montana.dev.us.factiva.com/nuget/main/repository</em>")

Finally, click the `Add` button to add the Package Source, then close the dialog.

If all has gone well, you should have the Dow Jones Package Sources listed in your Available package sources list.
