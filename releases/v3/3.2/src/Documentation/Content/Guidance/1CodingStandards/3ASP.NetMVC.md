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

10. Prefer Presentation Models Over Direct Usage of business objects.    
In general, try to avoid allowing changes to the business model to directly affect the view.  

11. Encapsulate View `if` statements with Html Helpers.  
Integrating code and markup is quite powerful; however, it can get quite messy.
Consider the following (relatively simple) if-else statement:

	**Index.cshtml**

		@@if(Model.IsAnonymousUser) {
			<img src="Url.Content("~/content/images/anonymous.jpg")" />
		} else if(Model.IsAdministrator) {
			<img src="Url.Content("~/content/images/administrator.jpg")" />
		} else if(Model.Membership == Membership.Standard) {
			<img src="Url.Content("~/content/images/member.jpg")" />
		} else if(Model.Membership == Membership.Preferred) {
			<img src="Url.Content("~/content/images/preferred_member.jpg")" />
		}

		}

	That's quite obscure code for rendering out essentially the same markup with the exception of one part (the URL).
	Consider this approach instead:

    **UserHtmlHelperExtensions.cs**

	   public static string UserAvatar(this HtmlHelper<User> helper)
		{
			var user = helper.ViewData.Model;

			string avatarFilename = "anonymous.jpg";

			if (user.IsAnonymousUser)
			{
				avatarFilename = "anonymous.jpg";
			}
			else if (user.IsAdministrator)
			{
				avatarFilename = "administrator.jpg";
			}
			else if (user.Membership == Membership.Standard)
			{
				avatarFilename = "member.jpg";
			}
			else if (user.Membership == Membership.Preferred)
			{
				avatarFilename = "preferred_member.jpg";
			}

			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			var contentPath = string.Format("~/content/images/{0}", avatarFilename);
			string imageUrl = urlHelper.Content(contentPath);

			return string.Format("<img src='{0}' />", imageUrl);
		}

	**Index.cshtml**  (and everywhere else you need the user's avatar)

		@@Html.UserAvatar()

	Not only is this cleaner, it's also more declarative and moves this logic into a central location so that it may be more easily maintained.  
	For instance, if the requirements change and the site needs to support custom avatars, the Html.UserAvatar helper method can be modified in one place.

12. Use Script Registry and Stylesheet Registry instead of direct includes (even for snippets)  
The Dow Jones MVC Framework introduces the Script Registry and Stylesheet Registry concepts to better manage script and style includes.  
These Registries provide you with a central location to manage these resources, as well as 'behind the scenes' optimizations such as script combining, minifying, etc.  
To best take advantage of these optimizations, leverage these Registries as opposed to implementing these things yourself.

13. Prefer explicit View names  
A majority of the ASP.NET MVC controller action code samples call the View() method without specifying a view name.  
This is suitable for simple demo code, however when tests or other action methods begin calling each other, the detriments to this approach become clear.  
When no view name is specified, the ASP.NET MVC framework defaults to the name of the action that was *originally* called.  
Thus, calling the Index action in the following example will attempt to locate a view named 'Index.cshtml' – a view that probably doesn't exist (but 'List.cshtml' certainly does!):

	**EmployeeController.cs**

		public ActionResult Index()
        {
            return List();
        }

        public ActionResult List()
        {
            var employees = Employee.GetAll();
            return View(employees);
        }
	
	If the List action is modified to call the View() method with a specific view name (as shown below), everything works fine.
		public ActionResult List( )
        {
            var employees = Employee.GetAll();
            return View("List", employees);
        }
		
14. Prefer objects over long lists of parameters.  
This advice is not specific to ASP.NET MVC - long parameter lists are commonly considered a 'code smell' and should be avoided whenever possible.  
Additionally, ASP.NET MVC's powerful Model Binders make following this advice incredibly easy.  
Consider the two contrasting snippets:

	**Long Parameter List**

		public ActionResult Create(
				string firstName, string lastName, DateTime? birthday,
				string addressLine1, string addressLine2,
				string city, string region, string regionCode, string country
				[... and many, many more]
			)
		{
			var employee = new Employee( [Long list of parameters...] )
			employee.Save();
			return View("Details", employee);
		}

	**Parameter Object**

		public ActionResult Create(Employee employee)
		{
			employee.Save();
			return View("Details", employee);
		}

	The Parameter Object example is much more straight-forward, and leverages the ASP.NET MVC framework's powerful Model Binders and model validation to make this code much safer and easier to maintain.

15. Encapsulate shared/common functionality, logic, and data with Action Filters or Child Actions (Html.RenderAction).    
Every website of any significant complexity will have common elements across multiple (or perhaps all) pages in the application.  
  
    A global website navigation menu – the kind that appears on every single page in the site - is a canonical example of this type of globally-applied logic and content.  
  
    The data for these common elements needs to come from somewhere, yet explicitly retrieving the data in every controller action would create a maintenance nightmare.  
   
    Action Filters and/or child actions (via the Html.RenderAction method) provide a central location to hold this kind of logic.
    Consider the following layout snippet (cut from the larger layout page) which renders navigation items in a list:

		<ul id="global-menu">
		@@foreach (var menuItem in ViewData.SingleOrDefault<NavigationMenu>()) {
				<li class="(menuItem.IsSelected ? "selected" : null)">				
				@@Html.RouteLink(menuItem.DisplayName, menuItem.RouteData)
				</li>
		} 
		</ul>

	The **NavigationMenu** **ViewData** object needs to come from somewhere.
	Since they can be configured to execute prior to every controller request, Action Filters make an excellent candidate to populate View Data with globally-required data like this.
	Here is the Action Filter that populates the **NavigationMenu** data required in the previous example:

		public class NavigationMenuPopulationFilter : ActionFilterAttribute
		{
			private readonly INavigationDataSource _dataSource;

			public NavigationMenuPopulationFilter(INavigationDataSource dataSource)
			{
				_dataSource = dataSource;
			}

			public override void OnActionExecuting(ActionExecutingContext filterContext)
			{
				NavigationMenu mainMenu = _dataSource.GetNavigationMenu("factiva-main-menu");
				filterContext.Controller.ViewData["MainNavigationMenu"] = mainMenu;
			}
		}

	This Filter is pretty straight-forward – it gets the correct navigation menu data model from some data source and adds it to the View Data collection prior to executing the requested action. 
	From this point on, any component that requires it can retrieve the navigation menu from View Data.
	As with most Action Filters, there are several ways to apply the NavigationMenuPopulationFilter:  either apply the attribute to your controllers or actions, or register the filter in the Global Action Filters collection.  
	Below is a bootstrapper task for adding the Action Filter to the list of  global filters:

		public class GlobalFilterRegistrationTask : IBootstrapperTask
		{
			[Inject]
			public NavigationMenuPopulationFilter NavigationFilter { get; set; }

			public void Execute()
			{
				GlobalFilters.Filters.Add(NavigationFilter, 4);
			}
		}

16.	Prefer placing Action Filters at the 'highest appropriate level'.  
Most Action Filter attributes can be applied at either the method (Action) or class (Controller) level.
When an attribute applies to all actions in a controller, prefer placing that attribute on the controller itself rather than on each individual class.
Also consider whether or not the attribute may be appropriate further up the controller's dependency chain (i.e. on one of its base classes) instead.

17. Prefer partial views (or entirely separate views) over complex if-then-else logic that shows and hides sections.  
The Page Controller pattern of Web Forms encourages posting back to the same page, possibly showing or hiding certain sections of the page depending on the request.
Due to ASP.NET MVC's separation of concerns, this can often be avoided by creating separate views for each of these situations, lowering or eliminating entirely the need for complex view logic.  
Consider the following example:

    **Wizard.cshtml**

			@@if(Model.WizardStep == WizardStep.First) {
				<!-- The first step of the wizard -->
			} else if(Model.WizardStep == WizardStep.Second) {
				<!-- The second step of the wizard -->
			} else if(Model.WizardStep == WizardStep.Third) {
				<!-- The third step of the wizard -->
			}

	Here the view is deciding which step of the Wizard to display, which is dangerously close to business logic!  Let's move this logic to the Controller where it belongs and split this view into multiple views:

    **WizardController.cs**

			public ActionResult Step(WizardStep currentStep)
			{
				// This is simple logic, but could be MUCH more complex!
				string view = currentStep.ToString();

				return View(view);
			}

	**First.cshtml**
			<!-- The first step of the wizard -->
	
	**Second.cshtml**
			<!-- The second step of the wizard -->

	**Third.cshtml**
			<!-- The third step of the wizard -->

18. Prefer the Post-Redirect-Get pattern When Posting Form Data.  
The Post/Redirect/Get (PRG)  pattern is a common design pattern for web developers to help avoid certain duplicate form submissions and allow user agents to behave more intuitively with bookmarks and the refresh button.  
Due to the Page Controller nature of Web Forms in which developers are usually required to post back to the same page for all actions in a particular context (e.g. display employee data so that it may be edited and re-submitted), the PRG pattern is not used as much in Web Forms environments.
Because ASP.NET MVC separates actions into separate URLs it is easy to run into trouble with update scenarios.  
For instance,

	**EmployeeController.cs**

			public class EmployeeController : Controller
			{
				public ActionResult Edit(int id)
				{
					var employee = Employee.Get(id);

					return View("Edit", employee);
				}

				[AcceptVerbs(HttpVerbs.Post)]
				public ActionResult Update(int id)
				{
					var employee = Employee.Get(id);

					UpdateModel(employee);

					return View("Edit", id);
				}
			}

	In this example when a user posts to the Update action, though the user will be looking at the 'Edit' view as desired, the resulting URL in their browser will be '/employees/update/1'.
	If the user refreshes the page or bookmarks a link to that URL, etc. subsequent visits would update the employee information again or even not work at all.
	What we really want to happen in the Update action is to update the Employee information and then redirect the user back to the Edit page so that they are back to the original 'Edit' location.  
	In this scenario, the PRG pattern may be applied thusly:

	 **EmployeeController.cs**  (partial, showing only the changed Update action)

		   [AcceptVerbs(HttpVerbs.Post)]
			public ActionResult Update(int id)
			{
				var employee = Employee.Get(id);

				UpdateModel(employee);

				return RedirectToAction("Edit", new { id });
			}

	Though it's a subtle change, switching from the View() method to the RedirectToAction() method will produce a client-side redirect (as opposed to a 'server-side redirect' in the original example) after the Update method has finished updating the employee, landing the user on the proper URL:  '/employees/edit/1'.

19. Prefer Bootstrapper tasks over logic placed in Application_Start (Global.asax).  
Most ASP.NET MVC demos will advise modifying the Application_Start method in the Global.asax file in order to introduce logic that will execute when the application starts.
While this is certainly the easiest and most straight-forward approach, the Dow Jones MVC Framework provides the concept of 'bootstrapper tasks'.
These tasks are easy to implement (simply implement the IBootstrapperTask interface – the Execute() method) and are automatically discovered (merely by existing in referenced assembly) and executed during the Application_Start phase of the application.
These Tasks help provide cleaner code and encourage proper adherence of the Single Responsibility principle.

20. Prefer Authorize attribute over imperative security checks.  
Traditionally, authorization control resembles the following:
		
			public ActionResult Details(int id)
			{
				if (!User.IsInRole("EmployeeViewer"))
					return new HttpUnauthorizedResult();

	This is a very imperative approach, and makes it difficult to implement application-wide changes.  
	The ASP.NET MVC AuthorizeAttribute provides a simple and declarative way to authorize access to actions.  
	This same code may be rewritten as:	

			[Authorize(Roles = "EmployeeViewer")]
			public ActionResult Details(int id)
			{
		
21. Prefer creating a custom layout for mobile devices.  
Mobile device browsers operate under a completely different environment than desktop browsers.
It is difficult enough to worry about the intricacies of various desktop browsers, let alone adding mobile support to the mix.
Instead of flooding your 'default' (desktop-focused) views with mobile-conditional logic, separate the mobile views into their own files.
In fact, the Dow Jones MVC Framework even offers support for targeting specific mobile devices.
Take advantage of these capabilities whenever possible!

22. Prefer using the Route attribute over More Generic Global Routes.  
Routing is one of the most powerful aspects of ASP.NET 4.0.
The default route that ships with the ASP.NET MVC template ('{controller}/{action}/{id}') is derived from what is considered the most common routing pattern for 'standard' MVC applications.
This pattern is not universal, however, and is commonly augmented or replaced with additional custom routes.
When determining new routes for an application, the more specific you can be the better.
Otherwise, your application may experience matching on unintended routes.
Of course, the most specific route is one that maps directly to one action and one action only.
The Dow Jones MVC Framework provides you with this ability via Custom Routing (with the RouteAttribute), allowing you to specify a custom route for a particular action.
Prefer applying this attribute when you desire non-standard routing logic over adding a more generic custom global route.

23. Consider using an Anti-Forgery Token to avoid CSRF attacks.  
For form posts where security is a concern, ASP.NET MVC provides measures to help deter certain kinds of common attacks.
One of these measures is the Anti-forgery Token.
The Token has both server- and client-side components.

    **Client Side:** 	Simply call the Html.AntiForgeryToken helper method at some point inside your form

			@@using(Html.Form("Update", "Employee”)) {
				@@Html.AntiForgeryToken()
				<!-- rest of form goes here -->
			}

    **Server Side:**  	Apply the ValidateAntiForgeryTokenAttribute to the destination action (the action that is receiving the form post data)
		
			[ValidateAntiForgeryToken]
			[AcceptVerbs(HttpVerbs.Post | HttpVerbs.Put)]
			public ActionResult Update(int id)
			{

    This code will insert a user-specific token in a hidden field on your form and validate that token on the server side prior to executing any further processing of the data being posted.
			
24. Consider the AcceptVerbs attribute to restrict how actions may be called.  
Many Actions rest on a number of assumptions about how and when they will be called in the context of an application.
For instance, one assumption might be that an Employee.Update action will be called from some kind of Employee Edit page containing a form with the Employee properties to post to the Employee.
Update action in order to update an Employee record.
If this action is called in an unexpected way (e.g. via a GET request with no form posts), the action will probably not work, and in fact may produce unforeseen problems.
The ASP.NET MVC framework offers the AcceptVerbs attribute to help restrict action calls to specific Http Methods.
Thus, the answer to the aforementioned Employee.Update scenario would be:

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Put)]
        public ActionResult Update(int id)

	Applying the AcceptVerbs attribute in this way will restrict requests to this action only to those made specifying the POST or PUT Http Methods.
	All others (e.g. GET requests) will be ignored. 

25. Consider output caching.  
Output caching is one of the easiest ways to get additional performance from a web application.
Caching requests in which little or no content has changed since the previous request is a quick way to speed up your request times.
The ASP.NET MVC framework offers the OutputCacheAttribute to accomplish this task.
This attribute mirrors the Web Forms output caching functionality and accepts many of the same properties.
	
26.	Consider custom ActionResults for unique scenarios.  
The ASP.NET MVC Request Pipeline has a deliberate separation of concerns in which each step in the process completes its task and no more.
Each step does merely enough to provide the subsequent tasks with enough information to do what they need to do.
For instance, a controller action that decides a view should be rendered to the client does not load up a view engine and order it to execute the view, it merely returns a ViewResult object with the information that the Framework needs to take the next steps (most likely loading a view engine and executing the view!).
When it comes to results of controller actions, declarative is the name of the game.
For instance, the ASP.NET MVC Framework provides an HttpStatusCodeResult with a StatusCode property, but it also goes one step further to define a custom HttpStatusCodeResult named HttpUnauthorizedResult.
Though the following two lines are effectively the same, the latter provides a more declarative and strongly-typed expression of the controller's intent.

		return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

		return new HttpUnauthorizedResult();

	When your actions produce results that don't fit the 'normal results' take a moment to consider whether returning a custom Action Result may be more appropriate.
	Some common examples include things like RSS feeds, Word documents, Excel spreadsheets, etc.
	
27.	Consider Async Controllers for controller tasks that can happen in parallel.  
Parallel execution of multiple tasks can offer significant opportunities to enhance the performance of your site.
To this end, ASP.NET MVC offers the AsyncController base class to help make processing parallelizable requests easier.
When creating an action with processor-intensive logic, consider whether that action has any elements that may be safely run in parallel.
See the ASP.NET MVC Asynchronous Controller documentation  for more information.













		