17.	Classes should follow this general pattern.  Comments are for illustration only.

		public class SomeClass
		{
			// Static variables first
			private static object _expensiveObject;

			// Constants and readonly variables
			private const SomeConstant;
			private readonly int _someReadonlyNumber;

			// Member variables
			private int _number;

			// Properties (backing fields should be directly below the property)
			public int Property
			{
				get { return _property; }
			}
			private int _property;

			public string AnotherProperty { get; set; }

			// Events
			public event DelegateName EventName;

			// Constructors
			public SomeClass()
			{�}

			// Methods (in functional order and/or alphabetical order)
			public void DoSomething()
			protected void DoSomethingElse()
			private void SomeHelper()

			// Static Methods
			public static void CreateSomeClass()
			{�}
		}

18.	Maintain strict indentation using tabs.  Do not use spaces for formatting.

19.	Indent comments at the same level of indentation as the code you are documenting.

20.	Declare a local variable as close as possible to its first use.

21.	File names should match the name of the class/artifacts within 

22.	When multiple classes inherit from the same base class without any new implementation, they may be located in the same file.  The file name should be the plural for the base class:

		// In CountAttributes.cs
		public class OneAttribute : CountAttribute {}
		public class TwoAttribute : CountAttribute {}
		public class ThreeAttribute : CountAttribute {}

23.	When using partial types and allocating a part per file, name each file after the logical role that part plays.

		For example:
		// In MyClass.cs
		public partial class MyClass 
		{...}

		// In MyClass.Generated.cs
		public partial class MyClass 
		{...}

24.	Avoid putting multiple classes in a single file.

25.	A single file should contribute types to only a single namespace. Avoid having multiple namespaces in the same file.

26.	As a general rule:

	a.	avoid files with more than 500 lines.  
	b.	avoid methods with more than 30 lines.

27.	Avoid methods with more than 5 arguments. Use structures for passing multiple arguments.

28.	Lines should not require excessive scrolling to read.

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
			{�}

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

52.	Do not 'new up' service instances � prefer dependency injection.