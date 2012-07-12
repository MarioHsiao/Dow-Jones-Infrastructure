Before you can start adding the custom Dow Jones NuGet packages, you must first tell Visual Studio and NuGet where to find them by following the steps below:

#### Open the NuGet Library Package Manager Settings
In order to add NuGet packages, their package source must be listed in your Library Package Manager sources list, which can be found under `Tools > Library Package Manager > Package Manager Settings`:

<img src="@Url.Content("~/Content/images/NuGet_Navigation.png")" alt="Nuget Navigation">

This will open the Package Manager Settings dialog below:

<img src="@Url.Content("~/Content/images/NuGet_PackageSources.png")" alt="NuGet Package Sources">

Finally, for each source shown in [Dow Jones Nuget Repos](#DowJonesNugetRepos), populate the Name and Source boxes and click the `Add` button:

If all has gone well, you should have the Dow Jones Package Sources listed in your Available package sources list.
