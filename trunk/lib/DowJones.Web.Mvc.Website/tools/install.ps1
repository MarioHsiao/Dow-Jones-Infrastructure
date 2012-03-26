param($installPath, $toolsPath, $package, $project)

#### Overwrite Global.asax ####
Write-Host "Setting Application to DowJones.Web.Mvc.HttpApplication..."

# Read the transformed text from the custom template included in the package
$customGlobalAsax = $project.ProjectItems | where { $_.Name -eq "Global.asax.cs.custom" }
$customGlobalAsax.Open()
$customGlobalAsax.Document.Activate()
$customGlobalAsax.Document.Selection.SelectAll(); 
$replacementGlobalAsax = $customGlobalAsax.Document.Selection.Text;
$customGlobalAsax.Delete()

# Replace the contents of Global.asax.cs
$globalAsax = $project.ProjectItems | ForEach-Object { $_.ProjectItems } | where { $_.Name -eq "Global.asax.cs" }
$globalAsax.Open()
$globalAsax.Document.Activate()
$globalAsax.Document.Selection.SelectAll()
$globalAsax.Document.Selection.Insert($replacementGlobalAsax)
$globalAsax.Document.Selection.StartOfDocument()
$globalAsax.Document.Close(0)


	
#### Modify the Razor pageBaseType #### 
Write-Host "Setting default Razor base page to DowJones.Web.Mvc.UI.WebViewPage..."
$viewsWebConfig = $project.ProjectItems | where {$_.Name -eq "Views"} | ForEach-Object { $_.ProjectItems } | where {$_.Name -eq "Web.config"}

$viewsWebConfig.Open()
$viewsWebConfig.Save()
$viewsWebConfig.Document.Activate()
$viewsWebConfig.Document.Selection.SelectAll()
$viewsWebConfig.Document.Selection.ReplaceText("pageBaseType=""System.Web.Mvc.WebViewPage""", "pageBaseType=""DowJones.Web.Mvc.UI.WebViewPage""")
$viewsWebConfig.Document.Close(0)



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

# Replace the contents of Global.asax.cs
$layout = $sharedViews.ProjectItems | where { $_.Name -eq "_Layout.cshtml" }
$layout.Open()
$layout.Document.Activate()
$layout.Document.Selection.SelectAll()
$layout.Document.Selection.Insert($replacementLayout)
$layout.Document.Selection.StartOfDocument()
$layout.Document.Close(0)
