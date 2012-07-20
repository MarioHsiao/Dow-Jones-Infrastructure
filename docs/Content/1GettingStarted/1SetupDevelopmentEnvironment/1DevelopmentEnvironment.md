Before you can begin consuming or extending the Dow Jones UI Framework, you will need to configure your system appropriately.

### ASP.NET MVC Development
In order to work with the ASP.NET MVC Dow Jones UI Framework, you will must install the following components:

* Microsoft Visual Studio 2010 Professional Service Pack 1 (or higher -- Visual Studio 2012 works, too)
* The .NET 4.0 Framework 
* [ASP.NET MVC 3](http://www.asp.net/mvc)
* [NuGet Package Manager](http://nuget.org/) Visual Studio extension (the latest version, at least 2.0)
* The [Team Foundation Server Power Tools](http://msdn.microsoft.com/en-us/vstudio/bb980963)
* The [Razor View Component Class Generator](file://sbkntsfap05.dowjones.net/client_share/DJ%20Infrastructure/Tools/RazorViewComponentClassGenerator.vsix) (Only required for *creating* Components, not consuming them)

#### Optional Components
There are also several other tools that are very helpful and recommended, but not required in order to work with the Framework:

* Dow Jones FCS and RTS components
* [Dow Jones Visual Studio Item and Project templates](file://sbkntsfap05.dowjones.net/client_share/DJ%20Infrastructure/Tools)
* [Visual Studio Productivity Power Tools](http://visualstudiogallery.msdn.microsoft.com/d0d33361-18e2-46c0-8ff2-4adea1e34fef/) 
* [jetBrains ReSharper](http://www.jetbrains.com/resharper/), an invaluable developer productivity tool 
* [TestDriven.NET](http://testdriven.net/), an integrated unit test runner

#### Verifying Installed Components
You may verify which components you have installed -- and their corresponding versions -- by choosing the `Help > About Microsoft Visual Studio` menu option within Visual Studio.

<img src="@Url.Content("~/Content/images/VSVersions.png")" alt="Development Environment Versions">



### Client-Side (JavaScript) Development
While the ASP.NET MVC portion of the Dow Jones UI Framework requires the Microsoft Visual Studio development environment,
consuming the Client-Side (JavaScript) portion of the Framework requires nothing more than a link to the Dow Jones Component hosting site and the text editor of your choice.