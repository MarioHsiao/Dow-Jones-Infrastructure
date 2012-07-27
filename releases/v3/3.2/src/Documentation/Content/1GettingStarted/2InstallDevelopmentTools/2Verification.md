@using DowJones.Documentation.Website.Extensions

#### Verifying Visual Studio is installed

* Launch Visual Studio. If you cannot, you haven't installed it.
* On the top menu, goto `Help > About Microsoft Visual Studio`

Verify you have Visual Studio with **SP1** installed as shown below:

<img src="@Url.Content("~/Content/images/VerifyVSSP1.png")" alt="Verifying Visual Studio with SP1">

#### Verifying .NET 4.0 Framework is installed

* Launch Visual Studio. 
* On the top menu, goto `Help > About Microsoft Visual Studio`

Verify you have .NET 4.0 Framework or higher installed as shown below:

<img src="@Url.Content("~/Content/images/VerifyDotNetFrameWork.png")" alt="Verifying .NET 4.0 Framework">

#### Verify ASP.NET MVC 3 is installed

* Launch Visual Studio
* On the top menu, goto `File > New > Project`
* Under Installed Templates, make sure you see ASP.NET MVC 3 Web Application Template appears as shown below.

<img src="@Url.Content("~/Content/images/VerifyASPNETMVC3.png")" alt="Verifying ASP.NET MVC 3">

#### Verifying NuGet Package Manager is installed

* Launch Visual Studio. 
* On the top menu, goto `Help > About Microsoft Visual Studio`

Verify you have NuGet Package Manager installed as shown below:

<img src="@Url.Content("~/Content/images/VerifyNuGet.png")" alt="Verifying NuGet Package Manager">

@Html.Important("NuGet is updated frequently so make sure you install the <b>latest</b> version and not the version shown here in particular.")

#### Verifying Team Foundation Server Power Tools is installed

* Launch Visual Studio. 
* On the top menu, goto `Help > About Microsoft Visual Studio`

Verify you have "Team Foundation Server Power Tools" installed as shown below:

<img src="@Url.Content("~/Content/images/VerifyTFPT.png")" alt="Verifying Team Foundation Server Power Tools">

#### Verifying Razor View Component Class Generator is installed

* Launch Visual Studio. 
* On the top menu, goto `Tools > Extension Manager...`.

Verify you have "Razor View Component Class Generator" under "Installed Extensions" as shown below:

<img src="@Url.Content("~/Content/images/VerifyRazorViewComponentGenerator.png")" alt="Development Environment Versions">

#### Client-Side (JavaScript) Development

While the ASP.NET MVC portion of the Dow Jones UI Framework requires the Microsoft Visual Studio development environment,
consuming the Client-Side (JavaScript) portion of the Framework requires nothing more than a link to the Dow Jones Component hosting site and the text editor of your choice.