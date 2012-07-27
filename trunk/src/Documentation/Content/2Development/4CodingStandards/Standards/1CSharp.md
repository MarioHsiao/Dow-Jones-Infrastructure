####Naming Conventions####

1. Use Pascal casing for types, properties, method names and constants.

	a. The abbreviation ID should only be used as an identifier suffix and must appear uppercase at all times.

		public class SomeClass
		{
			private const int DefaultSize = 100;
			public int CurrentUserID { get; set; } 
			public void GetHTML() 
			{…}
		}

2.	Use camel casing for local variable names and method arguments.
	
	a.	Acronyms should be all lowercase.
	b.	Combination words such as ILot should be all lowercase.
	c.	The abbreviation ID should only be used as an identifier suffix and must appear uppercase at all times.

		private void MyMethod(int itemID)
		{
			int number;
		}

3.	**Never** prepend variables with type information (i.e. Hungarian notation)

		public class SomeClass 
		{
		// Correct:
			private int _number;

		// Avoid:
			private int i_number;
		}

4.	Prefix private member variables with an underscore.  Use camel case after the underscore.

		public class SomeClass
		{
			private int _number;
		}

5.	Prefix interface names with I.

		interface IMyInterface
		{…}

6.	Do not use scope indicators; rely instead on identifier casing and leading underscores.

		// Correct:
		private int _componentID;

		// Avoid:
		private int ThisComponentID;
		private int m_componentID;

7.	Explicitly state visibility with access modifiers, even if this restates the default visibility.

		// Correct:
		private Chart GetChart(…)
		{…}

		// Avoid:
		Chart GetChart(…)
		{…}

8.	Never suffix any identifier with a reserved word.

		// Correct:
		IPermissionManager PermissionManager { get; }

		// Avoid:
		IPermissionManager PermissionManagerInterface { get; }
		
10.	Suffix custom exception classes with Exception.

11.	Name extension classes with the type they extend, suffixed with Extensions.

		public static class StringExtensions

12.	Name methods using verb-object pair, such as ShowDialog().

13.	Methods with return values should describe the type returned, such as GetComponent().

14.	Use descriptive variable names.

	a.	Avoid using Hungarian notation – even when using it the right way.
	b.	Avoid single character variable names, such as i or t. Use index or temp instead.
	c.	Emphasize semantics, not structure: prefer the name components to items.
	d.	Do not abbreviate words (such as num instead of number).

15.	Always use C# keywords rather than the aliases in the System namespace.

		// Correct:
		object item1;
		string item2;
		int item3;

		// Avoid:
		Object item1;
		String item2;
		Int32 item3;

16.	Prefix generics with T

	a.	Reserve suffixing Type when dealing with the .NET type Type.

		// Correct:
		public class LinkedList<TKey,TEntity> 
		{…}

		// Avoid:
		public class LinkedList<KeyType,EntityType> 
		{…}

####Code layout####

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
			{…}

			// Methods (in functional order and/or alphabetical order)
			public void DoSomething()
			protected void DoSomethingElse()
			private void SomeHelper()

			// Static Methods
			public static void CreateSomeClass()
			{…}
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

####Exceptions####

53.	Catch only exceptions for which you have explicit handling.

54.	In a catch statement that throws an exception, always throw the original exception (or another exception constructed from the original exception) to maintain the stack location of the original error:

			catch(Exception exception) 
			{
				MessageBox.Show(exception.Message); 
				throw;
			}

55.	Avoid error codes as method return values.

56.	Avoid 'Expection Handling'.
Exceptions should not be the rule.

57.	When defining custom exceptions:
	a.	Derive the custom exception from Exception.
	b.	Ensure serialization is possible.
	
####Best Practises ####

58.	Become familiar with the architecture of the project – avoid re-inventing the wheel.

59.	Avoid clipboard inheritance.  Prefer refactoring.

60.	Do not manually edit any machine-generated code.

61.	If modifying machine generated code, modify the format and style to match this coding standard.

62.	Use partial classes whenever possible to factor out the maintained portions.

63.	Avoid comments that explain the obvious.
Code should be self-explanatory.
Good code with readable variable and method names should not require comments.

64.	Document only operational assumptions, algorithm insights and so on.  
Comments describing program flow indicate the need for 'Extract Method' refactoring.

65.	Encapsulate business rules within a class:

		Thing currentThing = new Thing();

		// Correct:
		if (currentThing.IsValid)
		{
			currentThing.Save();
		}

		// Avoid:
		if (currentThing.Value > 1 && currentThing.OtherValue == 2)
		{
			currentThing.Save();
		}

66.	Declare a constants where needed; do not use magic numbers.

67.	Use the const directive only on natural constants such as the number of days of the week.

68.	Avoid using const on read-only variables. For that, use the readonly directive.

		public class MyClass
		{
			public const int DaysInWeek = 7; 
			public readonly int Number; 
			public MyClass(int someValue) 
			{
				Number = someValue;
			}
		}

69.	Do not initialize value types to their default value.

70.	Prefer an enumerated type over a Boolean for method parameters:

		// Correct:
		public State PerformOperation(State currentState)
		{…}

		// Avoid:
		public State PerformOperation(bool isReady)

71.	Avoid multiple Main() methods in a single assembly.

72.	Make only the most necessary types public, mark others as internal.

73.	Avoid friend assemblies, as they increase inter-assembly coupling.

74.	Avoid code that blindly relies on an assembly running from a particular location.  Use of a structured settings mechanism to handle this is acceptable.

75.	Minimize code in application assemblies.  Use class libraries instead to contain business logic.

76.	Avoid providing explicit values for enums unless they are bit flags or the enum is generated by a tool.

