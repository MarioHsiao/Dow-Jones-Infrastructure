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

