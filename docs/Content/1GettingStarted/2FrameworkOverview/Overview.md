Dow Jones has made a significant investment in the ASP.NET MVC platform, customizing the platform with several extensions and a full-fledged "view component" framework.

Most of the back end data is exposed using set of REST enabled services called API. 
Together, these become the building blocks for any web application.

<img src="@Url.Content("~/Content/images/BuildingBlocks.png")" alt="Dow Jones UI Framework: High-level Architecture">

The framework consists of the following blocks:

- Dow Jones Core
- Dow Jones MVC Framework
- UI Components
- Composite Components
- Canvas Modules
- API Services (REST, Dashboard)

Each block contains a set of libraries that are designed to eliminate common, repetitive tasks and let the developer focus on building applications on top of these base libraries. 

In the coming sections, you'll learn more about each building block and the set of services that they provide.


[[DowJonesCore]]
=== Dow Jones Core  ===
Dow Jones core provides bare bones, low-level infrastructure which acts as the foundation for all other blocks.
Just like you never see the skeletal framework or plumbing in a building (unless you dig deeper behind the walls), this provides key classes and interfaces that you'll normally wouldn't see in your web application.
Whenever you hear words like *"don't worry about it, the framework will do this magically for you"*, think of this layer!

It provides following key capabilities:

- Dependency Injection
- Session Management 
- Security Management
- Various registries such as:
	* AssemblyRegistry
	* ClientResourceRegistry
- Token management
- Serialization
- Proxy Handlers
- Client Resource Handling
- Runtime combining and minifying of client resources
- Base, Base classes 
- Loggers
- Core Extensions
- Charting
- Ad Management
- Assembler, Managers, Formatters
- Web Handlers such as 
	* Syndication
	* Video
	* Article
	* ...and more!


[[DowJonesMVCFramework]]
=== Dow Jones MVC Framework ===
Build on top of <<DowJonesCore, Dow Jones Core>>, this layer provides:

 - DowJones.Web.MVC library
 - DowJones.Web.Mvc.Search library

[[DowJonesWebMVCLibrary]]
.DowJones.Web.MVC library

This library extends the core ASP.Net MVC framework and provides the following services:

 - Session Timeout handler and redirect to login page
 - <<ScriptAndStylesheetRegistry, Script Registry>>
 - <<ScriptAndStylesheetRegistry, StyleSheet Registry>>
 - ControllerRegistry
 - Global Headers
 - HtmlHelper and ViewData extensions
 - JsonModelBinder
 - Route Generator
 - Threading and COM+ compatibility attributes
 - ClientState handling
 - <<ViewComponentBase, +ViewComponent+ and +ViewComponent+ base classes>>
 - Fluent Interfaces for component builders
 - Global JavaScript resources such as:
 	* jQuery
 	* jQuery UI
 	* DJ Common JS 
 	* ServiceProxy
 	* Modernizr
 	* Underscore
 	* PubSubManger
 	* YepNope loader (for JS dependency management)

[[DowJonesWebMvcSearchLibrary]]
.DowJones.Web.Mvc.Search library

As the name suggests, this library provides base controllers, managers, views for providing Search Results.
It provides View Models and mappers that map the domain model to its corresponding View Model.

The views can be styled with CSS and it does not provide any default styling.


[[UIComponents]]
=== UI Components ===

Home to the numerous UI Components that applications can build upon. 
It consists of the following libraries:

 - DowJones.Web.Mvc.UI.Components
 - DowJones.Web.Mvc.UI.Components.Models

UI Components are the lowest visual elements that interact with the DOM directly. 
They listen to DOM events and react to various clicks, mouse overs and other DOM events.
The DOM event handler listens to an event and then _publishes_ the event to its container with various arguments. 

The arguments usually include:
 - name of the event (along with its namespace)
 - data associated with the event (e.g. in case of a headline click, accession number of the headline)
 - any other optional attributes or data that might be needed higher up (in the container or page level handler).

When an event is published by a component, it is captured by its owner, if defined. 
If no owner is defined, it simply bubbles up to the page level.

A UI Component cannot be set as an owner of another UI component. 
Only a Composite component or its derived classes can be set as owners. 
In the coming sections, you'll see why this restriction is put in place.

Some of the notable components are:

 - HeadlineList
 - PortalHeadlineList
 - Article
 - HeadlineCarousel
 - HeadlinePostProcessing
 - RegionalMap
 - Radar
 - NewsChart
 - NavBar
 - Menu
 - CalloutPopup
 - SearchBuilder
 - and many more

Each UI Component consists of the following parts:

 - <<CustomRazorViews, Custom Razor Views>> 
 - JavaScript Plugin
 - One or more <<ClientTemplate, Client Template(s)>> (optional but recommended to have)
 - View Model (which is housed in a separate project <<DowJonesWebMvcUIComponentsModels, DowJones.Web.Mvc.UI.Components.Models>>)

The CSS is provided by application team. 
This way, the components can be styled and branded as per the end application's requirements.

[[DowJonesWebMvcUIComponentsModels]]
.DowJones.Web.Mvc.UI.Components.Models

This library houses the ViewModels for a UI component.
The +ViewModel+ provides:

 - Client Side Options (properties marked with +ClientProperty+ attribute with an optional name)
 - Client Side Data (properties marked with +ClientData+ attribute with an optional name)
 - Methods and business logic that provide a calculated value that can be used in the View

[[CompositeComponents]]
=== Composite Components ===

Composite Components are made up of two or more UI Components. 

They act as a scope container for the events fired by individual components.
Each of the composite component or its derived classes contain an instance of +PubSubManager+ which acts as a component event sink and an outlet.


An UI Component is agnostic of other components on the page. 
An UI Component also doesn't fetch its own data, rather, it is fed data by the Composite Component or its owner.


These are the primary reasons why a UI component cannot own another component.


@using DowJones.Documentation.Website.Extensions

@Html.Note("
<p>
It is worth noting that two or more composite components can talk to each other, again via PubSub model. 
</p>
<p>
Also, a composite component can have another composite component as its child.
</p>
<p>
`SearchResults` component is a classic example of this. 
It is composite component which houses 3 other composite components as well as some UI components.
</p>

[[CanvasModules]]
=== Canvas Modules ===

Canvas Modules are specialized Composite Component. 
They are special in the sense that they are aware of +Canvas+. 
In addition, they have specific data attached to them which decides their behavior on the UI (such as position).
They cannot exist outside of a Canvas on page, unlike composite components.
