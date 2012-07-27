﻿#### PREFER Custom Route Attributes

Too many of these custom routing patterns added to the global routing table (`RouteTable.Routes`) can quickly become difficult to manage and troubleshoot. 
When you want to provide a custom route to a single action, apply the `RouteAttribute` directly to your action.

#### ALWAYS Use the Script Registry and Stylesheet Registry instead of direct includes (even for snippets)

The Dow Jones MVC Framework introduces the Script Registry and Stylesheet Registry concepts to better manage script and style includes.
These Registries provide you with a central location to manage these resources, as well as 'behind the scenes' optimizations such as script combining, minifying, etc. 
To best take advantage of these optimizations, leverage these Registries as opposed to implementing these things yourself.

#### PREFER Presentation Models Over Direct Usage of business objects

In general, try to avoid allowing changes to the business model to directly affect the view. `Presentation Models` help with this.

#### PREFER Bootstrapper tasks over logic placed in Application\_Start (Global.asax)

Most ASP.NET MVC demos will advise modifying the `Application_Start` method in the *Global.asax.cs* file in order to introduce logic that will execute when the application starts. 
While this is certainly the easiest and most straight-forward approach, the Dow Jones MVC Framework provides the concept of "bootstrapper tasks". 
These tasks are easy to implement (simply implement the `IBootstrapperTask` interface), are automatically discovered (merely by existing in referenced assembly) and are executed during the `Application_Start` phase of the application.
These Tasks help provide cleaner code and encourage proper adherence of the Single Responsibility principle.

#### PREFER explicit View names

A majority of the ASP.NET MVC controller action code samples call the `View()` method without specifying a view name.
This is suitable for simple demo code, however when tests or other action methods begin calling each other, the detriments to this approach become clear. 
When no view name is specified, the ASP.NET MVC framework defaults to the name of *the action that was originally called*.