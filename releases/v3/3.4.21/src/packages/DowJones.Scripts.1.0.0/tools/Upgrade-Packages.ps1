###########################################################
#
# Script to upgrade all NuGet packages in solution to latest version
# https://gist.github.com/1082558
#
# USAGE
# Place this file (Upgrade-Packages.ps1) to your solution folder. 
# From Package Manager Console execute
#
# .\Upgrade-Packages.ps1 -PackageFilter:Castle.*
# or
# .\Upgrade-Packages.ps1 Castle.*
# or just
# .\Upgrade-Packages.ps1
# to upgrade all NuGet packages in the solution
#
##########################################################

param($PackageFilter = "DowJones.*")

$packageManager = $host.PrivateData.packageManagerFactory.CreatePackageManager()

foreach ($project in Get-Project -all) {
	$fileSystem = New-Object NuGet.PhysicalFileSystem($project.Properties.Item("FullPath").Value) 	
	$repo = New-Object NuGet.PackageReferenceRepository($fileSystem, $packageManager.LocalRepository)

	foreach ($package in $repo.GetPackages() | ? {$_.Id -like $PackageFilter}) {
		Update-Package $package.Id -Project:$project.Name 
	}
}