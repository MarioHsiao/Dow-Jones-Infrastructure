The Dow Jones Core Framework provides bare bones, low-level infrastructure which acts as the foundation for all other blocks.
This core framework provides key classes and interfaces that you'll normally wouldn't see in your web application.

The Dow Jones Core Framework provides following key capabilities:


Feature																| Description
--------------------------------------------------------------------|---------------------------------------------------------------------------------------------
Dependency Injection												| Using ninject, we all clients change aspects of the framework at run-time rather than compile time.  It allows clients to inject objects into our classes, rather than relying on a class to create the object itself. Also, it helps reduce depency coupling, allowing construction logic to be reused on a broader level.
Session Management													| Utilities/abstractions to facilitate in  management of **Factiva Platform Session Management**.  These classes contain well defined business logic to work with the cookies set by **Login Server** in the "Factiva Platform Ecosystem". 
Security Management													| Utilities/abstractions to facilitate the interpretation and retrival of **Factiva Platform** based entitilements.
Localization Management												| Utilities/abstractions for use in the localization of web sites. Localization, is scoped to a site and all the resources used within.   
Client Resource Management											| Utilities/abstractions to facilitate the adding, packaging, localization, optimizations and run-time retrival of Scripts and/or Css resources.
Core Extensions														| Provide many useful extensions to base classes. For Instance, the ability to serialize any class to a JSON object.
Assemblers/TypeMappers												| Class(es) whos sole purpose is to transform data from one source to another.  TypeMappers represent a more source-to-target one-to-one mapping, while Assemblers tend to encapsulate well defined business logic.
Managers															| Class(es) that manage connectivity/data acquisition to the **Third-Party** services as well as **Factiva Platform** services like *Search*, *Archive*, Etc.
Formatters															| Class(es) whos sole purpose is to manage the display of certain types of data. For instance, `DateTimeFormatter` handles the formatting of dates.  `NumberFormmatter` handles the formatting of numbers. [Stocks, Percentages etc.]
Proxy Handlers														| Class(es) that provide the ability to make external resoruce calls to third-party sites/services that are not on the calling sites domain.   
UI Component Framework												| A comprehensive framework for creating UI Components that can be used on your site or put into a a centralized library to be used by other sites.