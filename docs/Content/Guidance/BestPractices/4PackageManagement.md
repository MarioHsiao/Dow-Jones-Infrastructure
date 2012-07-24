### ALWAYS check your Pending Changes to make sure that package references will be checked in

NuGet tries its best to maintain the version-controlled state of the
*packages* folder. In most cases, NuGet will add new packages and clean
up (delete) unused package folders. But, sometimes NuGet can’t do this
and you need to add or remove these folders yourself. Sometimes, even
when NuGet has done its job perfectly, the package files show up as
Pending Changes, yet are not checked to be commited with other changes!

Regardless of how good or bad a job that NuGet does, it is up to you to
ensure that your changes make it into source control. **Always review
the Pending Changes list to ensure that your package files are checked
in correctly.**

### ALWAYS use the "All" package source to install Dow Jones packages

Not only does this ensure that you get the latest package version from
the correct repository, it also ensures that NuGet will be able to
properly resolve cross-repository package references.

It is usually a good idea to use the "All" package source even for
non-Dow Jones packages.

### PREFER the *UpgradePackages.ps1* script to upgrade all projects in a solution

As of the current version, all of NuGet’s operations work against
individual projects and it does not offer a way to upgrade all
references in a solution.

You can feel free to manually upgrade every individual package in every
single project, but for efficiency’s sake it is recommended that you use
the *UpgradePackages.ps1* script to upgrade all packages and projects at
once.

### PREFER the official NuGet documentation

NuGet is not a proprietary tool. It is an open source project created
within Microsoft and has excellent documentation and community support
on its websites ([http://nuget.org](http://nuget.org) and
[http://nuget.codeplex.org](http://nuget.codeplex.org)). If you have
questions or experience issues that this guide does not address, please
refer to the official NuGet documentation prior to elevating the
question within Dow Jones.