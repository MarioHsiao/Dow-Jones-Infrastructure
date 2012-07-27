29.	Use meaningful namespaces, such as:

			namespace DowJones.Web.Mvc.UI.Components

30.	Avoid fully qualified type names.  Prefer the using statement instead.

31.	Group all framework namespaces together, followed by third-party and custom namespaces.

			using System;
			using System.Collections.Generic;
			using ThirdParty.Library;
			using DowJones.Web.Mvc;
			using DowJones.Web.Mvc.UI.Components;

32.	Remove unused using statements.

33.	Use Action<> or Func<> over delegates

34.	All comments should pass spell checking

35.	All humorous identifier names should be funny.  Obscure humor should be commented.

			StringBuilder bob = new StringBuilder();
			StreamWriter paulBunyan = new StreamWriter(logfile); // get it? log... Paul Bunyan?

36.	Prefer Automatic Properties

			// Correct:
			public string Name { get; set; }

			public ClassName()
			{
			Name = DefaultName;
			}

			// Avoid:
			public string Name
			{
				get { return _name; }
				set { _name = value; }
			}
			private string _name = DefaultName;

37.	Avoid accessing property backing fields directly:
			public string Name
			{
				get { return _name; }
				set { _name = value; }
			}
			private string _name;

			// Correct:
			Name = "Bob";

			// Avoid:
			_name = "Bob";

38.	Limit type and member accessibility to the most private possible level.

39.	Avoid Inner classes

40.	Always place an open curly brace ({) in a new line, except for single line property accessors and auto-implemented properties.

41.	The only acceptable use of multiple semicolons on a single line is when initializing a for loop.

42.	With Lambda expressions, mimic the code layout of a regular method, aligned with the delegate declaration. Omit the variable type and rely on type inference, yet use parentheses:

			public delegate void SomeDelegate(string someString);

			SomeDelegate someDelegate = (name) => 
                            {
                                Trace.WriteLine(name); 
                                MessageBox.Show(name); 
                            };
43.	Only use in-line Lambda expressions when they contain a single simple statement.  Avoid multiple statements that require a curly brace or a return statement with in-line expressions.  Omit parentheses:

			public delegate void SomeDelegate(string someString);

			public void MyMethod(SomeDelegate someDelegate) 
			{…}

			// Correct:
			MyMethod(name => MessageBox.Show(name));

			// Avoid:
			MyMethod((name) => {Trace.WriteLine(name);MessageBox.Show(name);});

44.	Always use a curly brace scope in an if statement, even if it conditions a single statement.

	a.	Flow-control statements are exempt from this rule; specifically the keywords return, break and continue.  
	b.	You may throw on the same line, assuming you're not creating a new Exception.  
	c.	You may return on the same line, as long as you don't need multiple line initialization:  

			// Correct:
			if (isFoo) return new Bar();

			// Avoid:
			if (isFoo) return new Bar
			{
				Thing = "Something",
				Something = "Something else"
			};

45.	Favor the traditional C == equality comparison over .Equals(a,b).

46.	When building a long string (more than two concatenations), use StringBuilder, not string, except in the explicit case when you are joining only string literals.

47.	Avoid excessive use of StringBuilder; use an external template instead.

48.	Never hard-code strings that will be presented to end users. Use tokens.

49.	Never hard-code strings that might change based on deployment environment. Use settings.

50.	Use string.Empty instead of "":

			string name;

			/* Some conditional code to initialize name, then: */
			if (isUnknown)
			{
				// Correct:
				name = String.Empty;

				// Avoid:
				name = "";
			}

51.	Avoid providing methods on structures.

52.	Do not 'new up' service instances – prefer dependency injection.