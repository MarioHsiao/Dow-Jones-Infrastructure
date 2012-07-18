To start using components in your ASP.NET MVC project, add reference to latest `DowJones.Web.Mvc.UI.Components` NuGet package from Dow Jones NuGet Repo.

If you haven't configured Dow Jones NuGet repo in Visual Studio, checkout [Package Management](PackageManagement) for step-by-step instructions on how to do so.

There are 3 basic steps to get this going:

* **Adding References:** Add the necessary `using` statements to the c# code. 

~~~~
	using DowJones.Web.Mvc.UI.Components.Common.Types;
	using DowJones.Web.Mvc.UI.Components.Models;
~~~~ 

* **Populate the Model:** Fill in data server side if available and then populate the components model.

@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

~~~~
	// Instantiate and populate data
	var data = new <name_of_component_data_type>
	{
		// populate data
	}

	var model = new <name_Of_Component>Model
	{
		// populate properties including data
		Data = data
	};
~~~~

* **Render the View:** Render the model in your view which will render the component in the browser.

~~~~
	<!-- Render the component -->
	@@model DowJones.Web.Mvc.UI.Components.Models.<name_Of_Component>Model
	@@Html.DJ().Render(Model) 
~~~~