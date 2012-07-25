Much like ASP.NET MVC Controllers, the Windows Communication Foundation (WCF) runtime expects service classes to contain a default (parameterless) constructor and will not instantiate an instance of the service without one. 

Just as ASP.NET MVC provides an extension point via custom Controller Factories, WCF also provides similar functionality in the form of custom Service Host Factories. 
And - just as with ASP.NET MVC Controller Factories - some dependency injection frameworks provide custom Service Host Factory implementations to make it much easier and transparent to leverage Dependency Injection with WCF services. 
When DI frameworks do not provide their own implementations, developers can take this task into their own hands and create their own relatively easily.
