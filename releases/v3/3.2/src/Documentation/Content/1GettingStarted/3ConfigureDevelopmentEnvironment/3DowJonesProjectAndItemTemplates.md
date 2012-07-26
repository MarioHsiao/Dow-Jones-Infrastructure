The Dow Jones UI Framework uses a number of conventions for creating components.
to help jump-start development of new components and websites, we've created several custom Visual Studio Project and Item Templates.

In order to use these templates, simply execute <a href="file:///sbkntsfap05.dowjones.net/client_share/DJ Infrastructure/Tools/Dow Jones Templates.vsi">the Project and Item Templates installer</a> to add the templates to Visual Studio.

Then to create a new project or item, simply select the template from the `Add New Item...` wizard:

<img src="@Url.Content("~/Content/images/DowJonesItemTemplates.png")" alt="Dow Jones Item Templates">

#### General-Purpose Item Templates
The Dow Jones UI Framework adds 3 general-purpose custom Item Templates:

1. **Dow Jones Bootstrapper Task**: Creates a C# file with a placeholder for code that will execute when the application starts.

2. **Dow Jones MVC View Component**: Creates the artifacts required for a Razor-based Dow Jones View Component (aka "UI Component")
	* `[Name].cshtml`: A Razor file containing the view markup that generates the service-side logic to render the component HTML
	* `[Name]Model.cs`: A C# file containing the model for the component
	* `[Name].js`: A JavaScript file containing the client-side (JavaScript) logic for the component

3. **Dow Jones MVC View Component (Razor Only)**: Used for "simple" UI components that require only HTML markup (no backing model or client-side logic).  This template is the same as the View Comopnent template, only without the Razor View and JavaScript.


#### Dashboard-Specific Item Templates
The UI Framework also adds 2 custom Item Templates specific to Dashboards development:

1. **Canvas**: Creates the artifacts required for a Dow Jones Dashboard container (aka "Canvas")
	* `[Name].cs`: A C# file containing the server-side logic to render the Dashboard HTML
	* `[Name]Model.cs`: A C# file containing the model for the Dashboard
	* `[Name].js`: A JavaScript file containing the client-side (JavaScript) logic for the Dashboard

2. **Dow Jones MVC Canvas Module**: Creates the artifacts required for a Dow Jones Dashboard Module (aka "Canvas Module")
	* `[Name].cshtml`: A Razor file containing the view markup that generates the service-side logic to render the Dashboard Module HTML
	* `[Name]Model.cs`: A C# file containing the model for the Dashboard Module component
	* `[Name].js`: A JavaScript file containing the client-side (JavaScript) logic for the Dashboard Module

