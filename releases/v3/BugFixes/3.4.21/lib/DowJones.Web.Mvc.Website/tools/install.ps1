param($installPath, $toolsPath, $package, $project)

#### Modify Global.asax ####
$globalAsax = $project.ProjectItems | where { $_.Name -eq "Global.asax" }
if($globalAsax) {
	
	# Fold Global.asax.generated.cs underneather Global.asax
	$globalAsaxGenerated = $project.ProjectItems | where { $_.Name -eq "Global.asax.generated.cs" }
	$globalAsaxGenerated.Open()
	$globalAsax.ProjectItems.AddFromFileCopy($globalAsaxGenerated.Document.FullName)

	# Replace the contents of Global.asax.cs with Global.asax.cs.custom
	$globalAsaxCs = $globalAsax.ProjectItems | where { $_.Name -eq "Global.asax.cs" }
	$customGlobalAsax = $project.ProjectItems | where { $_.Name -eq "Global.asax.cs.custom" }
	if($globalAsaxCs -and $customGlobalAsax) {
		Write-Host "Setting Application to DowJones.Web.Mvc.HttpApplication..."

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
	} else {
		Write-Host "Global.asax.cs or Global.asax.cs.custom not found -- skipping application base type update"
	}
}

	
	
#### Modify the Razor pageBaseType #### 
Write-Host "Setting default Razor base page to DowJones.Web.Mvc.UI.WebViewPage..."
$viewsWebConfig = $project.ProjectItems | where {$_.Name -eq "Views"} | ForEach-Object { $_.ProjectItems } | where {$_.Name -eq "Web.config"}

$viewsWebConfig.Open()
$viewsWebConfig.Save()
$viewsWebConfig.Document.Activate()
$viewsWebConfig.Document.Selection.SelectAll()
$viewsWebConfig.Document.Selection.ReplaceText("pageBaseType=""System.Web.Mvc.WebViewPage""", "pageBaseType=""DowJones.Web.Mvc.UI.WebViewPage""")
$viewsWebConfig.Document.Close(0)


#### Modify the BaseController #### 
Write-Host "Setting base controller to DowJones.Web.Mvc.ControllerBase..."
$controllers = $project.ProjectItems | where {$_.Name -eq "Controllers"} | ForEach-Object { $_.ProjectItems } | where { $_.Name.EndsWith("Controller.cs") }

foreach	($controller in $controllers) {
	$controller.Open()
	$controller.Save()
	$controller.Document.Activate()
	$controller.Document.Selection.SelectAll()
	$controller.Document.Selection.ReplaceText(": Controller", ": DowJones.Web.Mvc.ControllerBase")
	$controller.Document.Close(0)
}



#### Overwrite _Layout.cshtml ####
Write-Host "Overwriting _Layout.cshtml..."

$sharedViews = $project.ProjectItems | where {$_.Name -eq "Views"} | ForEach-Object { $_.ProjectItems } | where {$_.Name -eq "Shared"}

# Read the transformed text from the custom template included in the package
$customLayout = $sharedViews.ProjectItems | where { $_.Name -eq "_Layout.cshtml.custom" }
$customLayout.Open()
$customLayout.Document.Activate()
$customLayout.Document.Selection.SelectAll(); 
$replacementLayout = $customLayout.Document.Selection.Text;
$customLayout.Delete()

# Replace the contents of _Layout.cshtml
$layout = $sharedViews.ProjectItems | where { $_.Name -eq "_Layout.cshtml" }
if($layout) {
	$layout.Open()
	$layout.Document.Activate()
	$layout.Document.Selection.SelectAll()
	$layout.Document.Selection.Insert($replacementLayout)
	$layout.Document.Selection.StartOfDocument()
	$layout.Document.Close(0)
} else {
	Write-Host "_Layout.cshtml not found -- skipping update"
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

$homeSearchView = $views.ProjectItems | where {$_.Name -eq "Home"} | ForEach-Object { $_.ProjectItems } | where{ $_.Name -eq "Search.cshtml" }
if($homeSearchView){
	$homeSearchView.Delete()
}

$project.ProjectItems.Item("Views").ProjectItems.Item("Home").ProjectItems.Item("Index.cshtml.custom").Name = "Index.cshtml"
$project.ProjectItems.Item("Views").ProjectItems.Item("Home").ProjectItems.Item("Search.cshtml.custom").Name = "Search.cshtml"

#### Delete Account Model, View and Controller ####
$accountModel = $project.ProjectItems | where { $_.Name -eq "Models" } | ForEach-Object { $_.ProjectItems } | where { $_.Name -eq "AccountModels.cs" }
if($accountModel){
	$accountModel.Delete()
}
$accountView = $views.ProjectItems | where { $_.Name -eq "Account" }
if($accountView){
	$accountView.Delete()
}
$accountController = $controllers | where { $_.Name -eq "AccountController.cs" }
if($accountController){
	$accountController.Delete()
}


#### Delete Scripts folder ####
$scripts = $project.ProjectItems | where { $_.Name -eq "Scripts" }
if($scripts){
	$scripts.Delete()
}

#### Delete themes folder and site.css ####
$content = $project.ProjectItems | where { $_.Name -eq "Content" }
$themes = $content.ProjectItems | where { $_.Name -eq "themes" }
if($themes){
	$themes.Delete()
}
$siteCss = $content.ProjectItems | where { $_.Name -eq "site.css" }
if($siteCss){
	$siteCss.Delete()
}

#### Delete _LogOnPartial.cshtml partial view ####
$logOnPartial = $sharedViews.ProjectItems | where { $_.Name -eq "_LogOnPartial.cshtml" }
if($logOnPartial){
	$logOnPartial.Delete()
}



