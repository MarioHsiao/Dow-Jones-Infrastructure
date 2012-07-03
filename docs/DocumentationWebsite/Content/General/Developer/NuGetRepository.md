Dow Jones projects leverage NuGet packages to manage our external dependencies. 
In addition to the public NuGet package repository, the Dow Jones Platform Integration Team hosts several custom NuGet repositories which you need to add to your NuGet configuration in Visual Studio:

#### Main Repository

  [http://montana.dev.us.factiva.com/nuget/main/repository](http://montana.dev.us.factiva.com/nuget/main/repository)

or...

  <a href="file:///fsegdevw1.win.dowjones.net\WebSites\nuget\main\Packages">\\\fsegdevw1.win.dowjones.net\WebSites\nuget\main\Packages</a>

#### Beta Packages::

  [http://montana.dev.us.factiva.com/nuget/beta/repository](http://montana.dev.us.factiva.com/nuget/beta/repository) 

or...

  <a href="file:///fsegdevw1.win.dowjones.net\WebSites\nuget\beta\Packages">\\\fsegdevw1.win.dowjones.net\WebSites\nuget\beta\Packages</a>

See <<PackageManagement>> for more details on how to configure NuGet package management.