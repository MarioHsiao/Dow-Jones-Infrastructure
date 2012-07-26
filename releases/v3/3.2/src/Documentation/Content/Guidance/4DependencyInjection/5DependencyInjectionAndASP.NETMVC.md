Arguably the greatest benefit of the Model-View-Controller (MVC) pattern is its emphasis on loose coupling and separation of concerns. 
Since the Dependency Injection pattern shares these same fundamental principles, the two patterns are very commonly combined together to great effect, and Microsoft’s highly-extensible ASP.NET MVC framework provides excellent integration points that make these patterns very easy to implement together.

Just about every component of the ASP.NET MVC framework is interchangeable, from the way Controllers and Views are located, instantiated, and executed to the way that models are validated. 
ASP.NET MVC provides several important extensibility points relevant to dependency injection:

#### Controller Factories

By default, ASP.NET MVC Controller classes must include a default constructor. 
This requirement derives from the fact that the ASP.NET MVC framework does not have a dependency injection container built in, so the default Controller Factory implementation does not have the information required to fill the parameters of anything but a default constructor. 
One of the most important DI integration points the ASP.NET MVC provides is the ability to provide your own implementation of IControllerFactory - an implementation with the ability to leverage a particular DI container to create and initialize Controller instances.
This is achieved with a simple call to: System.Web.Mvc.ControllerBuilder.Current.SetControllerFactory([Type or instance]); Several of the popular Dependency Injection frameworks provide custom Controller Factory implementations that work with their containers. 
Luckily, implementing custom Controller Factory implementations for most frameworks that do not provide implementations is a relatively trivial task.

#### Model Binders

Model Binders are a powerful way to transform parameters from a web request to object instances, encouraging an object-oriented approach to Controller Actions (i.e. working with objects as opposed to a set of loosely-associated parameters). 
Since Model Binders (and the classes they create instances of) are fully capable of implementing Dependency Injection, it is important for the dependency injection framework to participate in this subsystem. Fortunately, ASP.NET MVC makes this easy with a simple call to: System.Web.Mvc.ModelBinders.Binders.Add([Model Type], [Model Binder]);

#### View Engines

Just as the logic implemented in Controller Actions is very important, so too is the ability to locate and render the View needed to relay the Action’s response to consumers. 
Since there are a multitude of markup languages and syntax - and a View Engine implementation for each of them - ASP.NET MVC provides extensibility points to override the default View Engine (the Web Forms View Engine). 
Almost by definition, Views (and the Engines that render them) should not include much logic, let alone such advance logic that would necessitate Dependency Injection to manage.
Thus, in the context of Dependency Injection the ability to inject View Engines is not very important, but is important to mention regardless, as the need for such functionality may arise. 
If such a scenario occurs, the solution is: System.Web.Mvc.ViewEngines.Engines.Add([IViewEngine]);