param($installPath, $toolsPath, $package, $project)

#### Modify Global.asax ####
$globalAsax = $project.ProjectItems | where { $_.Name -eq "Global.asax" }
if($globalAsax) {
	
	# Replace the contents of Global.asax.cs with Global.asax.cs.custom
	$globalAsaxCs = $globalAsax.ProjectItems | where { $_.Name -eq "Global.asax.cs" }
	$customGlobalAsax = $project.ProjectItems | where { $_.Name -eq "Global.asax.cs.custom" }
	if($globalAsaxCs -and $customGlobalAsax) {
		Write-Host "Error handling implementation.."

		# Read the transformed text from the custom template included in the package
		$customGlobalAsax.Open()
		$customGlobalAsax.Document.Activate()
		$customGlobalAsax.Document.Selection.SelectAll(); 
		$replacementGlobalAsax = $customGlobalAsax.Document.Selection.Text;

		$globalAsaxCs.Open()
		$globalAsaxCs.Document.Activate()
		$globalAsaxCs.Document.Selection.SelectAll()
		$globalAsaxCs.Document.Selection.Insert($replacementGlobalAsax)
		$globalAsaxCs.Document.Selection.StartOfDocument()
		$globalAsaxCs.Document.Close(0)

		$customGlobalAsax.Delete()
	}
}


#### Fold Web.config transforms underneather Web.config ####
$WebConfig = $project.ProjectItems | where { $_.Name -eq "Web.Config" }

## Remove Web.Debug.config and Web.Release.config added by the Studio
$WebConfigDebug = $WebConfig.ProjectItems | where { $_.Name -eq "Web.Debug.config" }
if($WebConfigDebug){
	$WebConfigDebug.Delete()
}

$WebConfigRelease = $WebConfig.ProjectItems | where { $_.Name -eq "Web.Release.config" }
if($WebConfigRelease){
	$WebConfigRelease.Delete()
}

# Web.BC.config # 
$WebConfigBC = $project.ProjectItems | where { $_.Name -eq "Web.BC.config" }
if($WebConfigBC){
	$WebConfigBC.Open()
	$WebConfig.ProjectItems.AddFromFileCopy($WebConfigBC.Document.FullName)
}

# Web.Beta.config # 
$WebConfigBeta = $project.ProjectItems | where { $_.Name -eq "Web.Beta.config" }
if($WebConfigBeta){
	$WebConfigBeta.Open()
	$WebConfig.ProjectItems.AddFromFileCopy($WebConfigBeta.Document.FullName)
}

# Web.Integration.config # 
$WebConfigIntegration = $project.ProjectItems | where { $_.Name -eq "Web.Integration.config" }
if($WebConfigIntegration){
	$WebConfigIntegration.Open()
	$WebConfig.ProjectItems.AddFromFileCopy($WebConfigIntegration.Document.FullName)
}

# Web.Production.config # 
$WebConfigProduction = $project.ProjectItems | where { $_.Name -eq "Web.Production.config" }
if($WebConfigProduction){
	$WebConfigProduction.Open()
	$WebConfig.ProjectItems.AddFromFileCopy($WebConfigProduction.Document.FullName)
}

# Web.Staging.config # 
$WebConfigStaging = $project.ProjectItems | where { $_.Name -eq "Web.Staging.config" }
if($WebConfigStaging){
	$WebConfigStaging.Open()
	$WebConfig.ProjectItems.AddFromFileCopy($WebConfigStaging.Document.FullName)
}

#### Inheriting all the controllers by AbstractController except Abstract and Error controllers ####
$controllers = $project.ProjectItems | where {$_.Name -eq "Controllers"} | ForEach-Object { $_.ProjectItems } | where { $_.Name.EndsWith("Controller.cs") }
foreach	($controller in $controllers) {
	if($controller.Name -ne "ErrorController.cs" -and $controller.Name -ne "AbstractController.cs"){
		$controller.Open()
		$controller.Save()
		$controller.Document.Activate()
		$controller.Document.Selection.SelectAll()
		$controller.Document.Selection.ReplaceText(": DowJones.Web.Mvc.ControllerBase", ": AbstractController")
		$controller.Document.Close(0)
	}
}

#### Relacing HomeController and Index view if they exist or else just copy from the pacakge ####
Write-Host "Overwriting Home Controller content..."
$homeController = $controllers | where { $_.Name -eq "HomeController.cs" }
if($homeController){
	$homeController.Delete()
}

$project.ProjectItems.Item("Controllers").ProjectItems.Item("HomeController.cs.custom").Name = "HomeController.cs"

Write-Host "Overwriting Home Index View..."

$views = $project.ProjectItems | where {$_.Name -eq "Views"}
$homeIndexView = $views.ProjectItems | where {$_.Name -eq "Home"} | ForEach-Object { $_.ProjectItems } | where{ $_.Name -eq "Index.cshtml" }
if($homeIndexView){
	$homeIndexView.Delete()
}

$project.ProjectItems.Item("Views").ProjectItems.Item("Home").ProjectItems.Item("Index.cshtml.custom").Name = "Index.cshtml"

