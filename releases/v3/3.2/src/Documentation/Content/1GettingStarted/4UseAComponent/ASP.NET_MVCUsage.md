@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

To start using components in your ASP.NET MVC project, add reference to latest `DowJones.Web.Mvc.UI.Components` NuGet package from Dow Jones NuGet Repo.

If you haven't configured Dow Jones NuGet repo in Visual Studio, checkout [Package Management](PackageManagement) for step-by-step instructions on how to do so.

There are 4 basic steps to get this going:

* **Adding References:** Add the necessary `using` statements to the C# code. 

~~~~
using DowJones.Web.Mvc.UI.Components.Common;
using DowJones.Web.Mvc.UI.Components.[Component];
~~~~ 

* **Populate the Model:** Then, create and populate an instance of the component's model.

~~~~
// Instantiate and populate data
var data = new [Component Data Type]()
{
	// populate data
}

var model = new [Component]Model
{
	// populate properties including data
	Data = data
};

return View(model);
~~~~

* **Update \_Layout.cshtml (master page):** to render `ScriptRegistry` and `StylesheetRegistry`: 

~~~~
<head>
	<!-- other page related stuff -->
	
	<!-- Render startup scripts and css (such as modernizr.js or reset.css if the page uses them) -->
    @@DJ.ScriptRegistry().RenderHead()
    @@DJ.StylesheetRegistry().RenderHead()
</head>
<body>
	@@RenderBody()

	<!-- Render non-startup scripts and css -->
	@@DJ.StylesheetRegistry().Render() 
    @@DJ.ScriptRegistry().Render()
</body>
~~~~

* **Render the View:** Render the model in your view.

~~~~
@@Html.DJ().Render(Model) 
~~~~