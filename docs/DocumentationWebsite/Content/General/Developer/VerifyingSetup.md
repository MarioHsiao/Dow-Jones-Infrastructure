After completing all of the above installations and configurations, you can verify everything is correct by creating a new Dow Jones <<ViewComponents>> solution using the following steps:

* Open Visual Studio and choose `File -> New Project -> Class Library`.  Name the project whatever you like.
* Add the NuGet package named 'DowJones.Web.Mvc.UI.Canvas'
* Use the "Dow Jones View Component" item template to add a new view component named 'Test'.  This should create (at least) the following files
	* TestComponent.cshtml
	* *TestComponent.cshtml.cs*  '(if you do not see this file, you have not properly installed the Razor View Component Class Generator plugin)'
	* TestModel.cs
* Replace the contents of `TestComponent.cshtml` with the following:
<pre><code>@@model TestModel
Hello, world!
</code></pre>
* Save the file and check that the `TestComponent.cshtml.cs` file was updated with your _Hello, World!_ changes
* Save the file and compile the solution - *everything should compile correctly*