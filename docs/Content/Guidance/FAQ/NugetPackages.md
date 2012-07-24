#### I get an `System.OutOfMemoryException` while installing or updating a package?

For packages with many/deep dependencies, NuGet uses a good deal of memory. 
Since most of our machines are 32-bit and each process can access only 2GB of memory, this is often a problem.

**Workaround**:

1.  Close all instances of Visual Studio
2.  Open the solution and try again.
3.  If it continues to be an issue, try to install one of the lower-level dependencies first, then retry the original package.  
    For instance, if *DowJones.Web.Mvc* depends on *DowJones.Infrastructure* and attempts to install *DowJones.Web.Mvc* fail with an `OutOfMemoryException`, try to install the *DowJones.Infrastructure* package first, then attempt the *DowJones.Web.Mvc* package.

#### A Release build succeeds, but the new version of the package does not show in the feed

Packages should be available immediately (within a few seconds) of the successful build. 
In rare cases they might take a minute or two to show up, but it should never be more than a few minutes. 
If a Release build succeeds that is supposed to generate a new version of a package in our custom feeds but you do not see a new version, something probably went wrong.

Unfortunately, there is not much you can do about this, as it is probably a backend/server issue that needs to be addressed by an admin. 
You can, however, attempt to try the build again. To do this, select the `Queue New Build...` option in the build’s context menu to trigger  a new build.

If this does not fix the issue, contact a build administrator.