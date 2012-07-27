Since the dependency inversion principle generally precludes the ability for components to directly create instances of other components that they depend upon, they must advertise these dependencies to the higher-level components responsible for providing them. 
This can be implemented in a few ways:

Service Location
    While the techniques above are effective ways to advertise dependencies to be satisfied in a passive manner, some components may wish to take a more active approach to retrieve their dependencies by requesting them from a Service Locator object like so:

        var dataService = ServiceLocator.Get<IStockQuoteDataService>();

   Warning: this technique is listed here for documentation purposes.
   It is not actually a form of dependency injection and **should be avoided at all costs**. In other words, **use this only as a last resort**!

Constructor Injection
    Using this approach, dependencies are declared in the object's constructor. 
	In this way, the object cannot be created without fulfilling the dependency. 
	Due to the fact that dependencies must be fulfilled upon creation of the object, **Constructor Injection** is the most favorable approach**. 
	Following is an example of constructor injection:

        public StockQuoteAnalysisService(IStockQuoteDataService dataService)

Property Injection
    Like Constructor Injection, dependencies can be advertised via object properties though which consumers of the object may assign the dependencies after an instance of the object has been created.
    This approach is most suitable for situations when a dependency is "optional" (i.e. a suitable default exists), or when constructor injection cannot be used (e.g. a circular dependency exists). 
	An example of property injection is:

        public IStockQuoteDataService StockQuoteDataService { get; set; }

Method Injection
    Method Injection describes the scenario in which a dependency is provided in the parameters of a method. 
	Because the two previous methods easily handle the majority of situations, the Method Injection approach is rarely used, but can still be helpful at times. 
	Generally speaking, this approach is helpful in two scenarios:

   *Setter method* - this is largely synonymous to Property Injection, but instead of setting a property directly, a method (e.g. `SetHostname(hostname)`) is used. 
	This allows classes to properly encapsulate and protect their properties. 
	Because of the power provided by .NET's property getters and setters, this method is rarely needed.

   *Command/template method* - in some scenarios, a class may contain logic that executes against another component that may be different for any given execution of the method. 
	Since this other component may change freely while the rest of the logic remains the same, the best way to handle this is typically by passing (injecting) the other component via a method parameter.
    A classic example is that of an `XmlWriter`: `new XElement("body").WriteTo(xmlWriter);` In this example, we are passing in our instance of `xmlWriter` to the `.WriteTo()` method. 
	Thus, the higher-level components remain in full control `xmlWriter`, including its concrete type and implementation details such as the file path it's writing to.