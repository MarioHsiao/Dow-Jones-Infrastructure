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
	
