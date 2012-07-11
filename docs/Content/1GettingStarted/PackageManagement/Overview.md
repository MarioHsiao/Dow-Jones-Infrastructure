Dow Jones uses the [NuGet](http://nuget.org/) package management system to manage external dependencies and references. 
Each individual NuGet package represents a version of a particular library or API. 
The [NuGet Package Manager](http://visualstudiogallery.msdn.microsoft.com/27077b70-9dad-4c64-adcf-c7cf6bc9970c) is a Visual Studio extension that allows for easy management of NuGet packages within a Visual Studio solution.

NuGet packages (really just ZIP archives with some metadata) are downloaded from a remote repository, expanded locally, and stored in a folder named 'packages' in the current solution.  
Then, the NuGet Package Manager adds the appropriate Project References (on a per-project basis) to the local assemblies in the 'packages' folder.  
As such, the contents of the 'packages' folder and its children is crucial to the build process, so *always be sure to check in changes to the 'packages' folder*.

If you have questions that this page does not adequately answer, please visit the [official NuGet documentation site](http://nuget.codeplex.com/documentation) for additional information on how to configure, use, and create NuGet packages.