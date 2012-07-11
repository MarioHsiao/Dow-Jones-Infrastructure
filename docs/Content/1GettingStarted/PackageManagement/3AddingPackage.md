You can add, remove, and update packages using either the Add Library Package Reference dialog box or by using PowerShell command-line commands in the Package Manager Console window. 
The following illustration shows the Add Library Package Reference dialog box.

<img src="@Url.Content("~/Content/images/NuGet_AddPackageReference.png")" alt="Nuget Add Package Reference">

In order to install a package, simply select the package and click "Install". 
The package manager will automatically download and configure the package in your project.

After the package has been sucessfully installed, you should see it listed in your project's References and the files should reside in the 'packages' folder in your solution's root folder.

For example, if your project's folder path is

	C:\Code\TestSolution\TestProject

then your packages folder path should be

	C:\Code\TestSolution\packages

#### NuGet Packages and Version Control
By default, NuGet attempts to add packages to version control when working in a version-controlled project. 
Be sure to include the package changes in your checkins.