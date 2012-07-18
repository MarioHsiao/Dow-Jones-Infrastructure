@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

To start using components in your ASP.NET MVC project, add reference to latest `DowJones.Web.Mvc.UI.Components` NuGet package from Dow Jones NuGet Repo.

If you haven't configured Dow Jones NuGet repo in Visual Studio, checkout [Package Management](PackageManagement) for step-by-step instructions on how to do so.

There are 3 basic steps to get this going:

* **Adding References:** Add the necessary `using` statements to the C# code. 

~~~~
	using DowJones.Web.Mvc.UI.Components.Common.Types;
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

* **Render the View:** Render the model in your view.

~~~~
	@@Html.DJ().Render(Model) 
~~~~