77.	Avoid using the ternary conditional operator for complex logic; simple a or b assignments are acceptable.

78.	Avoid using pre-processor directives.  Use conditional attributes to exclude method calls:

		[Conditional("MySpecialCondition")]
		public void MyMethod()
		{…}

79.	Avoid function calls in Boolean conditional statements.
Assign into local variables and check on them.

		private bool IsEverythingOK() 
		{…}

		// Avoid:
		if (IsEverythingOK()) 
		{…}

		// Correct:
		bool ok = IsEverythingOK(); 
		if (ok)
		{…}

80.	Always use zero-based arrays.

81.	With indexed collection, use zero-based indexes

82.	Do not provide public or protected member variables. Use properties instead.

83.	Use automatically implemented properties when there is no business logic or validation for the property.

84.	Avoid using the new inheritance qualifier.  
Use override instead (ensuring consistent operation for polymorphic use)

85.	Never use unsafe code, except when using interop.

86.	Avoid explicit casting. Use the as operator to defensively cast to a type.

		Dog dog = new GermanShepherd();
		GermanShepherd shepherd = dog as GermanShepherd;
		if (shepherd != null)
		{…}

87.	Always check a delegate for null before invoking it.

88.	Do not provide public event member variables. Use event accessors instead.

		public class MyPublisher
		{
			MyDelegate _someEvent;

			public event MyDelegate SomeEvent 
			{
				add
				{
					_someEvent += value;
				}
				remove
				{
					_someEvent -= value;
				}
			}
		}

32.	Avoid defining event-handling delegates. Use EventHandler<T> instead.

33.	Favor the use of interfaces.

34.	Classes and interfaces should have at least 2:1 ratio of methods to properties.

35.	Avoid data classes (with nothing but properties).  
Use the 'Move Method' refactoring.

36.	Avoid interfaces with one member.
Interfaces as just a naming container aren't good either.

37.	Strive to have three to five members per interface.

38.	Do not have more than 20 members per interface.
Twelve is probably the practical limit.

39.	Avoid events as interface members.

40.	When using abstract classes, offer an interface as well.

41.	Expose interfaces on class hierarchies.

42.	Prefer using explicit interface implementation.

43.	Never assume a type supports an interface.
Defensively query for that interface.

		SomeType obj1;
		IMyInterface obj2;

		/* Some code to initialize obj1, then: */
		obj2 = obj1 as IMyInterface;
		if (obj2 != null)
		{
			obj2.Method1();
		}
		else
		{
			// Handle error in expected interface
		}

44.	Always code to the most abstract interface or type that supports the feature required:

		private void RespondToButtonEvent(object sender, EventArgs e)
		{
			// Correct:
			IButtonControl button = sender as IButtonControl;
			if (button != null)
			{
				string argument = button.CommandArgument; // property is defined by IButtonControl
			}

			// Avoid:
			Button button = sender as Button; // Explicit reference makes refactoring more complex
			{…}
		}

45.	Use application logging and tracing.

46.	Never use goto unless in a switch statement fall-through, or to exit a nested for loop.

47.	Always have a default case in a switch statement that asserts.

		int number = SomeMethod();
		switch (number)
		{
			case 1:
				Trace.WriteLine("Case 1:"); 
				break;
			case 2:
				Trace.WriteLine("Case 2:"); 
				break;
			default:
				Debug.Assert(false); 
				break;
		}

48.	Do not use the this reference unless invoking another constructor from within a constructor.

		// Correct use of 'this':
		public class MyClass
		{
			public MyClass(string message) 
			{}
			public MyClass() : this("Hello") 
			{}
		}

49.	Do not use GC.AddMemoryPressure().

50.	Do not rely on HandleCollector.

51.	Always run code unchecked by default (for the sake of performance), but explicitly in checked mode for overflow- or underflow-prone operations:

		private int CalcPower(int number, int power)
		{
			int result = 1;
			for (int count = 1; count <= power; count++) 
			{
				checked
				{
					result *= number;
				}
			}
			return result;
		}

52.	Avoid casting to and from System.Object in code that uses generics. Use constraints or the as operator instead:

		private class SomeClass
		{}

		// Correct:
		private class MyClass<T> where T : SomeClass 
		{
			private void SomeMethod(T t) 
			{
				SomeClass obj = t;
			}
		}

		// Avoid:
		private class MyClass<T> 
		{
			private void SomeMethod(T t) 
			{
				object temp = t;
				SomeClass obj = (SomeClass)temp;
			}
		}

53.	Do not define constraints in generic interfaces. Interface-level constraints can often be replaced by strong-typing.

		public class Customer 
		{…}

		// Correct:
		public interface ICustomerList : IList<Customer> 
		{…}

		// Avoid:
		public interface IList<T> where T : Customer 
		{…}

54.	Do not define method-specific constraints in interfaces.

55.	Do not define constraints in delegates.

56.	If a class or a method offers both generic and non generic flavors, always prefer using the generics flavor.

57.	When implementing a generic interface that derives from an equivalent non-generic interface (such as IEnumerable<T>), use explicit interface implementation on all methods, and implement the non-generic methods by delegating to the generic ones:
		public class MyCollection<T> : IEnumerable<T>
		{
			IEnumerator<T> IEnumerable<T>.GetEnumerator() 
			{…}

			IEnumerator IEnumerable.GetEnumerator() 
			{
				IEnumerable<T> enumerable = this;
				return enumerable.GetEnumerator(); 
			}
		}

58.	Prefer generic methods to entire generic classes.

59.	Always throw semantically useful exceptions from public methods.

60.	Avoid parameters that control method flow; ensure each parameter is necessary for computation.
