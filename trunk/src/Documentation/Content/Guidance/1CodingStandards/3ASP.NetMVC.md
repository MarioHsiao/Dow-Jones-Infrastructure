1. Inherit from Important base classes (`DowJonesHttpApplication`, `ControllerBase`, etc.)  
Some key base classes in the framework offer powerful and relatively universal functionality.  In some cases, many parts of the Framework will simply not work unless you inherit from these classes. Two of the most important base classes are `DowJones.Web.Mvc.DowJonesHttpApplication` and `DowJones.Web.Mvc.Infrastructure.ControllerBase`.

2. Depend on abstractions  
Abstractions encourage loosely-coupled systems with a healthy separation of contracts and implementations.  Abstractions are easily interchanged which not only provides easier maintenance, but is also crucial to unit testing.

3. Avoid the `New` Keyword  
Any time you employ the new keyword to create a new instance of a concrete type you are – by definition – not depending on an abstraction.  Though this is often not a problem at all (e.g. new StringBuilder(), new List<T>(), etc.), take a moment any time you use the new keyword to consider if the object you are creating might be better expressed as a dependency to be injected.  Let another component create it!

4. Avoid referring to HttpContext directly (use HttpContextBase)  
ASP.NET MVC (and later, .NET 4) introduced System.Web.Abstractions, a set of abstractions over many of the core parts of the ASP.NET framework.  
The 'depend on abstractions' advice extends to these classes as well.  In particular, one of the most often referenced objects in ASP.NET development is HttpContext – prefer using the HttpContextBase abstraction instead.

5. Avoid `magic strings`  
'Magic strings' – crucial, yet arbitrary string values may be convenient and in many situations even required, however they have many issues.  
Some of the biggest issues with magic strings are as below:  
	a.	don't have any intrinsic meaning  (e.g. it's difficult to tell how or if one 'ID' relates to another 'ID').  
	b.	are easily broken with misspelling or case sensitivity.   
	c.	don't react well to refactoring.    
	d.	promote rampant, pervasive duplication.  

	Here are two examples, the first using magic strings to access data in a ViewData dictionary, and the second refactored example with that same data in a strongly-typed model:

	**Using magic strings**:

		<p>
		<label for="FirstName">First Name:</label>		
		<span id="FirstName">@@ViewData["FirstName"]</span>
		</p>

	**Using a strongly-typed model**:

		<p>
		<label for="FirstName">First Name:</label>
		<span id="FirstName">@@Model.FirstName</span>
		</p>

	Magic strings carry the allure of being very simple to use when you introduce them, but that ease of use often comes back to bite you later when it comes time to maintain them.

6. Prefer `View.Model` over `View.ViewData`  
As the preceding example shows, the ViewData dictionary is one of the most tempting places to leverage magic strings in an ASP.NET MVC application.  However, strongly-typed Presentation Models can be a handy tool to avoid assigning and retrieving data directly to and from the ViewData dictionary.

7. Do not write HTML in `back end` code   
Follow the practice of separation of concerns:  it is not the responsibility of controllers and other 'back-end code' to render HTML.  
The exceptions here, of course, are UI helper methods and classes whose only job is to help the views render code.  These classes should be considered part of the view, not 'back-end' classes.

8. Do not perform business logic in views  
The inverse of the previous practice is true as well:  views should not contain any business logic.  In fact, views should contain as little logic as possible!  Views should concentrate on how to display data that they have been provided, not take action on that data.

9. Consolidate commonly-Used View Snippets with Helper Methods  
The notion of 'user controls,' 'server controls,' and simply 'controls' in general is very widespread and for good reason.
These concepts help consolidate commonly-used code and logic in a central location to make it easier to reuse and maintain.
ASP.NET MVC is not control-driven, however – instead, it relies on the `helper method` paradigm in which methods do the work that controls once did.  This can pertain to an entire section of HTML (what we're used to calling a 'control'), or even as simple as strongly-typed access to a commonly-referred URL.  
For example, you may notice many of the same references to the 'Membership Page' (~/membership) like so:

		@@Html.ActionLink("Membership", "Index", "Membership", […])

You can instead consolidate this call (and eliminate the magic strings!) by turning it into a helper method:

		@@Html.MembershipLink()

10. Prefer Presentation Models Over Direct Usage of business objects
In general, try to avoid allowing changes to the business model to directly affect the view.  

11. Encapsulate View 'if' statements with Html Helpers
Integrating code and markup is quite powerful; however, it can get quite messy.
Consider the following (relatively simple) if-else statement:

	**Index.cshtml**

		@@if(Model.IsAnonymousUser) {
	
		}

That's quite obscure code for rendering out essentially the same markup with the exception of one part (the URL).
Consider this approach instead:

   **UserHtmlHelperExtensions.cs**

		