@using DowJones.Documentation.Website.Extensions

In addition to the Visual Studio Project and Item Templates, the Dow Jones Framework also provides a NuGet package that contains everything you need to convert a new ASP.NET MVC web application into a "Dow Jones ASP.NET MVC Web Application".

To use this package, simply use the NuGet Package manager to install the `DowJones.Web.Mvc.Website` package:

<div class="nuget-badge">
    <p>
        <code>PM&gt; Install-Package DowJones.Web.Mvc.Website</code>
	</p>
</div>

This package will retrieve all of the dependencies that the Dow Jones ASP.NET MVC Framework requires as well as update your website files (such as `Global.asax` and `web.config`), configuring them to the Framework's needs.

@Html.Caution("This package <em>will</em> overwrite files in your project so it is best to apply this package to a brand new ASP.NET MVC website.")