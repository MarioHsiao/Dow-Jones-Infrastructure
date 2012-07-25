#### PREFER the dependency injection pattern

The [Inversion of Control pattern](http://en.wikipedia.org/wiki/Inversion_of_control) in general - and the [dependency injection pattern](http://en.wikipedia.org/wiki/Dependency_injection) in specific - help create loosely-coupled, highly maintainable, and easily-testable applications.

#### AVOID the singleton pattern

The Singleton pattern is a direct violation of the dependency injection pattern since it cannot be replaced. Feel free to use the singleton scope, but configure the DI framework to manage the lifetime of the
object for you, don't let the object manage itself.

#### AVOID checking in commented-out code

Commented code does not contribute to the application - it merely makes it more difficult to maintain.

Don't be concerned about "losing" code that you might use later - our version control system ensures that that will never happen!

#### PREFER objects over long lists of parameters

Long parameter lists are commonly considered a code smell and should be avoided whenever possible. Consider the two contrasting snippets:

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

        public ActionResult Create(Employee employee)
        {
            employee.Save();
            return View("Details", employee);
        }

The Parameter Object example is much more straight-forward and easier to maintain.
