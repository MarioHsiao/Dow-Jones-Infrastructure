@using DowJones.Documentation.Website.Extensions

In addition to the Visual Studio Project and Item Templates, the Dow Jones Framework also provides a NuGet package that contains everything you need to convert a new ASP.NET MVC web application into a "Dow Jones ASP.NET MVC Web Application".

To use this package, simply use the NuGet Package manager to install the `DowJones.Web.Mvc.Website` package:

@Html.Note("Be sure to select the <em>'All'</em> package source")

<div class="nuget-badge">
    <p>
        <code>PM&gt; Install-Package DowJones.Web.Mvc.Website</code>
	</p>
</div>

<img src="~/Content/images/InstallingWebsiteTemplate.png" />

@Html.Caution("This package <em>will</em> overwrite files in your project so it is best to apply this package to a brand new ASP.NET MVC website.")

This package will retrieve all of the dependencies that the Dow Jones ASP.NET MVC Framework requires as well as update your website files (such as `Global.asax` and `web.config`), configuring them to the Framework's needs.

Specifically, it makes the following (potentially destructive) changes to your ASP.NET MVC web application:

- Modifies the web application base class by replacing `Global.asax.cs` with a partial class combined with generated code
- Overwrites `_Layout.cshtml` with a template that includes the 
- Changes the default base type of your Views with `DowJones.Web.Mvc.UI.WebViewPage` 
- Registers the Dow Jones Client Resource Handler in the site's `web.config`
- Includes the Dow Jones extension methods by adding their namespaces to the `~/Views/web.config`

In addition to modifying existing application files, the package also adds several new ones:

- `Dependencies.cs`; a class containing the (Ninject) Dependency Injection contract-to-dependency mappings
- Several "bootstrapper task" files containing code that gets executed when the application starts
- The `HomeController` example of a Dow Jones controller
- The `~/Views/Home/Index.cshtml` view; an example of a view that uses Dow Jones Framework functionality


Note that the Dow Jones ASP.NET MVC Website package is merely a bundle containing the dependencies and configuration settings that the Dow Jones Framework requires in order to work.
Its purpose is to get your project up and running quickly, not affect how you build your application.
So, after the package has been installed, feel free to modify the project as you wish.